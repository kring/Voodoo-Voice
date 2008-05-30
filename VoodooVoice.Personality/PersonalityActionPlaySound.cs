using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Media;
using System.IO;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionPlaySound : PersonalityAction
    {
        [XmlAttribute("file")]
        public string File
        {
            get { return m_soundFile; }
            set { m_soundFile = value; m_player = null; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            if (m_player == null)
            {
                m_player = new SoundPlayer();
                string directory = Path.GetDirectoryName(manager.ActivePersonality.SourceFile);
                m_player.SoundLocation = Path.Combine(directory, m_soundFile);
                m_player.Load();
            }
            m_player.Play();
        }

        public override string ToString()
        {
            return "Play Sound " + File;
        }

        private string m_soundFile;
        private SoundPlayer m_player;
    }
}
