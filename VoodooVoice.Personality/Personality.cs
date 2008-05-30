using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.Mp3Player;
using FuzzLab.VoiceRecognition;

namespace FuzzLab.VoodooVoice
{
    public class Personality
    {
        public delegate void ActiveModesChangedHandler();
        public event ActiveModesChangedHandler ActiveModesChanged;

        /// <summary>
        /// The name of the personality.
        /// </summary>
        [XmlElement("Name")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Some text decribing the personality.
        /// </summary>
        [XmlElement("Description")]
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// The file from which the personality was loaded.
        /// </summary>
        public string SourceFile
        {
            get { return m_sourceFile; }
            set { m_sourceFile = value; }
        }

        [XmlElement("Mode")]
        public List<PersonalityMode> Modes
        {
            get { return m_modes; }
            set
            {
                m_modes = value;
                UpdateManager();
            }
        }

        [XmlIgnore]
        public List<PersonalityMode> ActiveModes
        {
            get
            {
                List<PersonalityMode> result = new List<PersonalityMode>();
                foreach (PersonalityMode mode in Modes)
                {
                    if (mode.IsActive)
                    {
                        result.Add(mode);
                    }
                }
                return result;
            }
        }

        [XmlIgnore]
        public PersonalityManager Manager
        {
            get { return m_manager; }
            internal set
            {
                m_manager = value;
                UpdateManager();
            }
        }

        internal void AddToCommandRecognizer()
        {
            foreach (PersonalityMode mode in Modes)
            {
                mode.AddToCommandRecognizer();
                mode.ActiveStateChanged += RaiseActiveModesChanged;
            }
        }

        protected void RaiseActiveModesChanged()
        {
            ActiveModesChangedHandler temp = ActiveModesChanged;
            if (temp != null)
            {
                temp();
            }
        }

        private void UpdateManager()
        {
            foreach (PersonalityMode mode in m_modes)
            {
                mode.Manager = Manager;
            }
        }

        private string m_name = "Default";
        private string m_description = "Default Personality";
        private string m_sourceFile = "N/A";
        private List<PersonalityMode> m_modes = new List<PersonalityMode>();
        private PersonalityManager m_manager = null;
    }
}
