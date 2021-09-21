namespace Chorale
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.ctr = new System.Windows.Forms.Panel();
            this.lbContexts = new System.Windows.Forms.CheckedListBox();
            this.tbMelody = new System.Windows.Forms.TextBox();
            this.btnHarmonise = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ctr
            // 
            this.ctr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctr.Location = new System.Drawing.Point(12, 230);
            this.ctr.Name = "ctr";
            this.ctr.Size = new System.Drawing.Size(809, 189);
            this.ctr.TabIndex = 4;
            // 
            // lbContexts
            // 
            this.lbContexts.FormattingEnabled = true;
            this.lbContexts.Location = new System.Drawing.Point(12, 12);
            this.lbContexts.MultiColumn = true;
            this.lbContexts.Name = "lbContexts";
            this.lbContexts.Size = new System.Drawing.Size(246, 199);
            this.lbContexts.TabIndex = 1;
            // 
            // tbMelody
            // 
            this.tbMelody.AcceptsReturn = true;
            this.tbMelody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMelody.Location = new System.Drawing.Point(274, 12);
            this.tbMelody.Multiline = true;
            this.tbMelody.Name = "tbMelody";
            this.tbMelody.Size = new System.Drawing.Size(547, 138);
            this.tbMelody.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(286, 172);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 39);
            label1.TabIndex = 5;
            label1.Text = "   <\r\nHighlight home key\r\nCheck other keys";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(403, 172);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(41, 26);
            label2.TabIndex = 6;
            label2.Text = "    ∧ \r\nMelody";
            // 
            // btnHarmonise
            // 
            this.btnHarmonise.Location = new System.Drawing.Point(470, 185);
            this.btnHarmonise.Name = "btnHarmonise";
            this.btnHarmonise.Size = new System.Drawing.Size(74, 26);
            this.btnHarmonise.TabIndex = 3;
            this.btnHarmonise.Text = "Harmonise";
            this.btnHarmonise.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 431);
            this.Controls.Add(this.btnHarmonise);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.tbMelody);
            this.Controls.Add(this.lbContexts);
            this.Controls.Add(this.ctr);
            this.Name = "Form1";
            this.Text = "Chorale";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel ctr;
        private System.Windows.Forms.CheckedListBox lbContexts;
        private System.Windows.Forms.TextBox tbMelody;
        private System.Windows.Forms.Button btnHarmonise;
    }
}

