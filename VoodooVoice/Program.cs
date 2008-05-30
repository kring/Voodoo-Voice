using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FuzzLab.Utility;

namespace FuzzLab.VoodooVoice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}