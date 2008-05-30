using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.VoiceRecognition;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityMode
    {
        public delegate void ActiveStateChangedHandler();
        public event ActiveStateChangedHandler ActiveStateChanged;

        [XmlAttribute("name")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        [XmlElement("Command")]
        public List<PersonalityCommand> Commands
        {
            get { return m_commands; }
            set
            {
                m_commands = value;
                UpdateManager();
            }
        }

        [XmlElement("Unrecognized")]
        public PersonalityCommand Unrecognized
        {
            get { return m_unrecognized; }
            set
            {
                m_unrecognized = value;
                m_unrecognized.Manager = Manager;
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

        [XmlIgnore]
        public CommandRecognizerMode CommandRecognizerMode
        {
            get { return m_commandRecognizerMode; }
        }

        [XmlIgnore]
        public bool IsActive
        {
            get { return CommandRecognizerMode.IsActive; }
        }

        /// <summary>
        /// Activates this mode.
        /// </summary>
        public void Activate()
        {
            m_oldModes = Manager.Recognizer.ActiveModes;
            List<CommandRecognizerMode> activeModes = new List<CommandRecognizerMode>();
            activeModes.Add(m_commandRecognizerMode);
            Manager.Recognizer.ActiveModes = activeModes;
        }

        internal void AddToCommandRecognizer()
        {
            m_commandRecognizerMode = Manager.Recognizer.AddMode(Name);
            m_commandRecognizerMode.ActiveStateChanged += RaiseActiveStateChanged;
            m_commandRecognizerMode.CommandExecuted += CommandExecuted;
            m_commandRecognizerMode.CommandNotRecognized += CommandNotRecognized;
            foreach (PersonalityCommand command in Commands)
            {
                CommandRecognizerMode.AddCommand(command.Phrase, command.Run);
            }
        }

        protected void RaiseActiveStateChanged()
        {
            ActiveStateChangedHandler temp = ActiveStateChanged;
            if (temp != null)
            {
                temp();
            }
        }

        void CommandExecuted(CommandRecognizerMode.CommandHandler handler)
        {
            RestorePreviousMode();
        }

        private void CommandNotRecognized()
        {
            RestorePreviousMode();
            if (m_unrecognized != null)
            {
                m_unrecognized.Run(new Dictionary<string, string>());
            }
        }

        private void RestorePreviousMode()
        {
            if (m_oldModes != null && m_oldModes.Count > 0)
            {
                Manager.Recognizer.ActiveModes = m_oldModes;
                m_oldModes = null;
            }
        }

        private void UpdateManager()
        {
            foreach (PersonalityCommand command in m_commands)
            {
                command.Manager = Manager;
            }
            if (m_unrecognized != null)
            {
                m_unrecognized.Manager = Manager;
            }
        }

        private string m_name;
        private List<PersonalityCommand> m_commands = new List<PersonalityCommand>();
        private PersonalityManager m_manager = null;
        private CommandRecognizerMode m_commandRecognizerMode = null;
        private List<CommandRecognizerMode> m_oldModes = null;
        private PersonalityCommand m_unrecognized = null;
    }
}
