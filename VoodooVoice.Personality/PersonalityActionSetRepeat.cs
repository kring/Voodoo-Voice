using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.Mp3Player;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSetRepeat : PersonalityAction
    {
        [XmlAttribute("mode")]
        public RepeatMode Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            manager.Mp3Player.RepeatMode = Mode;
        }

        public override string ToString()
        {
            switch (Mode)
            {
                case RepeatMode.PlayPlaylistOnce:
                    return "Disable Repeat";
                case RepeatMode.RepeatSong:
                    return "Repeat Song";
                case RepeatMode.RepeatPlaylist:
                    return "Repeat Playlist";
                default:
                    return "Change Repeat Mode";
            }
        }

        private RepeatMode m_mode;
    }
}
