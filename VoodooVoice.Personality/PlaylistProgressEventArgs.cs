using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.VoodooVoice
{
    public class PlaylistProgressEventArgs : EventArgs
    {
        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }

        public int CompletedItems
        {
            get { return m_completedItems; }
            set { m_completedItems = value; }
        }

        public int TotalItems
        {
            get { return m_totalItems; }
            set { m_totalItems = value; }
        }

        private bool m_cancel;
        private int m_completedItems;
        private int m_totalItems;
    }
}
