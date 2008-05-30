using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public class TrackList : ITrackList
    {
        public TrackList()
        {
            m_tracks = new List<ITrack>();
        }

        public TrackList(List<ITrack> tracks)
        {
            m_tracks = tracks;
        }

        public int TrackCount
        {
            get { return m_tracks.Count; }
        }

        public IEnumerable<ITrack> Tracks
        {
            get { return m_tracks; }
        }

        public ITrack this[int index]
        {
            get
            {
                return m_tracks[index];
            }
        }

        public int IndexOf(ITrack track)
        {
            for (int i = 0; i < m_tracks.Count; ++i)
            {
                if (track.Equals(m_tracks[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public ITrackList FindByTitle(string title)
        {
            List<ITrack> result = new List<ITrack>();
            foreach (ITrack track in Tracks)
            {
                if (track.Name == title)
                {
                    result.Add(track);
                }
            }
            return new TrackList(result);
        }

        public ITrackList FindByArtist(string artist)
        {
            List<ITrack> result = new List<ITrack>();
            foreach (ITrack track in Tracks)
            {
                if (track.Artist == artist)
                {
                    result.Add(track);
                }
            }
            return new TrackList(result);
        }

        private List<ITrack> m_tracks;
    }
}
