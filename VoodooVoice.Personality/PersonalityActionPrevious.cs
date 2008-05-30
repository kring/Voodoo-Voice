using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionPrevious : PersonalityAction
    {
        [XmlAttribute("sameArtist")]
        public bool SameArtist
        {
            get { return m_sameArtist; }
            set { m_sameArtist = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            if (m_sameArtist)
            {
                manager.Mp3Player.PreviousByArtist();
            }
            else
            {
                manager.Mp3Player.Previous();
            }
        }

        public override string ToString()
        {
            if (m_sameArtist)
            {
                return "Previous By Same Artist";
            }
            else
            {
                return "Previous";
            }
        }

        private bool m_sameArtist = false;
    }
}
