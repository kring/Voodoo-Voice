using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionActivateMode : PersonalityAction
    {
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            foreach (PersonalityMode mode in manager.ActivePersonality.Modes)
            {
                if (mode.Name == Name)
                {
                    mode.Activate();
                }
            }
        }

        public override string ToString()
        {
            return "Activate Mode " + Name;
        }

        private string m_name;
    }
}
