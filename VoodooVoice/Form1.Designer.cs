namespace FuzzLab.VoodooVoice
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.currentCommands = new System.Windows.Forms.TabPage();
            this.currentCommandsTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.commandBrowser = new System.Windows.Forms.TabPage();
            this.commandTree = new System.Windows.Forms.TreeView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.voodooVoiceLogo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openPersonalityButton = new System.Windows.Forms.ToolStripButton();
            this.configureButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.outputBox = new System.Windows.Forms.ListBox();
            this.versionLabel = new System.Windows.Forms.ToolStripLabel();
            this.tabControl.SuspendLayout();
            this.currentCommands.SuspendLayout();
            this.commandBrowser.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.currentCommands);
            this.tabControl.Controls.Add(this.commandBrowser);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 333);
            this.tabControl.TabIndex = 6;
            // 
            // currentCommands
            // 
            this.currentCommands.Controls.Add(this.currentCommandsTree);
            this.currentCommands.Location = new System.Drawing.Point(4, 22);
            this.currentCommands.Name = "currentCommands";
            this.currentCommands.Padding = new System.Windows.Forms.Padding(3);
            this.currentCommands.Size = new System.Drawing.Size(376, 307);
            this.currentCommands.TabIndex = 0;
            this.currentCommands.Text = "Current Commands";
            // 
            // currentCommandsTree
            // 
            this.currentCommandsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentCommandsTree.ImageIndex = 0;
            this.currentCommandsTree.ImageList = this.imageList;
            this.currentCommandsTree.ItemHeight = 20;
            this.currentCommandsTree.Location = new System.Drawing.Point(3, 3);
            this.currentCommandsTree.Name = "currentCommandsTree";
            this.currentCommandsTree.SelectedImageIndex = 0;
            this.currentCommandsTree.Size = new System.Drawing.Size(370, 301);
            this.currentCommandsTree.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder-closed_16.png");
            this.imageList.Images.SetKeyName(1, "Phrase.bmp");
            this.imageList.Images.SetKeyName(2, "arrow-forward_16.png");
            this.imageList.Images.SetKeyName(3, "PhraseDisabled.bmp");
            // 
            // commandBrowser
            // 
            this.commandBrowser.Controls.Add(this.commandTree);
            this.commandBrowser.Location = new System.Drawing.Point(4, 22);
            this.commandBrowser.Name = "commandBrowser";
            this.commandBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.commandBrowser.Size = new System.Drawing.Size(376, 307);
            this.commandBrowser.TabIndex = 1;
            this.commandBrowser.Text = "Command Browser";
            // 
            // commandTree
            // 
            this.commandTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandTree.ImageIndex = 0;
            this.commandTree.ImageList = this.imageList;
            this.commandTree.ItemHeight = 20;
            this.commandTree.Location = new System.Drawing.Point(3, 3);
            this.commandTree.Name = "commandTree";
            this.commandTree.SelectedImageIndex = 0;
            this.commandTree.Size = new System.Drawing.Size(370, 301);
            this.commandTree.TabIndex = 0;
            // 
            // toolStrip
            // 
            this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip.AutoSize = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.voodooVoiceLogo,
            this.toolStripSeparator1,
            this.openPersonalityButton,
            this.configureButton,
            this.versionLabel});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(384, 43);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 9;
            this.toolStrip.Text = "toolStrip1";
            // 
            // voodooVoiceLogo
            // 
            this.voodooVoiceLogo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.voodooVoiceLogo.Image = ((System.Drawing.Image)(resources.GetObject("voodooVoiceLogo.Image")));
            this.voodooVoiceLogo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.voodooVoiceLogo.IsLink = true;
            this.voodooVoiceLogo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.voodooVoiceLogo.Name = "voodooVoiceLogo";
            this.voodooVoiceLogo.Size = new System.Drawing.Size(125, 40);
            this.voodooVoiceLogo.Text = "Voodoo Voice";
            this.voodooVoiceLogo.ToolTipText = "Voodoo Voice v3.0, Copyright (c) 2006 Fuzz Lab Software, All Rights Reserved, www" +
                ".voodoo-voice.com";
            this.voodooVoiceLogo.Click += new System.EventHandler(this.VoodooVoiceLogo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // openPersonalityButton
            // 
            this.openPersonalityButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openPersonalityButton.Image = ((System.Drawing.Image)(resources.GetObject("openPersonalityButton.Image")));
            this.openPersonalityButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openPersonalityButton.Name = "openPersonalityButton";
            this.openPersonalityButton.Size = new System.Drawing.Size(23, 40);
            this.openPersonalityButton.Text = "Open New Personality";
            this.openPersonalityButton.Click += new System.EventHandler(this.openPersonalityButton_Click);
            // 
            // configureButton
            // 
            this.configureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.configureButton.Image = ((System.Drawing.Image)(resources.GetObject("configureButton.Image")));
            this.configureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.configureButton.Name = "configureButton";
            this.configureButton.Size = new System.Drawing.Size(23, 40);
            this.configureButton.Text = "Configure Speech Recognition";
            this.configureButton.Click += new System.EventHandler(this.ConfigureButton_Click);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.outputBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(384, 402);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(384, 467);
            this.toolStripContainer1.TabIndex = 10;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripContainer1.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(384, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(267, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            this.progressBar.Step = 1;
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.FormattingEnabled = true;
            this.outputBox.Location = new System.Drawing.Point(0, 331);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(384, 69);
            this.outputBox.TabIndex = 7;
            this.outputBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.outputBox_MouseDoubleClick);
            // 
            // versionLabel
            // 
            this.versionLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.versionLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(65, 40);
            this.versionLabel.Text = "toolStripLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 467);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Voodoo Voice";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.currentCommands.ResumeLayout(false);
            this.commandBrowser.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage currentCommands;
        private System.Windows.Forms.TabPage commandBrowser;
        private System.Windows.Forms.TreeView commandTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView currentCommandsTree;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton openPersonalityButton;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripLabel voodooVoiceLogo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListBox outputBox;
        private System.Windows.Forms.ToolStripButton configureButton;
        private System.Windows.Forms.ToolStripLabel versionLabel;
    }
}

