using System;
using System.Collections.Generic;
using System.Text;
using iTunesLib;

namespace FuzzLab.Mp3Player
{
    public class iTunesTrackList : ITrackList
    {
        public iTunesTrackList(IITTrackCollection collection)
        {
            m_collection = collection;
        }

        public int TrackCount
        {
            get { return m_collection.Count; }
        }

        public IEnumerable<ITrack> Tracks
        {
            get
            {
                if (m_collection != null)
                {
                    foreach (IITTrack track in m_collection)
                    {
                        yield return new iTunesTrack(track);
                    }
                }
            }
        }

        public ITrack this[int index]
        {
            get
            {
                if (m_collection != null)
                {
                    return new iTunesTrack(m_collection[index + 1]);
                }
                else
                {
                    return null;
                }
            }
        }

        public int IndexOf(ITrack track)
        {
            iTunesTrack typeTrack = track as iTunesTrack;
            if (m_collection == null ||
                typeTrack == null)
            {
                return -1;
            }

            for (int i = 0; i < m_collection.Count; ++i)
            {
                if (m_collection[i + 1].TrackDatabaseID == typeTrack.InternalTrack.TrackDatabaseID)
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

        private IITTrackCollection m_collection;
    }
}
