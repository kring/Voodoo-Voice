using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionChooseRandom : PersonalityActionGroup
    {
        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            int actionIndex = m_random.Next(Actions.Count);
            Actions[actionIndex].Run(manager, arguments);
        }

        public override string ToString()
        {
            return "Random Action";
        }

        private static Random m_random = new Random();
    }
}
