using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SpeechLib;
using FuzzLab.Utility;

namespace FuzzLab.VoiceRecognition
{
    public class CommandRecognizer
    {
        public CommandRecognizer()
        {
            m_context = new SpSharedRecoContext();

            // We want to know when a phrase is recognized and also when one fails to be recognized
            m_context.EventInterests = SpeechRecoEvents.SRERecognition | SpeechRecoEvents.SREFalseRecognition;
            m_context.Recognition += new _ISpeechRecoContextEvents_RecognitionEventHandler(RecognitionHandler);
            m_context.FalseRecognition += new _ISpeechRecoContextEvents_FalseRecognitionEventHandler(FailedRecognitionHandler);

            m_grammar = m_context.CreateGrammar(0);
            m_grammar.Reset(0);
        }

        /// <summary>
        /// Creates a mode with the specified name.
        /// </summary>
        /// <param name="name">The name of the mode to create.</param>
        /// <returns>A new mode with the specified name.</returns>
        public CommandRecognizerMode AddMode(string name)
        {
            CommandRecognizerMode mode = new CommandRecognizerMode(name, m_grammar, m_firstRuleId, m_firstRuleId + MaxRulesPerMode - 1);
            m_modes.Add(mode);
            m_firstRuleId += MaxRulesPerMode;
            return mode;
        }

        public void RemoveAllModes()
        {
            m_grammar.Reset(0);
            m_modes.Clear();
        }

        public List<CommandRecognizerMode> Modes
        {
            get { return m_modes; }
        }

        public List<CommandRecognizerMode> ActiveModes
        {
            get
            {
                List<CommandRecognizerMode> result = new List<CommandRecognizerMode>();
                foreach (CommandRecognizerMode mode in Modes)
                {
                    if (mode.IsActive)
                    {
                        result.Add(mode);
                    }
                }
                return result;
            }
            set
            {
                foreach (CommandRecognizerMode mode in Modes)
                {
                    if (value.Contains(mode))
                    {
                        mode.IsActive = true;
                    }
                    else
                    {
                        mode.IsActive = false;
                    }
                }
            }
        }

        /// <summary>
        /// Activates the listed modes, deactivating all others.
        /// </summary>
        /// <param name="names">The names of the modes to be activated.</param>
        public void ActivateModes(params string[] names)
        {
            List<CommandRecognizerMode> activeModes = new List<CommandRecognizerMode>();
            foreach (CommandRecognizerMode mode in Modes)
            {
                if (Array.IndexOf<string>(names, mode.Name) >= 0)
                {
                    activeModes.Add(mode);
                }
            }
            ActiveModes = activeModes;
        }

        public void AddPhraseToList(string listName, string phrase)
        {
            AddPhraseToList(listName, phrase, 0);
        }

        public void AddPhraseToList(string listName, string phrase, int id)
        {
            ISpeechGrammarRule listRule = m_grammar.Rules.FindRule(listName);
            if (listRule == null)
            {
                listRule = m_grammar.Rules.Add(listName, 0, m_firstRuleId);
                ++m_firstRuleId;
            }

            phrase = phrase.Replace('/', ' ');

            object propertyValue = phrase;
            try
            {
                listRule.InitialState.AddWordTransition(null, phrase, " ", SpeechGrammarWordType.SGLexical, listName, id, ref propertyValue, 1);
            }
            catch (COMException)
            {
                // TODO: Ignore duplicate phrase error, re-throw all others
            }
        }

        public void AddNumberToList(string listName, int number)
        {
            ISpeechGrammarRule listRule = m_grammar.Rules.FindRule(listName);
            if (listRule == null)
            {
                listRule = m_grammar.Rules.Add(listName, 0, m_firstRuleId);
                ++m_firstRuleId;
            }

            object propertyValue = number;
            try
            {
                listRule.InitialState.AddWordTransition(null, NumberToWords.Convert(number), " ", SpeechGrammarWordType.SGLexical, listName, 0, ref propertyValue, 1);
            }
            catch (COMException)
            {
                // TODO: Ignore duplicate phrase error, re-throw all others
            }
        }

        public void RemoveAllPhrasesFromList(string listName)
        {
            ISpeechGrammarRule listRule = m_grammar.Rules.FindRule(listName);
            if (listRule != null)
            {
                listRule.Clear();
                object propertyValue = null;
                listRule.InitialState.AddWordTransition(null, "this is a placeholder to avoid an empty list", " ", SpeechGrammarWordType.SGLexical, listName, 0, ref propertyValue, 1);
            }
        }

        /// <summary>
        /// Saves changes to the recognizer and activates them for use
        /// </summary>
        public void Commit()
        {
            m_grammar.State = SpeechGrammarState.SGSEnabled;
            m_grammar.Rules.Commit();
            m_grammar.State = SpeechGrammarState.SGSEnabled;

            m_context.State = SpeechRecoContextState.SRCS_Enabled;
        }

        /// <summary>
        /// Shows a dialog that can be used to configure speech recognition
        /// </summary>
        public void ShowConfigurationDialog(IntPtr parentWindowHandle)
        {
            object extraData = null;
            if (m_context.Recognizer.IsUISupported(SpeechStringConstants.SpeechRecoProfileProperties, ref extraData))
            {
                m_context.Recognizer.DisplayUI(parentWindowHandle.ToInt32(), "Speech Properties", SpeechStringConstants.SpeechRecoProfileProperties, ref extraData);
            }
            else
            {
                // TODO: Notify the user that the engine does not support a properties UI
            }
        }

        private void RecognitionHandler(
            int StreamNumber,
            object StreamPosition,
            SpeechRecognitionType RecognitionType,
            ISpeechRecoResult Result)
        {
            int id = Result.PhraseInfo.Rule.Id;
            foreach (CommandRecognizerMode mode in Modes)
            {
                if (id >= mode.FirstRuleId &&
                    id <= mode.LastRuleId)
                {
                    mode.OnCommandRecognized(Result);
                }
            }
        }

        private void FailedRecognitionHandler(
            int StreamNumber,
            object StreamPosition,
            ISpeechRecoResult Result)
        {
            foreach (CommandRecognizerMode mode in ActiveModes)
            {
                mode.OnCommandNotRecognized(Result);
            }
        }

        private const int MaxRulesPerMode = 1000000;

        private List<CommandRecognizerMode> m_modes = new List<CommandRecognizerMode>();
        private SpSharedRecoContext m_context;
        private ISpeechRecoGrammar m_grammar;
        private int m_firstRuleId = 1;
        private Stack<CommandRecognizerMode> m_modeStack = new Stack<CommandRecognizerMode>();
    }
}
