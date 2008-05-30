using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FuzzLab.Mp3Player;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionSetVolume : PersonalityAction
    {
        public enum VolumeMode
        {
            Set,
            ReduceByAbsolute,
            IncreaseByAbsolute,
            AdjustByPercent,
            RestoreLast
        }

        [XmlAttribute("percent")]
        public string Percent
        {
            get { return m_percent; }
            set { m_percent = value; }
        }

        [XmlAttribute("mode")]
        public VolumeMode Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            string percent = m_percent;
            if (arguments.ContainsKey("percent"))
            {
                percent = arguments["percent"];
            }

            if (Mode == VolumeMode.RestoreLast)
            {
                manager.Mp3Player.Volume = s_lastVolume;
            }
            else
            {
                int percentNum;
                if (Int32.TryParse(percent, out percentNum))
                {
                    switch (Mode)
                    {
                        case VolumeMode.IncreaseByAbsolute:
                            percentNum += manager.Mp3Player.Volume;
                            break;
                        case VolumeMode.ReduceByAbsolute:
                            percentNum = manager.Mp3Player.Volume - percentNum;
                            break;
                        case VolumeMode.AdjustByPercent:
                            percentNum = manager.Mp3Player.Volume * percentNum / 100;
                            break;
                    }

                    if (percentNum < 0)
                    {
                        percentNum = 0;
                    }
                    else if (percentNum > 100)
                    {
                        percentNum = 100;
                    }

                    s_lastVolume = manager.Mp3Player.Volume;
                    manager.Mp3Player.Volume = percentNum;
                }
            }
        }

        public override string ToString()
        {
            return "Set Volume";
        }

        private string m_percent;
        private VolumeMode m_mode = VolumeMode.Set;
        private static int s_lastVolume;
    }
}
