using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Chorale
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            FlowLayoutPanel1 = new FlowLayoutPanel();
            MenuStrip1 = new MenuStrip();
            FileToolStripMenuItem = new ToolStripMenuItem();
            _CreateToolStripMenuItem = new ToolStripMenuItem();
            _CreateToolStripMenuItem.Click += new EventHandler(CreateToolStripMenuItem_Click);
            tsmiData = new ToolStripMenuItem();
            _TEMPToolStripMenuItem = new ToolStripMenuItem();
            _TEMPToolStripMenuItem.Click += new EventHandler(TEMPToolStripMenuItem_Click);
            MenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // FlowLayoutPanel1
            // 
            FlowLayoutPanel1.AutoScroll = true;
            FlowLayoutPanel1.Dock = DockStyle.Fill;
            FlowLayoutPanel1.Location = new Point(0, 24);
            FlowLayoutPanel1.Name = "FlowLayoutPanel1";
            FlowLayoutPanel1.Size = new Size(535, 373);
            FlowLayoutPanel1.TabIndex = 1;
            // 
            // MenuStrip1
            // 
            MenuStrip1.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, tsmiData });
            MenuStrip1.Location = new Point(0, 0);
            MenuStrip1.Name = "MenuStrip1";
            MenuStrip1.Size = new Size(535, 24);
            MenuStrip1.TabIndex = 2;
            MenuStrip1.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _CreateToolStripMenuItem, _TEMPToolStripMenuItem });
            FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            FileToolStripMenuItem.Size = new Size(37, 20);
            FileToolStripMenuItem.Text = "File";
            // 
            // CreateToolStripMenuItem
            // 
            _CreateToolStripMenuItem.Name = "_CreateToolStripMenuItem";
            _CreateToolStripMenuItem.Size = new Size(180, 22);
            _CreateToolStripMenuItem.Text = "Create";
            // 
            // tsmiData
            // 
            tsmiData.Name = "tsmiData";
            tsmiData.Size = new Size(43, 20);
            tsmiData.Text = "Data";
            // 
            // TEMPToolStripMenuItem
            // 
            _TEMPToolStripMenuItem.Name = "_TEMPToolStripMenuItem";
            _TEMPToolStripMenuItem.Size = new Size(180, 22);
            _TEMPToolStripMenuItem.Text = "TEMP";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(535, 397);
            Controls.Add(FlowLayoutPanel1);
            Controls.Add(MenuStrip1);
            MainMenuStrip = MenuStrip1;
            Name = "Form1";
            Text = "Chorale";
            MenuStrip1.ResumeLayout(false);
            MenuStrip1.PerformLayout();
            Load += new EventHandler(Form1_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        internal FlowLayoutPanel FlowLayoutPanel1;
        internal MenuStrip MenuStrip1;
        internal ToolStripMenuItem FileToolStripMenuItem;
        internal ToolStripMenuItem tsmiData;
        private ToolStripMenuItem _CreateToolStripMenuItem;

        internal ToolStripMenuItem CreateToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _CreateToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_CreateToolStripMenuItem != null)
                {
                    _CreateToolStripMenuItem.Click -= CreateToolStripMenuItem_Click;
                }

                _CreateToolStripMenuItem = value;
                if (_CreateToolStripMenuItem != null)
                {
                    _CreateToolStripMenuItem.Click += CreateToolStripMenuItem_Click;
                }
            }
        }

        private ToolStripMenuItem _TEMPToolStripMenuItem;

        internal ToolStripMenuItem TEMPToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TEMPToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TEMPToolStripMenuItem != null)
                {
                    _TEMPToolStripMenuItem.Click -= TEMPToolStripMenuItem_Click;
                }

                _TEMPToolStripMenuItem = value;
                if (_TEMPToolStripMenuItem != null)
                {
                    _TEMPToolStripMenuItem.Click += TEMPToolStripMenuItem_Click;
                }
            }
        }
    }
}