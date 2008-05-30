using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.ComponentModel;
using FuzzLab.VoiceRecognition;
using FuzzLab.SpeechSynthesis;
using FuzzLab.Mp3Player;
using FuzzLab.Utility;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityManager
    {
        public delegate void NewPersonalityLoadedHandler(Personality personality);
        public event NewPersonalityLoadedHandler NewPersonalityLoaded;

        public delegate void PercentCompletedHandler(int totalItems, int completedItems);
        public event EventHandler<PlaylistProgressEventArgs> PlaylistLoadProgressChanged;

        public PersonalityManager(CommandRecognizer recognizer, Talker talker, IMp3Player mp3Player)
        {
            m_recognizer = recognizer;
            m_talker = talker;
            m_mp3Player = mp3Player;

            Mp3Player.ActivePlaylistChanged += new EventHandler(AddCurrentPlaylistToRecognizer);
            Mp3Player.ActivePlaylistContentsChanged += new EventHandler(AddCurrentPlaylistToRecognizer);
            Mp3Player.ListOfPlaylistsChanged += new EventHandler(AddPlaylistsToRecognizer);
        }

        public Personality LoadPersonality(string filename)
        {
            do
            {
                m_stopAddingPlaylistEvent.Set();
            }
            while (!Monitor.TryEnter(m_stopAddingPlaylistEvent, 100));

            try
            {
                do
                {
                    m_stopAddingPlaylistListEvent.Set();
                }
                while (!Monitor.TryEnter(m_stopAddingPlaylistListEvent, 100));

                try
                {
                    EventLog.Instance.AddEvent(EventType.Information, "VoodooVoice.Personality", "Loading personality from: " + filename);
                    StreamReader reader = new StreamReader(filename);
                    Personality personality = (Personality)m_serializer.Deserialize(reader);

                    if (personality != null)
                    {
                        if (m_activePersonality != null)
                        {
                            UnloadPersonality();
                        }
                        m_activePersonality = personality;

                        m_activePersonality.Manager = this;
                        m_activePersonality.SourceFile = filename;
                        EventLog.Instance.AddEvent(EventType.Information, "VoodooVoice.Personality", "Successfully loaded personality: " + m_activePersonality.Name);

                        EventLog.Instance.AddEvent(EventType.Information, "PersonalityManager", "Adding basic voice commands...");

                        // Add all commands to the recognizer and Commit it
                        m_activePersonality.AddToCommandRecognizer();

                        Recognizer.Commit();

                        // Notify anyone who cares that a new personality is loaded
                        RaiseNewPersonalityLoaded(m_activePersonality);

                        // Activate the first mode
                        if (m_activePersonality.Modes.Count > 0)
                        {
                            m_activePersonality.Modes[0].Activate();
                        }

                        // Add percentages to recognizer
                        for (int i = 0; i <= 100; ++i)
                        {
                            Recognizer.AddNumberToList("percent", i);
                        }

                        EventLog.Instance.AddEvent(EventType.Information, "PersonalityManager", "Non-playlist voice commands are now ready to be used!");

                        AddPlaylistsToRecognizer(Mp3Player, new EventArgs());
                        AddCurrentPlaylistToRecognizer(Mp3Player, new EventArgs());

                        EventLog.Instance.AddEvent(EventType.Information, "PersonalityManager", "The playlist has been loaded and all voice commands are now available.");
                    }
                    m_personalities.Add(m_activePersonality);
                    return m_activePersonality;
                }
                finally
                {
                    Monitor.Exit(m_stopAddingPlaylistListEvent);
                }
            }
            finally
            {
                Monitor.Exit(m_stopAddingPlaylistEvent);
            }
        }

        private List<Personality> m_personalities = new List<Personality>();

        private void UnloadPersonality()
        {
            Recognizer.RemoveAllModes();
        }

        public CommandRecognizer Recognizer
        {
            get { return m_recognizer; }
        }

        public Talker Talker
        {
            get { return m_talker; }
        }

        public IMp3Player Mp3Player
        {
            get { return m_mp3Player; }
        }

        public Personality ActivePersonality
        {
            get { return m_activePersonality; }
            set { m_activePersonality = value; }
        }

        protected void RaiseNewPersonalityLoaded(Personality personality)
        {
            NewPersonalityLoadedHandler temp = NewPersonalityLoaded;
            if (temp != null)
            {
                temp(personality);
            }
        }

        protected bool RaisePlaylistLoadProgressChanged(int totalItems, int completedItems)
        {
            EventHandler<PlaylistProgressEventArgs> temp = PlaylistLoadProgressChanged;
            if (temp != null)
            {
                PlaylistProgressEventArgs e = new PlaylistProgressEventArgs();
                e.TotalItems = totalItems;
                e.CompletedItems = completedItems;
                temp(this, e);
                return e.Cancel;
            }
            return false;
        }

        private void AddPlaylistsToRecognizer(object sender, EventArgs e)
        {
            // If this function is called while another thread is already executing this function, we want
            // to cancel the old one and execute this new call instead.

            do
            {
                m_stopAddingPlaylistListEvent.Set();
            }
            while (!Monitor.TryEnter(m_stopAddingPlaylistListEvent, 100));

            try
            {
                m_stopAddingPlaylistListEvent.Reset();

                Recognizer.RemoveAllPhrasesFromList("playlistName");

                if (m_stopAddingPlaylistListEvent.WaitOne(0, false) == false)
                {
                    Recognizer.Commit();
                }

                foreach (IPlaylist playlist in Mp3Player.Playlists)
                {
                    if (m_stopAddingPlaylistListEvent.WaitOne(0, false) == true)
                    {
                        break;
                    }

                    Recognizer.AddPhraseToList("playlistName", playlist.Name);
                }

                if (m_stopAddingPlaylistListEvent.WaitOne(0, false) == false)
                {
                    Recognizer.Commit();
                }
            }
            finally
            {
                Monitor.Exit(m_stopAddingPlaylistListEvent);
            }
        }

        private void AddCurrentPlaylistToRecognizer(object sender, EventArgs e)
        {
            // If this function is called while another thread is already executing this function, we want
            // to cancel the old one and execute this new call instead.

            do
            {
                m_stopAddingPlaylistEvent.Set();
            }
            while (!Monitor.TryEnter(m_stopAddingPlaylistEvent, 100));

            try
            {
                m_stopAddingPlaylistEvent.Reset();

                Recognizer.RemoveAllPhrasesFromList("artist");
                Recognizer.RemoveAllPhrasesFromList("title");

                if (m_stopAddingPlaylistEvent.WaitOne(0, false) == false)
                {
                    Recognizer.Commit();
                }

                IPlaylist playlist = Mp3Player.ActivePlaylist;
                bool cancel = false;
                if (playlist != null)
                {
                    int totalTracks = playlist.TrackCount;
                    int tracksAdded = 0;

                    foreach (ITrack track in playlist.Tracks)
                    {
                        if (m_stopAddingPlaylistEvent.WaitOne(0, false) == true)
                        {
                            break;
                        }

                        if ((cancel = RaisePlaylistLoadProgressChanged(totalTracks, tracksAdded)) == true)
                        {
                            break;
                        }

                        if (track.Artist != null && track.Artist.Length > 0)
                        {
                            Recognizer.AddPhraseToList("artist", track.Artist);
                        }
                        if (track.Name != null && track.Name.Length > 0)
                        {
                            Recognizer.AddPhraseToList("title", track.Name);
                        }

                        ++tracksAdded;
                    }

                    cancel = RaisePlaylistLoadProgressChanged(totalTracks, tracksAdded);
                }

                if (!cancel && m_stopAddingPlaylistEvent.WaitOne(0, false) == false)
                {
                    Recognizer.Commit();
                }
            }
            finally
            {
                Monitor.Exit(m_stopAddingPlaylistEvent);
            }
        }

        private XmlSerializer m_serializer = new XmlSerializer(typeof(Personality));
        private CommandRecognizer m_recognizer;
        private Talker m_talker;
        private IMp3Player m_mp3Player;
        private Personality m_activePersonality = null;
        private ManualResetEvent m_stopAddingPlaylistEvent = new ManualResetEvent(true);
        private ManualResetEvent m_stopAddingPlaylistListEvent = new ManualResetEvent(true);
    }
}
