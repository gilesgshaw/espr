using aud;
using NAudio.Wave;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static aud.AudioUt;
using static intp.Ut;

namespace intp
{
    //remote static tags
    //updated using PB.Refresh(). See instances.
    //channels... NAudio.Wave.WaveIn.GetCapabilities(devicenumber).Channels
    public partial class frmMain : Form
    {

        const int GlobalSampleRate = 44100;
        const int BufferSeconds = 10;

        //Rounding errors using these values...
        //STOP SETTINGS GOING WRONG, ARB
        GuiSetting<decimal   > ProcessFPS;
        GuiSetting<decimal   > CanvasLength;
        GuiSetting<decimal   > FloorDB;
        GuiSetting<decimal   > CeilDB;
        GuiSetting<NamedPitch> MinFreq;
        GuiSetting<NamedPitch> MaxFreq;
        GuiSetting<decimal   > LocalFreq;
        GuiSetting<decimal   > PointsPerOctave;
        GuiSetting<decimal   > ISC;
        GuiSetting<bool      > Follow;
        GuiSetting<bool      > Fundamentals;
        GuiSetting<decimal   > HarmonicsNum;
        GuiSetting<decimal   > FundamentalSens;
        GuiSetting<bool      > Hide;

        //thing gets forgotten if rolling wave is not long enough
        //could be more efficient possibly on converting to float
        WaveRolling Data = new WaveRolling(GlobalSampleRate * BufferSeconds, GlobalSampleRate);
        object ProcessLock = new object();
        float NextProcessPropCm = 0;
        int NextProcessAvr = 0;
        Spectrum sp;

        WaveIn inStream = null;
        WaveOut outStream = null;
        BufferedWaveProvider bwp = null;

        SettingPanel pnl;
        void InitializeInterface()
        {

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                coIn.Items.Add(WaveIn.GetCapabilities(i).ProductName + " (" + WaveIn.GetCapabilities(i).Channels.ToString() + " ch.)");
            }
            coIn.SelectedIndex = 0;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                coOut.Items.Add(WaveOut.GetCapabilities(i).ProductName + " (" + WaveOut.GetCapabilities(i).Channels.ToString() + " ch.)");
            }
            coOut.SelectedIndex = 0;

            pnl = new SettingPanel(tlpSettings, 116);

            ProcessFPS = new NudSetting(60, 1, 20, 5);
            pnl.Add("Process Rate (FPS)", ProcessFPS);

            FloorDB = new NudSetting(60, -300, -50, -120, 2);
            pnl.Add("Floor (DB)", FloorDB);

            CeilDB = new NudSetting(60, -48, 0, -30, 2);
            pnl.Add("Ceiling (DB)", CeilDB);

            MinFreq = new PudSetting(60, (Pitch)new NamedPitch(Letter.A, 0), (Pitch)new NamedPitch(Letter.C, 4), new NamedPitch(Letter.F, 2));
            pnl.Add("Min Freq.", MinFreq);

            MaxFreq = new PudSetting(60, (Pitch)new NamedPitch(Letter.C, 1, 4), (Pitch)new NamedPitch(Letter.C, 10), new NamedPitch(Letter.C, 8));
            pnl.Add("Max Freq.", MaxFreq);

            LocalFreq = new NudSetting(60, 20, 1000, 70, 5);
            pnl.Add("Comparison Freq.", LocalFreq);

            PointsPerOctave = new NudSetting(60, 12, 120, 24, 12);
            pnl.Add("Pts. per Octave", PointsPerOctave);

            CanvasLength = new NudSetting(60, 10, 300, 30, 2);
            pnl.Add("Canvas Length (s)", CanvasLength);

            ISC = new NudSetting(60, 50, 1000, 300, 50);
            pnl.Add("Initial Samples", ISC);

            Follow = new CbSetting(true);
            pnl.Add("More as Needed", Follow);

            //give (strig, null) for now, Sketchy
            pnl.Add(string.Empty, null);

            Fundamentals = new CbSetting(false);
            pnl.Add("Show Fundamentals", Fundamentals);

            HarmonicsNum = new ConditionalSetting<decimal>(new NudSetting(60, 1, 10, 5), Fundamentals);
            pnl.Add("Combining Harmonics", HarmonicsNum);

            FundamentalSens = new ConditionalSetting<decimal>(new NudSetting(60, 1, 5, 1, 0.1M, 1), Fundamentals);
            pnl.Add("Sensitivity", FundamentalSens);

            Hide = new ConditionalSetting<bool>(new CbSetting(false), Fundamentals, true);
            pnl.Add("Hide Original", Hide);
        }

        //Sketch
        void TryUpdateLabels()
        {
            try
            {
                Invoke(new Action(() => {
                    try
                    {
                        ddGraph.Text = "Graphics Queue: " + (CStatus.HasFlag(Status.In) ? sp.GraphicsQueue.ToString() : "-") + "/" + sp.GraphicsQueueMax;
                        ddProc.Text  = "Process Queue: "  + (CStatus.HasFlag(Status.In) ? sp.ProcessQueue.ToString()  : "-") + "/" + sp.ProcessQueueMax;
                    }
                    catch (Exception) {}
                }));
            }
            catch (Exception) {}
        }

        //Sketch. takes off 8.
        public void AddLRTrack(bool left, Color ForeColor, Color BackColor, Font Font, Control Item, Func<(double, double)> Ends, Func<double, string> GetText, Func<bool> Dtmn = null)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Visible = false;
            lbl.Font = Font;
            lbl.ForeColor = ForeColor;
            lbl.BackColor = BackColor;
            Item.Parent.Controls.Add(lbl);
            lbl.BringToFront();
            Item.MouseLeave += (sender, e) => lbl.Hide();
            if (left)
            {
                Item.MouseMove += (sender, e) =>
                {
                    if (Dtmn != null && !Dtmn()) return;
                    var ends = Ends();
                    lbl.Show();
                    lbl.Text = GetText(ends.Item1 + (((ends.Item2 - ends.Item1) * (1 - e.Y / (double)Item.Height))));
                    lbl.Left = Item.Left + 1;
                    lbl.Top = Item.Top + e.Y - 5;
                };
            }
            else
            {
                Item.MouseMove += (sender, e) =>
                {
                    if (Dtmn != null && !Dtmn()) return;
                    var ends = Ends();
                    lbl.Show();
                    lbl.Text = GetText(ends.Item1 + (((ends.Item2 - ends.Item1) * (1 - e.Y / (double)Item.Height))));
                    lbl.Left = Item.Right - 1 - lbl.Width;
                    lbl.Top = Item.Top + e.Y - 5;
                };
            }
        }

        //Sketch. Set background so app can close probably.
        void Sketch()
        {
            AddLRTrack(false, Color.White, Color.Black, Font, pbSpect, () => (Math.Log(MinFreq.Value.Pitch.f), Math.Log(MaxFreq.Value.Pitch.f)), (x) => FreqToString(Math.Exp(x)));
            AddLRTrack(true, Color.White, Color.Black, Font, pbSpect, () => (Math.Log(MinFreq.Value.Pitch.f), Math.Log(MaxFreq.Value.Pitch.f)), (x) => Pitch.Closest(Math.Exp(x)).ToString());
            var g = new System.Threading.Thread(() =>
            {
                while (!IsDisposed)
                {

                    //PerformanceCounter cpuCounter = new PerformanceCounter();
                    //cpuCounter.CategoryName = "Processor";
                    //cpuCounter.CounterName = "% Processor Time";
                    //cpuCounter.InstanceName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

                    //// will always start at 0
                    //float firstValue = cpuCounter.NextValue();
                    //// now matches task manager reading
                    //float secondValue = cpuCounter.NextValue();
                    //System.Threading.Thread.Sleep(1000);

                    TryUpdateLabels();
                    System.Threading.Thread.Sleep(100);

                }
            });
            g.IsBackground = true;
            g.Priority = System.Threading.ThreadPriority.AboveNormal;
            g.Start();

            miDecProc.Enabled = (sp.ProcessQueueMax > 1);
            miDecGraph.Enabled = (sp.GraphicsQueueMax > 1);
            miDecProc.Click += (sender, e) => { sp.ProcessQueueMax -= 1; miDecProc.Enabled = (sp.ProcessQueueMax > 1); };
            miDecGraph.Click += (sender, e) => { sp.GraphicsQueueMax -= 1; miDecGraph.Enabled = (sp.GraphicsQueueMax > 1); };
            miIncProc.Click += (sender, e) => { sp.ProcessQueueMax += 1; miDecProc.Enabled = (sp.ProcessQueueMax > 1); };
            miIncGraph.Click += (sender, e) => { sp.GraphicsQueueMax += 1; miDecGraph.Enabled = (sp.GraphicsQueueMax > 1); };

            ////PerformanceCounter cpuCounter;
            ////cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ////pbSpect.Click += (sender, e) => Text = cpuCounter.NextValue() + "%";
        }

        public frmMain()
        {
            InitializeComponent();
            InitializeInterface();
            StatusToInterface();

            sp = new Spectrum((double)MinFreq.Value.Pitch.f, (double)MaxFreq.Value.Pitch.f, pbSpect.Size);

            pbSpect.SizeChanged += (sender, e) => { if (!(sp is null)) sp.Size = ((PictureBox)sender).Size; };
            //IMP not draw whole thing, and not update whole thing.
            //ARB, 2
            pbSpect.Paint += (sender, e) => { if (!(sp is null)) sp.Draw(e.Graphics, ((PictureBox)sender).Size,
                (NextProcessPropCm + (1F / ((float)CanvasLength.Value * (float)ProcessFPS.Value)) * (Data.End - NextProcessAvr) * (float)ProcessFPS.Value / (float)GlobalSampleRate) % 1, 2); };

            MinFreq.ValueChanged += (sender, e) => { if (!(sp is null)) sp.MinFreq = (double)MinFreq.Value.Pitch.f; };
            MaxFreq.ValueChanged += (sender, e) => { if (!(sp is null)) sp.MaxFreq = (double)MaxFreq.Value.Pitch.f; };

            FormClosing += (sender, e) => btnStop.PerformClick();
            btnStart.Click += (sender, e) => { if (CStatus == Status.None) startIn(); };
            btnStop.Click += (sender, e) => stopAll();
            this.cbThrough.CheckedChanged += (sender, e) => { if (cbThrough.Checked && CStatus == Status.In) startOut(); else if (!cbThrough.Checked) stopOut(); };

            //Sketch
            Sketch();
            TryUpdateLabels();
            Dev();
        }

        void Dev() {}

        //Supposedly: interface may trigger, Components update, then status updates, then inferface updates.
        Status CStatus = Status.None;
        [Flags] enum Status
        {
            None = 0,
            In = 1,
            Out = 2,
            Through = In | Out,
        }
        //should validate??
        void StatusToInterface()
        {
            btnStart.Enabled = !CStatus.HasFlag(Status.In);
            btnStop.Enabled = CStatus.HasFlag(Status.In);
            coIn.Enabled = !CStatus.HasFlag(Status.In);

            cbThrough.Enabled = CStatus.HasFlag(Status.In);
            coOut.Enabled = CStatus.HasFlag(Status.In) && !CStatus.HasFlag(Status.Out);
            cbThrough.Checked = CStatus.HasFlag(Status.In) && CStatus.HasFlag(Status.Out);
        }

        //must be None
        void startIn()
        {
            inStream = new WaveIn();
            inStream.DeviceNumber = coIn.SelectedIndex;
            inStream.WaveFormat = new WaveFormat(GlobalSampleRate, 1);
            inStream.DataAvailable += (sender, e) =>
            {
                if (!(bwp is null)) bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
                DataProc(e);
            };
            inStream.RecordingStopped += (sender, e) => { if (!(e.Exception is null)) stopAll(); };
            inStream.StartRecording();

            CStatus |= Status.In;
            StatusToInterface();
        }

        //must be In
        void startOut()
        {
            outStream = new WaveOut();
            outStream.DeviceNumber = coOut.SelectedIndex;
            //care about outStream.OutputWaveFormat?? 
            bwp = new BufferedWaveProvider(new WaveFormat(GlobalSampleRate, 1));
            //TODO
            bwp.DiscardOnBufferOverflow = true;
            outStream.Init(bwp);
            outStream.PlaybackStopped += (sender, e) => { if (!(e.Exception is null)) stopOut(); };
            outStream.Play();

            CStatus |= Status.Out;
            StatusToInterface();
        }

        void stopOut()
        {
            if (outStream != null)
            {
                outStream.Stop();
                outStream.Dispose();
                outStream = null;
            }
            bwp = null;
            CStatus &= Status.In;
            StatusToInterface();
        }

        void stopAll()
        {
            if (outStream != null)
            {
                outStream.Stop();
                outStream.Dispose();
                outStream = null;
            }
            bwp = null;
            if (inStream != null)
            {
                inStream.StopRecording();
                inStream.Dispose();
                inStream = null;
            }
            CStatus = Status.None;
            StatusToInterface();
        }

        async Task DataProc(WaveInEventArgs e)
        {

            float[] NewData = new float[e.BytesRecorded / 2];
            for (int i = 0; i < NewData.Length; i++)
            {
                int value = e.Buffer[i * 2] ^ (e.Buffer[i * 2 + 1] << 8);
                if (value >= 32768) { value -= 65536; }
                NewData[i] = (float)value / 32768;
            }
            Data.Extend(NewData);
            pbSpect.Refresh();

            //ARB arbitrary choice of when to process next
            while (NextProcessAvr + GlobalSampleRate / ProcessFPS.Value < Data.End)
            {
                float ThisProcessPropCm;
                int ThisProcessAvr;
                lock (ProcessLock)
                {
                    ThisProcessPropCm = NextProcessPropCm;
                    ThisProcessAvr = NextProcessAvr;
                    NextProcessPropCm += (1F / ((float)CanvasLength.Value * (float)ProcessFPS.Value));
                    NextProcessAvr += (int)(GlobalSampleRate / ProcessFPS.Value);
                }
                if (Fundamentals.Value) {
                    var ceil = (double)CeilDB.Value;
                    var floor = (double)FloorDB.Value;
                    var mid = ceil - floor;
                    await sp.ProcessPosition(Data, ThisProcessAvr, ThisProcessPropCm % 1, (1F / ((float)CanvasLength.Value * (float)ProcessFPS.Value)),
                        floor, ceil, (int)PointsPerOctave.Value, (int)LocalFreq.Value, (int)ISC.Value, Follow.Value,
                        (int)HarmonicsNum.Value, mid - mid / (double)FundamentalSens.Value, Hide.Value);
                }
                else
                {
                    await sp.ProcessPosition(Data, ThisProcessAvr, ThisProcessPropCm % 1, (1F / ((float)CanvasLength.Value * (float)ProcessFPS.Value)),
                        (double)FloorDB.Value, (double)CeilDB.Value, (int)PointsPerOctave.Value, (int)LocalFreq.Value, (int)ISC.Value, Follow.Value,
                        0, 0, false);
                }
                pbSpect.Refresh();
            }

        }

        //critical
        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            //if (disposing && (sp != null))
            //{
            //    sp.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
