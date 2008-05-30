using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public interface IPlaylist : ITrackList
    {
        /// <summary>
        /// The name of this playlist.
        /// </summary>
        string Name { get; }
    }
}
