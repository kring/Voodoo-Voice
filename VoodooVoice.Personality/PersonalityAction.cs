using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.VoodooVoice
{
    public abstract class PersonalityAction
    {
        public abstract void Run(PersonalityManager manager, Dictionary<string, string> arguments);
    }
}
