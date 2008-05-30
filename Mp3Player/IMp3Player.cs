using System;
using System.Collections.Generic;
using System.Text;
using FuzzLab.Utility;

namespace FuzzLab.Mp3Player
{
    /// <summary>
    /// An interface to query and control an MP3 player
    /// </summary>
    public interface IMp3Player : IDisposable
    {
        /// <summary>
        /// Signals that the MP3 player has closed.  This is NOT guaranteed to be called on the same
        /// thread that created the MP3 player object.
        /// </summary>
        event EventHandler Mp3PlayerClosed;

        /// <summary>
        /// Signals that the active playlist has changed.  This is NOT guaranteed to be called
        /// on the same thread that created the MP3 player object.
        /// </summary>
        event EventHandler ActivePlaylistChanged;

        /// <summary>
        /// Signals that the list of playlists changed in some way.
        /// </summary>
        event EventHandler ListOfPlaylistsChanged;

        /// <summary>
        /// Signals that the contents of the currently active playlist changed in some way.
        /// </summary>
        event EventHandler ActivePlaylistContentsChanged;

        void Play();
        void Pause();
        void Stop();
        void Next();
        void NextByArtist();
        void Previous();
        void PreviousByArtist();

        IPlaylist ActivePlaylist { get; set; }
        IEnumerable<IPlaylist> Playlists { get; }
        ITrack CurrentlyPlayingTrack { get; }

        /// <summary>
        /// The sound volume, in the range 0-100.
        /// </summary>
        int Volume { get; set; }

        bool ShuffleEnabled { get; set; }
        RepeatMode RepeatMode { get; set; }
    }
}
