namespace NetworkShareGUI
{
    partial class MainForm
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
            this.lstNodes = new System.Windows.Forms.ListBox();
            this.cmuNodeActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mmuSendFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmuNodeActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstNodes
            // 
            this.lstNodes.ContextMenuStrip = this.cmuNodeActions;
            this.lstNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNodes.FormattingEnabled = true;
            this.lstNodes.Location = new System.Drawing.Point(0, 0);
            this.lstNodes.Name = "lstNodes";
            this.lstNodes.Size = new System.Drawing.Size(800, 450);
            this.lstNodes.TabIndex = 0;
            this.lstNodes.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // cmuNodeActions
            // 
            this.cmuNodeActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmuSendFile});
            this.cmuNodeActions.Name = "cmuNodeActions";
            this.cmuNodeActions.Size = new System.Drawing.Size(122, 26);
            this.cmuNodeActions.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mmuSendFile
            // 
            this.mmuSendFile.Name = "mmuSendFile";
            this.mmuSendFile.Size = new System.Drawing.Size(180, 22);
            this.mmuSendFile.Text = "Send File";
            this.mmuSendFile.Click += new System.EventHandler(this.mmuSendFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.cmuNodeActions;
            this.Controls.Add(this.lstNodes);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.cmuNodeActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstNodes;
        private System.Windows.Forms.ContextMenuStrip cmuNodeActions;
        private System.Windows.Forms.ToolStripMenuItem mmuSendFile;
    }
}

