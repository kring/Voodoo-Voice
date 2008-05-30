using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public class Winamp : IMp3Player
    {
        public event EventHandler Mp3PlayerClosed;

        public event EventHandler ActivePlaylistChanged;

        public event EventHandler ListOfPlaylistsChanged;

        public event EventHandler ActivePlaylistContentsChanged;

        public Winamp()
        {
        }

        public void Play()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Pause()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Stop()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Next()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void NextByArtist()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Previous()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void PreviousByArtist()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IPlaylist ActivePlaylist
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public IEnumerable<IPlaylist> Playlists
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ITrack CurrentlyPlayingTrack
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int Volume
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool ShuffleEnabled
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public RepeatMode RepeatMode
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
