using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSetShuffle : PersonalityAction
    {
        [XmlAttribute("enable")]
        public bool Enable
        {
            get { return m_enable; }
            set { m_enable = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            manager.Mp3Player.ShuffleEnabled = Enable;
        }

        public override string ToString()
        {
            return (Enable ? "Enable" : "Disable") + " Shuffle";
        }

        private bool m_enable;
    }
}
