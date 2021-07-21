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

namespace Chorale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class MIDIData
        {

            public Pitch[][] Notes;
            public Display.Key Key;

            private static MidiOut midiOut = new MidiOut(0);

            public void Play()
            {
                int channel = 1;

                for (int i = 0; i < Notes.Length; i++)
                {

                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][0].MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][1].MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][2].MIDIPitch, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][3].MIDIPitch, 100, 50).GetAsShortMessage());

                    System.Threading.Thread.Sleep(i == Notes.Length - 1 ? 1400 : 800);

                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][0].MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][1].MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][2].MIDIPitch, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, Notes[i][3].MIDIPitch, 0, 50).GetAsShortMessage());

                    System.Threading.Thread.Sleep(i == Notes.Length - 1 ? 0 : 100);
                }
            }

            public Bitmap GetBitmap()
            {
                var bars1 = new List<Display.Event[][]>();
                var bars2 = new List<Display.Event[][]>();
                for (int Index = 0, loopTo = Notes.Length - 1; Index <= loopTo; Index += 4)
                {
                    var voice1 = new List<Display.Event>();
                    var voice2 = new List<Display.Event>();
                    for (int SubIndex = Index, loopTo1 = Math.Min(Notes.Length - 1, Index + 3); SubIndex <= loopTo1; SubIndex++)
                    {
                        if (Notes[SubIndex].Length > 0)
                            voice1.Add(new Display.Event() { Pitch = Notes[SubIndex][0], WholeDivisionPower = 2 });
                        if (Notes[SubIndex].Length > 1)
                            voice2.Add(new Display.Event() { Pitch = Notes[SubIndex][1], WholeDivisionPower = 2 });
                    }

                    bars1.Add(new[] { voice1.ToArray(), voice2.ToArray() });
                    voice1 = new List<Display.Event>();
                    voice2 = new List<Display.Event>();
                    for (int SubIndex = Index, loopTo2 = Math.Min(Notes.Length - 1, Index + 3); SubIndex <= loopTo2; SubIndex++)
                    {
                        if (Notes[SubIndex].Length > 2)
                            voice1.Add(new Display.Event() { Pitch = Notes[SubIndex][2], WholeDivisionPower = 2 });
                        if (Notes[SubIndex].Length > 3)
                            voice2.Add(new Display.Event() { Pitch = Notes[SubIndex][3], WholeDivisionPower = 2 });
                    }

                    bars2.Add(new[] { voice1.ToArray(), voice2.ToArray() });
                }

                var part1 = new Display.Stave() { Bars = bars1.ToArray(), Clef = new Display.Clef(10, 5, "Treble"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var part2 = new Display.Stave() { Bars = bars2.ToArray(), Clef = new Display.Clef(-2, 5, "Bass"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var obj = new Display() { Staves = new[] { part1, part2 } };
                return obj.Draw();
            }

        }



        private PictureBox CreatePB(Passage data, Mode SigMode)
        {
            var Data = data.Verts;

            var result = Data.ToArray();
            var Notes = new Pitch[result.Length][];

            for (int i = 0; i < result.Length; i++)
            {
                Notes[i] = new Pitch[] {
                    new Pitch(data.Tonic + result[i].Chord.Root + result[i].Voicing.S),
                    new Pitch(data.Tonic + result[i].Chord.Root + result[i].Voicing.A),
                    new Pitch(data.Tonic + result[i].Chord.Root + result[i].Voicing.T),
                    new Pitch(data.Tonic + result[i].Chord.Root + result[i].Voicing.B)
                };
            }

            var nData = new MIDIData()
            {
                Notes = Notes,
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
            foreach (var item in result.Item1)
            {
                CreatePB(item, result.Item2);
            }
        }
    }
}
