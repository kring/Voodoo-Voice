using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime;
using FuzzLab.VoiceRecognition;
using FuzzLab.SpeechSynthesis;
using FuzzLab.Mp3Player;
using System.Windows.Forms;

namespace FuzzLab.VoodooVoice
{
    public class VoodooVoice
    {
        public delegate void PersonalityManagerCreatedHandler(PersonalityManager manager);
        public event PersonalityManagerCreatedHandler PersonalityManagerCreated;
        public event EventHandler ThreadEnding;

        public VoodooVoice()
        {
            m_thread = new Thread(ThreadEntryPoint);
        }

        public void Start()
        {
            m_thread.Start();
        }

        public void Stop()
        {
            m_stop = true;
            m_applicationContext.ExitThread();
        }

        public bool IsStopping
        {
            get { return m_stop; }
        }

        private IEnumerable<string> Initialize()
        {
            yield return "Loading MP3 player plugin...";
            IMp3PlayerPlugin mp3PlayerPlugin = LoadMp3PlayerPlugin();
            if (mp3PlayerPlugin == null)
            {
                yield return "The MP3 player plugin specified in the configuration file, " + Properties.Settings.Default.Mp3PlayerPlugin + ", could not be found.";
                yield return "Try reinstalling Voodoo Voice.";
                yield break;
            }

            yield return "Initializing Speech Recognition...";
            CommandRecognizer recognizer = null;
            try
            {
                recognizer = new CommandRecognizer();
            }
            catch (Exception)
            {
            }
            if (recognizer == null)
            {
                yield return "Speech recognition could not be initialized.  Try reinstalling Voodoo Voice.";
                yield break;
            }
            
            yield return "Initializing Speech Synthesis...";
            Talker talker = new Talker();

            yield return "Initializing " + mp3PlayerPlugin.Name + "...";
            IMp3Player mp3Player = mp3PlayerPlugin.LoadMp3Player();
            if (mp3Player == null)
            {
                foreach (string line in mp3PlayerPlugin.Mp3PlayerUnavailableMessage)
                {
                    yield return line;
                }
                yield break;
            }

            yield return "Initializing Voodoo Voice Personality Manager...";
            m_personalityManager = new PersonalityManager(recognizer, talker, mp3Player);
            m_personalityManager.PlaylistLoadProgressChanged += new EventHandler<PlaylistProgressEventArgs>(PlaylistLoadPercentCompleted);
            RaisePersonalityManagerCreated(m_personalityManager);

            yield return "Loading personality...";
            try
            {
                m_personality = m_personalityManager.LoadPersonality(Properties.Settings.Default.Personality);
            }
            catch (FileNotFoundException)
            {
                // Specified personality not found, so attempt to load the default personality instead.
                m_personality = m_personalityManager.LoadPersonality("DefaultPersonality.psn3");
            }
        }

        private IMp3PlayerPlugin LoadMp3PlayerPlugin()
        {
            AssemblyName name = new AssemblyName(Properties.Settings.Default.Mp3PlayerPlugin);
            Assembly assembly = Assembly.Load(name);

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface("FuzzLab.Mp3Player.IMp3PlayerPlugin") != null)
                {
                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                    return constructor.Invoke(null) as IMp3PlayerPlugin;
                }
            }

            return null;
        }

        protected void RaisePersonalityManagerCreated(PersonalityManager manager)
        {
            PersonalityManagerCreatedHandler temp = PersonalityManagerCreated;
            if (temp != null)
            {
                temp(manager);
            }
        }

        private void ThreadEntryPoint()
        {
            try
            {
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                foreach (string s in Initialize())
                {
                    EventLog.Instance.AddEvent(EventType.Information, "MainThread", s);
                    Application.DoEvents();
                    if (m_stop)
                    {
                        break;
                    }
                }
                if (!m_stop)
                {
                    Application.Run(m_applicationContext);
                }
            }
            catch (InvalidOperationException)
            {
            }

            EventHandler threadEnding = ThreadEnding;
            if (threadEnding != null)
            {
                threadEnding(this, new EventArgs());
            }
        }

        void  Application_ApplicationExit(object sender, EventArgs e)
        {
 	        Stop();
        }

        void PlaylistLoadPercentCompleted(object sender, PlaylistProgressEventArgs e)
        {
            // Process events so that speech recognition continues to work
            e.Cancel = m_stop;
            Application.DoEvents();
        }

        private Thread m_thread = null;
        private PersonalityManager m_personalityManager = null;
        private Personality m_personality = null;
        private volatile bool m_stop = false;
        private ApplicationContext m_applicationContext = new ApplicationContext();
    }
}
