using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mus.notation;
using Notation;
using static Notation.Ut;
using System.Diagnostics;

namespace Chorale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class ChoraleData
        {

            public struct Beat
            {
                public Pitch Pitch { get; }

                public Beat(Pitch pitch)
                {
                    Pitch = pitch;
                }
            }

            public Beat[] S;
            public Beat[] A;
            public Beat[] T;
            public Beat[] B;

            public Display.Key Key;

            private static MidiOut midiOut;

            public void Play()
            {

                if (midiOut == null) midiOut = new MidiOut(0);

                int channel = 1;

                for (int i = 0; i < S.Length; i++)
                {

                    midiOut.Send(new NoteOnEvent(0, channel, S[i].Pitch.MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, A[i].Pitch.MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, T[i].Pitch.MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, B[i].Pitch.MIDIPitch, 100, 50).GetAsShortMessage());

                    System.Threading.Thread.Sleep(i == S.Length - 1 ? 1400 : 800);

                    midiOut.Send(new NoteOnEvent(0, channel, S[i].Pitch.MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, A[i].Pitch.MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, T[i].Pitch.MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, B[i].Pitch.MIDIPitch, 0, 50).GetAsShortMessage());

                    System.Threading.Thread.Sleep(i == S.Length - 1 ? 0 : 100);
                }
            }

            public Bitmap GetBitmap()
            {

                var Stave1Events = new List<Display.Event>();
                var Stave2Events = new List<Display.Event>();

                for (int i = 0; i < S.Length; i++)
                {
                    Stave1Events.Add(new Display.Note()
                    {
                        BarNumber = i / 4,
                        timeW = (float)i / 4 - (i / 4),
                        WholeDivisionPower = 2,
                        Voice = 0,
                        Pitch = S[i].Pitch,
                        col = NamedColor.Blue
                    });
                    Stave1Events.Add(new Display.Note()
                    {
                        BarNumber = i / 4,
                        timeW = (float)i / 4 - (i / 4),
                        WholeDivisionPower = 2,
                        Voice = 1,
                        Pitch = A[i].Pitch
                    });
                    Stave2Events.Add(new Display.Note()
                    {
                        BarNumber = i / 4,
                        timeW = (float)i / 4 - (i / 4),
                        WholeDivisionPower = 2,
                        Voice = 0,
                        Pitch = T[i].Pitch
                    });
                    Stave2Events.Add(new Display.Note()
                    {
                        BarNumber = i / 4,
                        timeW = (float)i / 4 - (i / 4),
                        WholeDivisionPower = 2,
                        Voice = 1,
                        Pitch = B[i].Pitch
                    });
                }

                var stave1 = new Display.Stave() { NumberOfBars = (S.Length - 1) / 4 + 1, Events = Stave1Events.ToArray(), Clef = new Display.Clef(10, 5, "Treble"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var stave2 = new Display.Stave() { NumberOfBars = (S.Length - 1) / 4 + 1, Events = Stave2Events.ToArray(), Clef = new Display.Clef(-2, 5, "Bass"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var obj = new Display() { Staves = new[] { stave1, stave2 } };
                return obj.Draw();

            }

        }



        private PictureBox CreatePB(Passage data, Mode SigMode)
        {
            var Data = data.Verts;

            var result = Data.ToArray();
            var S = new ChoraleData.Beat[result.Length];
            var A = new ChoraleData.Beat[result.Length];
            var T = new ChoraleData.Beat[result.Length];
            var B = new ChoraleData.Beat[result.Length];

            for (int i = 0; i < result.Length; i++)
            {
                S[i] = new ChoraleData.Beat(new Pitch(data.Tonic + (result[i].Chord.Root + result[i].Voicing.S)));
                A[i] = new ChoraleData.Beat(new Pitch(data.Tonic + (result[i].Chord.Root + result[i].Voicing.A)));
                T[i] = new ChoraleData.Beat(new Pitch(data.Tonic + (result[i].Chord.Root + result[i].Voicing.T)));
                B[i] = new ChoraleData.Beat(new Pitch(data.Tonic + (result[i].Chord.Root + result[i].Voicing.B)));
            }

            var nData = new ChoraleData()
            {
                S = S,
                A = A,
                T = T,
                B = B,
                Key = new Display.Key(data.Tonic, SigMode)
            };

            var pb = new PictureBox();
            pb.Image = nData.GetBitmap();
            pb.Size = pb.Image.Size;

            flowLayoutPanel1.Controls.Add(pb);
            toolTip1.SetToolTip(pb, data.Penalty.ToString());
            pb.DoubleClick += (s, e) => MessageBox.Show(data.Penalty.ToString());
            pb.Click += (s, e) => nData.Play();

            return pb;
        }

        private void getCadencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = test();
            foreach (var item in result.Item1.Where((x) => x != null))
            {
                CreatePB(item, result.Item2);
            }
            PrintPretty(result.Item3, (x) => Situation.Cache[x].Count.ToString(), (x) =>
            {
                if (x.Left.Sop.Length == 1) return new Situation[] { };
                return new[] { x.Left, x.Right };
            }
            , (x) => Debug.Write(x));
        }

        private void cacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in Situation.Cache)
            {
                Debug.WriteLine(item.Value.Count + " " + item.Key.Sop.Length);
            }
        }
    }
}
