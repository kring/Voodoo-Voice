using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FuzzLab.Mp3Player;
using FuzzLab.VoiceRecognition;

namespace FuzzLab.VoodooVoice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            EventLog.Instance.NewEvent += new EventLog.NewEventHandler(NewEvent);
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            versionLabel.Text = typeof(Form1).Assembly.GetName().Version.ToString();
            versionLabel.ToolTipText = "This is Voodoo Voice v" + versionLabel.Text + " for iTunes";

            TreeNode loadingNode = currentCommandsTree.Nodes.Add("Loading, please wait...");
            loadingNode.ImageIndex = 1;
            loadingNode.SelectedImageIndex = 1;

            m_thread.ThreadEnding += new EventHandler(m_thread_ThreadEnding);
            m_thread.PersonalityManagerCreated += new VoodooVoice.PersonalityManagerCreatedHandler(PersonalityManagerCreated);
            m_thread.Start();
        }

        void m_thread_ThreadEnding(object sender, EventArgs e)
        {
            // The main thread has ended, so close the form and exit the application.
            m_exitApplication = true;
            Invoke(new MethodInvoker(Close));
        }

        private void PersonalityManagerCreated(PersonalityManager manager)
        {
            if (InvokeRequired)
            {
                // Switch to the UI thread
                Invoke(new VoodooVoice.PersonalityManagerCreatedHandler(PersonalityManagerCreated), manager);
                return;
            }

            m_personalityManager = manager;
            m_personalityManager.Mp3Player.Mp3PlayerClosed += new EventHandler(Mp3PlayerClosed);
            m_personalityManager.NewPersonalityLoaded += new PersonalityManager.NewPersonalityLoadedHandler(NewPersonalityLoaded);
            m_personalityManager.PlaylistLoadProgressChanged += new EventHandler<PlaylistProgressEventArgs>(PlaylistLoadPercentCompleted);
        }

        private void NewPersonalityLoaded(Personality personality)
        {
            if (InvokeRequired)
            {
                Invoke(new PersonalityManager.NewPersonalityLoadedHandler(NewPersonalityLoaded), personality);
                return;
            }

            m_personality = personality;
            m_personality.ActiveModesChanged += new Personality.ActiveModesChangedHandler(ActiveModesChanged);

            foreach (PersonalityMode mode in m_personality.Modes)
            {
                TreeNode modeNode = commandTree.Nodes.Add(mode.Name);
                modeNode.ImageIndex = 0;
                modeNode.SelectedImageIndex = 0;
                foreach (PersonalityCommand command in mode.Commands)
                {
                    TreeNode commandNode = modeNode.Nodes.Add(command.Phrase);
                    commandNode.ImageIndex = 1;
                    commandNode.SelectedImageIndex = 1;
                    AddActions(command, commandNode);
                }
                modeNode.Expand();
            }
        }

        private void AddActions(PersonalityActionGroup group, TreeNode groupNode)
        {
            foreach (PersonalityAction action in group.Actions)
            {
                TreeNode actionNode = groupNode.Nodes.Add(action.ToString());
                actionNode.ImageIndex = 2;
                actionNode.SelectedImageIndex = 2;

                PersonalityActionGroup subGroup = action as PersonalityActionGroup;
                if (subGroup != null)
                {
                    AddActions(subGroup, actionNode);
                }
            }
        }

        private void PlaylistLoadPercentCompleted(object sender, PlaylistProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<PlaylistProgressEventArgs>(PlaylistLoadPercentCompleted), sender, e);
                return;
            }

            if (e.TotalItems == e.CompletedItems)
            {
                progressBar.Visible = false;
                statusLabel.Text = "Listening...";
            }
            else
            {
                progressBar.Visible = true;
                statusLabel.Text = "Building voice playlist..."; 
            }

            if (e.TotalItems == e.CompletedItems)
            {
                progressBar.Value = 100;
            }
            else
            {
                progressBar.Value = e.CompletedItems * 100 / e.TotalItems;
            }
        }

        private void ActiveModesChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new Personality.ActiveModesChangedHandler(ActiveModesChanged));
                return;
            }

            currentCommandsTree.Nodes.Clear();
            foreach (PersonalityMode mode in m_personalityManager.ActivePersonality.ActiveModes)
            {
                TreeNode modeNode = currentCommandsTree.Nodes.Add(mode.Name);
                modeNode.ImageIndex = 0;
                modeNode.SelectedImageIndex = 0;
                foreach (PersonalityCommand command in mode.Commands)
                {
                    TreeNode commandNode = modeNode.Nodes.Add(command.Phrase);
                    commandNode.ImageIndex = 1;
                    commandNode.SelectedImageIndex = 1;
                }
                modeNode.Expand();
            }
        }

        private void Mp3PlayerClosed(object sender, EventArgs e)
        {
            // Stop the main thread - Voodoo Voice will close when the thread ends.
            m_thread.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the main thread, but do not actually close the form until the thread has ended.
            m_thread.Stop();
            e.Cancel = !m_exitApplication;
        }

        private void NewEvent(Event newEvent)
        {
            if (InvokeRequired)
            {
                Invoke(new EventLog.NewEventHandler(NewEvent), newEvent);
                return;
            }
            outputBox.SelectedIndex = outputBox.Items.Add(newEvent.text);
        }

        private void ConfigureButton_Click(object sender, EventArgs e)
        {
            if (m_personalityManager != null &&
                m_personalityManager.Recognizer != null)
            {
                m_personalityManager.Recognizer.ShowConfigurationDialog(Handle);
            }
        }

        private void VoodooVoiceLogo_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.voodoo-voice.com");
            }
            catch
            {
                MessageBox.Show("Unable to open a web browser.  Please manually visit http://www.voodoo-voice.com.");
            }
        }

        private void openPersonalityButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = "psn3";
            fd.Filter = "Voodoo Voice 3 Personality Files (*.psn3)|*.psn3";
            fd.Multiselect = false;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_personalityManager.LoadPersonality(fd.FileName);
                    Properties.Settings.Default.Personality = fd.FileName;
                    Properties.Settings.Default.Save();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error loading Personality file");
                }
            }
        }

        private void outputBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (outputBox.SelectedItem != null)
            {
                string text = outputBox.SelectedItem.ToString();
                int urlStart = text.IndexOf("http://");
                if (urlStart >= 0)
                {
                    int urlEnd = text.IndexOf(' ', urlStart);
                    if (urlEnd < 0)
                    {
                        urlEnd = text.Length;
                    }
                    System.Diagnostics.Process.Start(text.Substring(urlStart, urlEnd - urlStart));
                }
            }
        }

        private VoodooVoice m_thread = new VoodooVoice();
        private PersonalityManager m_personalityManager = null;
        private Personality m_personality = null;
        private volatile bool m_exitApplication = false;
        private object m_lockObj = new object();
    }
}