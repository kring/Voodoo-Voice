using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionNext : PersonalityAction
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
                manager.Mp3Player.NextByArtist();
            }
            else
            {
                manager.Mp3Player.Next();
            }
        }

        public override string ToString()
        {
            if (m_sameArtist)
            {
                return "Next By Same Artist";
            }
            else
            {
                return "Next";
            }
        }

        private bool m_sameArtist = false;
    }
}
