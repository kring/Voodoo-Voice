using System;
using System.Collections.Generic;
using System.Text;
using SpeechLib;

namespace FuzzLab.VoiceRecognition
{
    public class CommandRecognizerMode
    {
        public delegate void ActiveStateChangedHandler();
        public event ActiveStateChangedHandler ActiveStateChanged;

        public delegate void CommandRecognitionHandler(CommandHandler handler);
        public event CommandRecognitionHandler CommandExecuting;
        public event CommandRecognitionHandler CommandExecuted;

        public delegate void CommandNotRecognizedHandler();
        public event CommandNotRecognizedHandler CommandNotRecognized;

        public CommandRecognizerMode(string name, ISpeechRecoGrammar grammar, int firstRuleId, int lastRuleId)
        {
            m_name = name;
            m_grammar = grammar;
            m_firstRuleId = firstRuleId;
            m_nextRuleId = firstRuleId;
            m_lastRuleId = lastRuleId;
        }

        public string Name
        {
            get { return m_name; }
        }

        public int FirstRuleId
        {
            get { return m_firstRuleId; }
        }

        public int LastRuleId
        {
            get { return m_lastRuleId; }
        }

        public bool IsActive
        {
            get { return m_isActive; }
            set
            {
                // Set activation of all rules in this mode
                SpeechRuleState newState = value ? SpeechRuleState.SGDSActive : SpeechRuleState.SGDSInactive;
                foreach (int id in m_handlers.Keys)
                {
                    m_grammar.CmdSetRuleIdState(id, newState);
                }

                if (m_isActive != value)
                {
                    m_isActive = value;
                    RaiseActiveStateChanged();
                }
            }
        }

        public delegate void CommandHandler(Dictionary<string, string> arguments);

        public void AddCommand(string commandPhrase, CommandRecognizerMode.CommandHandler handler)
        {
            if (m_nextRuleId > m_lastRuleId)
            {
                throw new Exception("Maximum number of rules exceeded for Command Recognizer Mode '" + Name + "'.");
            }

            List<CommandPart> parts = GetParts(commandPhrase);

            int ruleId = m_nextRuleId;
            ++m_nextRuleId;

            m_handlers[ruleId] = handler;

            // Create a new rule in the grammar
            ISpeechGrammarRule rule = m_grammar.Rules.Add("",
                SpeechRuleAttributes.SRATopLevel,
                ruleId);

            // Add a state for each part of the phrase and connect them together with
            // appropriate transitions
            ISpeechGrammarRuleState from = rule.InitialState;
            for (int i = 0; i < parts.Count; ++i)
            {
                CommandPart part = parts[i];

                ISpeechGrammarRuleState state = null;
                if (i != parts.Count - 1)
                {
                    state = rule.AddState();
                }

                object propertyValue = 0;
                if (part.isWordFromList)
                {
                    // Add a transition through a list rule
                    ISpeechGrammarRule listRule = m_grammar.Rules.FindRule(part.phrase);
                    if (listRule == null)
                    {
                        listRule = m_grammar.Rules.Add(part.phrase, 0, 0);
                        propertyValue = "placeholder";
                        listRule.InitialState.AddWordTransition(null, "placeholder", " ", SpeechGrammarWordType.SGLexical, part.phrase, 0, ref propertyValue, 1);
                        propertyValue = 0;
                    }
                    from.AddRuleTransition(state, listRule, "", 0, ref propertyValue, 1);
                }
                else
                {
                    // Add a simple rule transition
                    from.AddWordTransition(state, part.phrase, " ", SpeechGrammarWordType.SGLexical, "", 0, ref propertyValue, 1);
                }

                from = state;
            }
        }

        internal void OnCommandRecognized(ISpeechRecoResult result)
        {
            int id = result.PhraseInfo.Rule.Id;
            if (m_handlers.ContainsKey(id))
            {
                CommandHandler handler = m_handlers[id];
                if (handler != null)
                {
                    // Pull any parameters from properties
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    foreach (ISpeechPhraseProperty property in result.PhraseInfo.Properties)
                    {
                        if (property.Children != null)
                        {
                            foreach (ISpeechPhraseProperty childProperty in property.Children)
                            {
                                if (childProperty.Name.Length > 0 &&
                                    childProperty.Value != null)
                                {
                                    parameters[childProperty.Name] = childProperty.Value.ToString();
                                }
                            }
                        }
                    }

                    RaiseCommandExecuting(handler);
                    handler(parameters);
                    RaiseCommandExecuted(handler);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Command Recognizer Mode does not contain the recognized command.");
            }
        }

        internal void OnCommandNotRecognized(ISpeechRecoResult result)
        {
            RaiseCommandNotRecognized();
        }

        protected void RaiseActiveStateChanged()
        {
            ActiveStateChangedHandler temp = ActiveStateChanged;
            if (temp != null)
            {
                temp();
            }
        }

        protected void RaiseCommandExecuting(CommandHandler handler)
        {
            CommandRecognitionHandler temp = CommandExecuting;
            if (temp != null)
            {
                temp(handler);
            }
        }

        protected void RaiseCommandExecuted(CommandHandler handler)
        {
            CommandRecognitionHandler temp = CommandExecuted;
            if (temp != null)
            {
                temp(handler);
            }
        }

        protected void RaiseCommandNotRecognized()
        {
            CommandNotRecognizedHandler temp = CommandNotRecognized;
            if (temp != null)
            {
                temp();
            }
        }

        private struct CommandPart
        {
            public bool isWordFromList;
            public string phrase;
        }

        private List<CommandPart> GetParts(string phrase)
        {
            List<CommandPart> parts = new List<CommandPart>();

            int nextListPart = phrase.IndexOf('[');
            while (nextListPart >= 0)
            {
                if (nextListPart > 0)
                {
                    // Add the phrase up to the beginning of the list part
                    CommandPart phrasePart;
                    phrasePart.isWordFromList = false;
                    phrasePart.phrase = phrase.Substring(0, nextListPart).Trim();
                    parts.Add(phrasePart);

                    // Trim off the phrase part, leaving the list part starting with '['
                    phrase = phrase.Substring(nextListPart);
                }

                if (phrase.Length == 1)
                {
                    // Phrase ends with a '[', which makes no sense, so we're done
                    phrase = "";
                }
                else
                {
                    int endOfListPart = phrase.IndexOf(']');
                    if (endOfListPart < 0)
                    {
                        endOfListPart = phrase.Length;
                    }

                    CommandPart listPart;
                    listPart.isWordFromList = true;
                    listPart.phrase = phrase.Substring(1, endOfListPart - 1);
                    parts.Add(listPart);

                    if (endOfListPart >= phrase.Length - 1)
                    {
                        phrase = "";
                        nextListPart = -1;
                    }
                    else
                    {
                        phrase = phrase.Substring(endOfListPart + 1);
                        nextListPart = phrase.IndexOf('[');
                    }
                }
            }

            if (phrase.Length > 0)
            {
                CommandPart part;
                part.isWordFromList = false;
                part.phrase = phrase;
                parts.Add(part);
            }

            return parts;
        }

        private string m_name;
        private bool m_isActive;
        private ISpeechRecoGrammar m_grammar;
        private int m_firstRuleId;
        private int m_nextRuleId;
        private int m_lastRuleId;
        private Dictionary<int, CommandHandler> m_handlers = new Dictionary<int, CommandHandler>();
    }
}
