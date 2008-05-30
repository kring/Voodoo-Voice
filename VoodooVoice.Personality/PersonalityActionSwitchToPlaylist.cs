using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.Mp3Player;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSwitchToPlaylist : PersonalityAction
    {
        [XmlAttribute("playlistName")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            string name = Name;
            if (arguments.ContainsKey("playlistName"))
            {
                name = arguments["playlistName"];
            }
            foreach (IPlaylist playlist in manager.Mp3Player.Playlists)
            {
                if (playlist.Name == name)
                {
                    manager.Mp3Player.ActivePlaylist = playlist;
                    break;
                }
            }
        }

        private string m_name;
    }
}
