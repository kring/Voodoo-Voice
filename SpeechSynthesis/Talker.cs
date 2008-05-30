using System;
using System.Collections.Generic;
using System.Text;
using SpeechLib;

namespace FuzzLab.SpeechSynthesis
{
    public class Talker
    {
        public Talker()
        {
            m_voice = new SpVoice();
        }

        public void Say(string phrase)
        {
            m_voice.Speak(phrase, SpeechVoiceSpeakFlags.SVSFDefault);
        }

        private SpVoice m_voice;
    }
}
