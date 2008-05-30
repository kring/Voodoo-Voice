using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public interface ITrack : IEquatable<ITrack>
    {
        /// <summary>
        /// The name of the track, which is generally the title of the song.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The artist of this track.
        /// </summary>
        string Artist { get; }

        /// <summary>
        /// Begins playing this track.
        /// </summary>
        void Play();
    }
}
