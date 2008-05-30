using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public interface ITrackList
    {
        /// <summary>
        /// The number of tracks in this list.
        /// </summary>
        int TrackCount { get; }

        /// <summary>
        /// Enumerates the tracks in this list.
        /// </summary>
        IEnumerable<ITrack> Tracks { get; }

        /// <summary>
        /// Gets the index of a track in the track list.
        /// </summary>
        /// <param name="track">The track to find.</param>
        /// <returns>The index of the track, or -1 if the track does not exist in the list.</returns>
        int IndexOf(ITrack track);

        /// <summary>
        /// Gets the track at the specified index.
        /// </summary>
        ITrack this[int index] { get; }

        /// <summary>
        /// Finds the subset of tracks in this list that match
        /// the specified title.
        /// </summary>
        /// <param name="title">The title to match.</param>
        /// <returns>A list of tracks where each track matches the specified title.</returns>
        ITrackList FindByTitle(string title);

        /// <summary>
        /// Finds the subset of tracks in this list that match
        /// the specified artist.
        /// </summary>
        /// <param name="artist">The artist to match.</param>
        /// <returns>A list of tracks where each track matches the specified artist.</returns>
        ITrackList FindByArtist(string artist);
    }
}
