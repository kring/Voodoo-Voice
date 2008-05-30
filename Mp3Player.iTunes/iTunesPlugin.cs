using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public class iTunesPlugin : IMp3PlayerPlugin
    {
        public string Name
        {
            get { return "iTunes"; }
        }

        public IMp3Player LoadMp3Player()
        {
            try
            {
                return new iTunes();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string[] Mp3PlayerUnavailableMessage
        {
            get
            {
                string[] message = new string[2];
                message[0] = "Apple iTunes does not appear to be installed.";
                message[1] = "Download it from http://www.itunes.com (double-click here)";
                return message;
            }
        }
    }
}
