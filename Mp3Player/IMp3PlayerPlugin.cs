using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Mp3Player
{
    public interface IMp3PlayerPlugin
    {
        string Name { get; }
        IMp3Player LoadMp3Player();
        string[] Mp3PlayerUnavailableMessage { get; }
    }
}
