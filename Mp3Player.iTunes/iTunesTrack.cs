using System;
using System.Collections.Generic;
using System.Text;
using iTunesLib;

namespace FuzzLab.Mp3Player
{
    public class iTunesTrack : ITrack
    {
        public iTunesTrack(IITTrack track)
        {
            m_track = track;
        }

        public string Name
        {
            get { return m_track.Name; }
        }

        public string Artist
        {
            get { return m_track.Artist; }
        }

        public void Play()
        {
            m_track.Play();
        }

        public bool Equals(ITrack track)
        {
            iTunesTrack typeTrack = track as iTunesTrack;
            if (typeTrack != null &&
                typeTrack.m_track.TrackDatabaseID == m_track.TrackDatabaseID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IITTrack InternalTrack
        {
            get { return m_track; }
        }

        private IITTrack m_track;
    }
}
