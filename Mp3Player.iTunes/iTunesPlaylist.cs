using System;
using System.Collections.Generic;
using System.Text;
using iTunesLib;

namespace FuzzLab.Mp3Player
{
    public class iTunesPlaylist : IPlaylist
    {
        public iTunesPlaylist(IITPlaylist playlist)
        {
            m_playlist = playlist;
        }

        public string Name
        {
            get { return m_playlist.Name; }
        }

        public int TrackCount
        {
            get { return m_playlist.Tracks.Count; }
        }

        public IEnumerable<ITrack> Tracks
        {
            get
            {
                foreach (IITTrack track in m_playlist.Tracks)
                {
                    yield return new iTunesTrack(track);
                }
            }
        }

        public ITrack this[int index]
        {
            get
            {
                return new iTunesTrack(m_playlist.Tracks[index + 1]);
            }
        }

        public int IndexOf(ITrack track)
        {
            iTunesTrack typeTrack = track as iTunesTrack;
            if (typeTrack == null)
            {
                return -1;
            }

            for (int i = 0; i < m_playlist.Tracks.Count; ++i)
            {
                if (m_playlist.Tracks[i + 1].TrackDatabaseID == typeTrack.InternalTrack.TrackDatabaseID)
                {
                    return i;
                }
            }
            return -1;
        }

        public ITrackList FindByTitle(string title)
        {
            IITTrackCollection collection = m_playlist.Search(title, ITPlaylistSearchField.ITPlaylistSearchFieldSongNames);
            return new iTunesTrackList(collection);
        }

        public ITrackList FindByArtist(string artist)
        {
            IITTrackCollection collection = m_playlist.Search(artist, ITPlaylistSearchField.ITPlaylistSearchFieldArtists);
            return new iTunesTrackList(collection);
        }

        internal IITPlaylist InternalPlaylist
        {
            get { return m_playlist; }
        }

        private IITPlaylist m_playlist;
    }
}
