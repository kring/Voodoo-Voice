using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.Mp3Player;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSwitchTo : PersonalityAction
    {
        [XmlAttribute("title")]
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        [XmlAttribute("artist")]
        public string Artist
        {
            get { return m_artist; }
            set { m_artist = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            string title = m_title;
            string artist = m_artist;

            if (arguments.ContainsKey("title"))
            {
                title = arguments["title"];
            }
            if (arguments.ContainsKey("artist"))
            {
                artist = arguments["artist"];
            }

            if ((title != null && title.Length > 0) ||
                (artist != null && artist.Length > 0))
            {
                ITrackList matching = manager.Mp3Player.ActivePlaylist;
                if (title != null && title.Length > 0)
                {
                    matching = matching.FindByTitle(title);
                }
                if (artist != null && artist.Length > 0)
                {
                    matching = matching.FindByArtist(artist);
                }

                foreach (ITrack track in matching.Tracks)
                {
                    if ((title == null || title.Length == 0 || track.Name == title) &&
                        (artist == null || artist.Length == 0 || track.Artist == artist))
                    {
                        track.Play();
                        break;
                    }
                }
            }
        }

        public override string ToString()
        {
            // TODO: this is wrong - needs to include artist
            string switchTo;
            if (m_title != null)
            {
                switchTo = m_title;
            }
            else if (m_artist != null)
            {
                switchTo = "something by " + m_artist;
            }
            else
            {
                switchTo = "title/arist";
            }
            return "Switch To " + switchTo;
        }

        private string m_title = null;
        private string m_artist = null;
    }
}
