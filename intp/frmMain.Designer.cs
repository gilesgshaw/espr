namespace intp
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.pbSpect = new System.Windows.Forms.PictureBox();
            this.coIn = new System.Windows.Forms.ComboBox();
            this.coOut = new System.Windows.Forms.ComboBox();
            this.cbThrough = new System.Windows.Forms.CheckBox();
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.ddProc = new System.Windows.Forms.ToolStripDropDownButton();
            this.miIncProc = new System.Windows.Forms.ToolStripMenuItem();
            this.miDecProc = new System.Windows.Forms.ToolStripMenuItem();
            this.ddGraph = new System.Windows.Forms.ToolStripDropDownButton();
            this.miIncGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.miDecGraph = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbSpect)).BeginInit();
            this.ss.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(28, 51);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 27);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(111, 51);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 27);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // pbSpect
            // 
            this.pbSpect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSpect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSpect.Location = new System.Drawing.Point(215, 97);
            this.pbSpect.Name = "pbSpect";
            this.pbSpect.Size = new System.Drawing.Size(594, 342);
            this.pbSpect.TabIndex = 4;
            this.pbSpect.TabStop = false;
            // 
            // coIn
            // 
            this.coIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coIn.FormattingEnabled = true;
            this.coIn.Location = new System.Drawing.Point(28, 20);
            this.coIn.Name = "coIn";
            this.coIn.Size = new System.Drawing.Size(250, 21);
            this.coIn.TabIndex = 8;
            // 
            // coOut
            // 
            this.coOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coOut.FormattingEnabled = true;
            this.coOut.Location = new System.Drawing.Point(314, 20);
            this.coOut.Name = "coOut";
            this.coOut.Size = new System.Drawing.Size(250, 21);
            this.coOut.TabIndex = 9;
            // 
            // cbThrough
            // 
            this.cbThrough.AutoSize = true;
            this.cbThrough.Location = new System.Drawing.Point(314, 57);
            this.cbThrough.Name = "cbThrough";
            this.cbThrough.Size = new System.Drawing.Size(66, 17);
            this.cbThrough.TabIndex = 10;
            this.cbThrough.Text = "Through";
            this.cbThrough.UseVisualStyleBackColor = true;
            // 
            // tlpSettings
            // 
            this.tlpSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tlpSettings.ColumnCount = 2;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSettings.Location = new System.Drawing.Point(13, 97);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 2;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSettings.Size = new System.Drawing.Size(185, 342);
            this.tlpSettings.TabIndex = 11;
            // 
            // ss
            // 
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddProc,
            this.ddGraph});
            this.ss.Location = new System.Drawing.Point(0, 449);
            this.ss.Name = "ss";
            this.ss.Size = new System.Drawing.Size(830, 22);
            this.ss.TabIndex = 12;
            this.ss.Text = "statusStrip1";
            // 
            // ddProc
            // 
            this.ddProc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddProc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIncProc,
            this.miDecProc});
            this.ddProc.Image = ((System.Drawing.Image)(resources.GetObject("ddProc.Image")));
            this.ddProc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddProc.Name = "ddProc";
            this.ddProc.Size = new System.Drawing.Size(69, 20);
            this.ddProc.Text = "PROCESS";
            // 
            // miIncProc
            // 
            this.miIncProc.Name = "miIncProc";
            this.miIncProc.Size = new System.Drawing.Size(156, 22);
            this.miIncProc.Text = "Increase Buffer";
            // 
            // miDecProc
            // 
            this.miDecProc.Name = "miDecProc";
            this.miDecProc.Size = new System.Drawing.Size(156, 22);
            this.miDecProc.Text = "Decrease Buffer";
            // 
            // ddGraph
            // 
            this.ddGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddGraph.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIncGraph,
            this.miDecGraph});
            this.ddGraph.Image = ((System.Drawing.Image)(resources.GetObject("ddGraph.Image")));
            this.ddGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddGraph.Name = "ddGraph";
            this.ddGraph.Size = new System.Drawing.Size(76, 20);
            this.ddGraph.Text = "GRAPHICS";
            // 
            // miIncGraph
            // 
            this.miIncGraph.Name = "miIncGraph";
            this.miIncGraph.Size = new System.Drawing.Size(156, 22);
            this.miIncGraph.Text = "Increase Buffer";
            // 
            // miDecGraph
            // 
            this.miDecGraph.Name = "miDecGraph";
            this.miDecGraph.Size = new System.Drawing.Size(156, 22);
            this.miDecGraph.Text = "Decrease Buffer";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 471);
            this.Controls.Add(this.ss);
            this.Controls.Add(this.tlpSettings);
            this.Controls.Add(this.cbThrough);
            this.Controls.Add(this.coOut);
            this.Controls.Add(this.coIn);
            this.Controls.Add(this.pbSpect);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.pbSpect)).EndInit();
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox pbSpect;
        private System.Windows.Forms.ComboBox coIn;
        private System.Windows.Forms.ComboBox coOut;
        private System.Windows.Forms.CheckBox cbThrough;
        private System.Windows.Forms.TableLayoutPanel tlpSettings;
        private System.Windows.Forms.StatusStrip ss;
        private System.Windows.Forms.ToolStripDropDownButton ddGraph;
        private System.Windows.Forms.ToolStripMenuItem miIncGraph;
        private System.Windows.Forms.ToolStripMenuItem miDecGraph;
        private System.Windows.Forms.ToolStripDropDownButton ddProc;
        private System.Windows.Forms.ToolStripMenuItem miIncProc;
        private System.Windows.Forms.ToolStripMenuItem miDecProc;
    }
}

