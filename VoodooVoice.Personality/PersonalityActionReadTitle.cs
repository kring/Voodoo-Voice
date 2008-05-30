using System;
using System.Collections.Generic;
using System.Text;
using FuzzLab.Mp3Player;

namespace FuzzLab.VoodooVoice
{
    public class PersonalityActionReadTitle : PersonalityAction
    {
        public override void Run(PersonalityManager manager, Dictionary<string, string> arguments)
        {
            ITrack track = manager.Mp3Player.CurrentlyPlayingTrack;
            // Say the song name
            string phrase = "";
            if (track != null)
            {
                if (track.Artist != null)
                {
                    phrase += track.Artist;
                }
                if (track.Name != null)
                {
                    if (phrase.Length != 0)
                    {
                        phrase += ", ";
                    }
                    phrase += track.Name;
                }
            }
            else
            {
                phrase = "Nothing is playing.";
            }
            manager.Talker.Say(phrase);
        }

        public override string ToString()
        {
            return "Read Name of Current Track";
        }
    }
}
