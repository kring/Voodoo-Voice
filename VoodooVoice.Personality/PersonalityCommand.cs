using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityCommand : PersonalityActionGroup
    {
        [XmlAttribute("phrase")]
        public string Phrase
        {
            get { return m_phrase; }
            set { m_phrase = value; }
        }

        [XmlIgnore]
        public PersonalityManager Manager
        {
            get { return m_manager; }
            internal set { m_manager = value; }
        }

        public void Run(Dictionary<string, string> arguments)
        {
            Run(Manager, arguments);
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            foreach (PersonalityAction action in Actions)
            {
                action.Run(manager, arguments);
            }
        }

        private string m_phrase;
        private PersonalityManager m_manager = null;
    }
}
