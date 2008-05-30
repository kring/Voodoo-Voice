using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FuzzLab.VoodooVoice
{
    public abstract class PersonalityActionGroup : PersonalityAction
    {
        [XmlElement("Play", typeof(PersonalityActionPlay))]
        [XmlElement("Next", typeof(PersonalityActionNext))]
        [XmlElement("Previous", typeof(PersonalityActionPrevious))]
        [XmlElement("Stop", typeof(PersonalityActionStop))]
        [XmlElement("PlaySound", typeof(PersonalityActionPlaySound))]
        [XmlElement("ActivateMode", typeof(PersonalityActionActivateMode))]
        [XmlElement("SwitchTo", typeof(PersonalityActionSwitchTo))]
        [XmlElement("ReadTitle", typeof(PersonalityActionReadTitle))]
        [XmlElement("SetVolume", typeof(PersonalityActionSetVolume))]
        [XmlElement("SetShuffle", typeof(PersonalityActionSetShuffle))]
        [XmlElement("SetRepeat", typeof(PersonalityActionSetRepeat))]
        [XmlElement("Say", typeof(PersonalityActionSay))]
        [XmlElement("ChooseRandom", typeof(PersonalityActionChooseRandom))]
        [XmlElement("SwitchToPlaylist", typeof(PersonalityActionSwitchToPlaylist))]
        public List<PersonalityAction> Actions
        {
            get { return m_actions; }
            set { m_actions = value; }
        }

        private List<PersonalityAction> m_actions = new List<PersonalityAction>();
    }
}
