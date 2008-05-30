using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;
using FuzzLab.Mp3Player;
using FuzzLab.Utility;
using iTunesLib;

namespace FuzzLab.Mp3Player
{
    /// <summary>
    /// Implements IMp3Player for Apple iTunes.
    /// </summary>
    public class iTunes : IMp3Player
    {
        /// <summary>
        /// Signals that iTunes has closed.  This is NOT guaranteed to be called on the same
        /// thread that created the iTunes object.
        /// </summary>
        public event EventHandler Mp3PlayerClosed;

        /// <summary>
        /// Signals that the active playlist has changed.  This is NOT guaranteed to be called
        /// on the same thread that created the iTunes object.
        /// </summary>
        public event EventHandler ActivePlaylistChanged;

        /// <summary>
        /// Signals that the list of playlists changed in some way.
        /// </summary>
        public event EventHandler ListOfPlaylistsChanged;

        /// <summary>
        /// Signals that the contents of the currently active playlist changed in some way.
        /// </summary>
        public event EventHandler ActivePlaylistContentsChanged;

        public iTunes()
        {
            // Construct an iTunes object and register to be notified when the user
            // tries to quit iTunes.
            m_iTunes = new iTunesLib.iTunesAppClass();
            _IiTunesEvents_Event events = (_IiTunesEvents_Event)m_iTunes;
            events.OnAboutToPromptUserToQuitEvent += new _IiTunesEvents_OnAboutToPromptUserToQuitEventEventHandler(this.OnAboutToQuit);
            events.OnDatabaseChangedEvent += new _IiTunesEvents_OnDatabaseChangedEventEventHandler(OnDatabaseChanged);

            // Create a timer to poll for a change in the active playlist
            m_lastActivePlaylist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
            m_activePlaylistChangeTimer = new Timer(PollForActivePlaylistChange, null, 1000, 1000);
        }

        ~iTunes()
        {
            Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            if (m_iTunes != null)
            {
                // Shut down iTunes (if it's still running)
                m_iTunes.Quit();
                m_iTunes = null;
                GC.SuppressFinalize(this);
            }
        }

        public void Play()
        {
            TryUntilNotBusy(delegate()
            {
                m_iTunes.Play();
            });
        }

        private delegate void Operation();

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void TryUntilNotBusy(Operation operation)
        {
            bool succeeded = false;
            bool notified = false;
            while (!succeeded)
            {
                try
                {
                    if (m_iTunes != null)
                    {
                        operation();
                    }
                    succeeded = true;
                    if (notified)
                    {
                        EventLog.Instance.AddEvent(EventType.Information, "iTunes", "iTunes is no longer busy.");
                    }
                }
                catch (COMException e)
                {
                    // 0x8001010a indicates that iTunes is busy.  If we get that, add a message
                    // saying that the MP3 player is busy, and try again.
                    // If we get any other COMException, give up.
                    if (e.ErrorCode == -2147417846)
                    {
                        if (!notified)
                        {
                            EventLog.Instance.AddEvent(EventType.Information, "iTunes", "iTunes is currently busy; waiting for it to become available.");
                            notified = true;
                        }
                    }
                    else
                    {
                        throw;
                    }
                } 
                Thread.Sleep(250);
            }
        }

        public void Pause()
        {
            TryUntilNotBusy(delegate()
            {
                m_iTunes.Pause();
            });
        }

        public void Stop()
        {
            TryUntilNotBusy(delegate()
            {
                m_iTunes.Stop();
            });
        }

        public void Next()
        {
            TryUntilNotBusy(delegate()
            {
                m_iTunes.NextTrack();
            });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void NextByArtist()
        {
            ITrack currentTrack = CurrentlyPlayingTrack;
            IPlaylist activePlaylist = ActivePlaylist;
            if (currentTrack != null && activePlaylist != null)
            {
                ITrackList tracksByArtist = activePlaylist.FindByArtist(currentTrack.Artist);
                if (tracksByArtist != null && tracksByArtist.TrackCount > 0)
                {
                    int nextTrackIndex = tracksByArtist.IndexOf(currentTrack);
                    if (nextTrackIndex < 0)
                    {
                        nextTrackIndex = 0;
                    }
                    else if (nextTrackIndex >= tracksByArtist.TrackCount - 1)
                    {
                        nextTrackIndex = 0;
                    }
                    else
                    {
                        ++nextTrackIndex;
                    }

                    tracksByArtist[nextTrackIndex].Play();
                }
            }
        }

        public void Previous()
        {
            TryUntilNotBusy(delegate()
            {
                m_iTunes.PreviousTrack();
            });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PreviousByArtist()
        {
            ITrack currentTrack = CurrentlyPlayingTrack;
            IPlaylist activePlaylist = ActivePlaylist;
            if (currentTrack != null && activePlaylist != null)
            {
                ITrackList tracksByArtist = activePlaylist.FindByArtist(currentTrack.Artist);
                if (tracksByArtist != null && tracksByArtist.TrackCount > 0)
                {
                    int prevTrackIndex = tracksByArtist.IndexOf(currentTrack);
                    if (prevTrackIndex < 1)
                    {
                        prevTrackIndex = tracksByArtist.TrackCount - 1;
                    }
                    else
                    {
                        --prevTrackIndex;
                    }

                    tracksByArtist[prevTrackIndex].Play();
                }
            }
        }

        public IPlaylist ActivePlaylist
        {
            get
            {
                IPlaylist result = null;
                TryUntilNotBusy(delegate()
                {
                    IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (playlist != null)
                    {
                        result = new iTunesPlaylist(playlist);
                    }
                    else
                    {
                        result = null;
                    }
                });
                return result;
            }
            set
            {
                TryUntilNotBusy(delegate()
                {
                    iTunesPlaylist playlist = value as iTunesPlaylist;
                    if (playlist != null)
                    {
                        object playlistObject = playlist.InternalPlaylist;
                        m_iTunes.BrowserWindow.set_SelectedPlaylist(ref playlistObject);
                    }
                });
            }
        }

        public IEnumerable<IPlaylist> Playlists
        {
            get
            {
                IiTunes iTunes = m_iTunes;
                if (iTunes != null)
                {
                    foreach (IITSource source in m_iTunes.Sources)
                    {
                        foreach (IITPlaylist playlist in source.Playlists)
                        {
                            yield return new iTunesPlaylist(playlist);
                        }
                    }
                }
            }
        }

        public ITrack CurrentlyPlayingTrack
        {
            get
            {
                iTunesTrack result = null;
                TryUntilNotBusy(delegate()
                {
                    IITTrack track = m_iTunes.CurrentTrack;
                    if (track != null)
                    {
                        result = new iTunesTrack(track);
                    }
                    else
                    {
                        result = null;
                    }
                });
                return result;
            }
        }

        public int Volume
        {
            get
            {
                int volume = 0;
                TryUntilNotBusy(delegate()
                {
                    volume = m_iTunes.SoundVolume;
                });
                return volume;
            }
            set
            {
                TryUntilNotBusy(delegate()
                {
                    m_iTunes.SoundVolume = value;
                });
            }
        }

        public bool ShuffleEnabled
        {
            get
            {
                bool shuffle = false;
                TryUntilNotBusy(delegate()
                {
                    IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (playlist != null)
                    {
                        shuffle = playlist.Shuffle;
                    }
                    else
                    {
                        shuffle = false;
                    }
                });
                return shuffle;
            }
            set
            {
                TryUntilNotBusy(delegate()
                {
                    IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (playlist != null)
                    {
                        playlist.Shuffle = value;
                    }
                });
            }
        }

        public RepeatMode RepeatMode
        {
            get
            {
                RepeatMode result = RepeatMode.PlayPlaylistOnce;
                TryUntilNotBusy(delegate()
                {
                    IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (playlist != null)
                    {
                        switch (playlist.SongRepeat)
                        {
                            case ITPlaylistRepeatMode.ITPlaylistRepeatModeOff:
                                result = RepeatMode.PlayPlaylistOnce;
                                break;
                            case ITPlaylistRepeatMode.ITPlaylistRepeatModeOne:
                                result = RepeatMode.RepeatSong;
                                break;
                            case ITPlaylistRepeatMode.ITPlaylistRepeatModeAll:
                                result = RepeatMode.RepeatPlaylist;
                                break;
                            default:
                                result = RepeatMode.PlayPlaylistOnce;
                                break;
                        }
                    }
                    else
                    {
                        result = RepeatMode.PlayPlaylistOnce;
                    }
                });
                return result;
            }
            set
            {
                TryUntilNotBusy(delegate()
                {
                    IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (playlist != null)
                    {
                        switch (value)
                        {
                            case RepeatMode.PlayPlaylistOnce:
                                playlist.SongRepeat = ITPlaylistRepeatMode.ITPlaylistRepeatModeOff;
                                break;
                            case RepeatMode.RepeatSong:
                                playlist.SongRepeat = ITPlaylistRepeatMode.ITPlaylistRepeatModeOne;
                                break;
                            case RepeatMode.RepeatPlaylist:
                                playlist.SongRepeat = ITPlaylistRepeatMode.ITPlaylistRepeatModeAll;
                                break;
                        }
                    }
                });
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void RaiseMp3PlayerClosed()
        {
            EventHandler temp = Mp3PlayerClosed;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void RaiseActivePlaylistChanged()
        {
            EventHandler temp = ActivePlaylistChanged;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void OnAboutToQuit()
        {
            // Tell everyone it's over
            RaiseMp3PlayerClosed();

            m_iTunes = null;
        }

        /// <summary>
        /// This method is invoked by a timer in order to detect and handle selection
        /// of a new playlist.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PollForActivePlaylistChange(object state)
        {
            try
            {
                if (m_iTunes != null && m_iTunes.BrowserWindow != null)
                {
                    IITPlaylist currentActivePlaylist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                    if (m_lastActivePlaylist == null ||
                        (currentActivePlaylist != null &&
                        m_lastActivePlaylist.playlistID != currentActivePlaylist.playlistID))
                    {
                        m_lastActivePlaylist = currentActivePlaylist;
                        RaiseActivePlaylistChanged();
                    }
                }
            }
            catch (COMException)
            {
                // This typically means that iTunes COM calls are deferred because
                // a modal dialog box is up.  Just silently ignore it.
            }
        }

        private void OnDatabaseChanged(object deletedObjectIDs, object changedObjectIDs)
        {
            bool contentsChanged = false;
            bool listsChanged = false;

            object[,] deletedList = (object[,])deletedObjectIDs;
            for (int i = 0; i < deletedList.GetLength(0); ++i)
            {
                int trackID = (int)deletedList[i, 2];
                int playlistID = (int)deletedList[i, 1];
                if (trackID != 0)
                {
                    if (IsActivePlaylist(playlistID))
                    {
                        contentsChanged = true;
                    }
                }
                else if (playlistID != 0)
                {
                    listsChanged = true;
                }
            }

            object[,] changedList = (object[,])changedObjectIDs;
            for (int i = 0; i < changedList.GetLength(0); ++i)
            {
                int trackID = (int)changedList[i, 2];
                int playlistID = (int)changedList[i, 1];
                if (trackID != 0)
                {
                    if (IsActivePlaylist(playlistID))
                    {
                        contentsChanged = true;
                    }
                }
                else if (playlistID != 0)
                {
                    listsChanged = true;
                }
            }

            if (listsChanged)
            {
                EventHandler temp = ListOfPlaylistsChanged;
                if (temp != null)
                {
                    temp.BeginInvoke(this, new EventArgs(), delegate(IAsyncResult result)
                    {
                        temp.EndInvoke(result);
                    }, null);
                }
            }

            if (contentsChanged)
            {
                EventHandler temp = ActivePlaylistContentsChanged;
                if (temp != null)
                {
                    temp.BeginInvoke(this, new EventArgs(), delegate(IAsyncResult result)
                    {
                        temp.EndInvoke(result);
                    }, null);
                }
            }
        }

        private bool IsActivePlaylist(int playlistID)
        {
            bool result = false;
            TryUntilNotBusy(delegate()
            {
                IITPlaylist playlist = m_iTunes.BrowserWindow.get_SelectedPlaylist();
                result = playlist != null && playlist.playlistID == playlistID;
            });
            return result;
        }

        private IiTunes m_iTunes;
        private Timer m_activePlaylistChangeTimer;
        private IITPlaylist m_lastActivePlaylist = null;
    }
}
