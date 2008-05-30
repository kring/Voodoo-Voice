using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSay : PersonalityAction
    {
        [XmlAttribute("phrase")]
        public string Phrase
        {
            get { return m_phrase; }
            set { m_phrase = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            manager.Talker.Say(Phrase);
        }

        public override string ToString()
        {
            return "Say '" + Phrase + "'";
        }

        private string m_phrase;
    }
}
