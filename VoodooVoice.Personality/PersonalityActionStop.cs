using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionStop : PersonalityAction
    {
        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            manager.Mp3Player.Stop();
        }

        public override string ToString()
        {
            return "Stop";
        }
    }
}
