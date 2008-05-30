using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionPlay : PersonalityAction
    {
        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            manager.Mp3Player.Play();
        }

        public override string ToString()
        {
            return "Play";
        }
    }
}
