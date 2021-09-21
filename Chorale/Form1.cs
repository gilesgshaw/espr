using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mus;
using mus.Chorale;
using Notation;
using static mus.Notation;

namespace Chorale
{
    public partial class Form1 : Form
    {


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
            public (string, string)[] ChordNames;

            public Display.Key Key;

            private static MidiOut midiOut;

            public void Play()
            {

                if (midiOut == null) midiOut = new MidiOut(0);

                int channel = 1;

                for (int i = 0; i < S.Length; i++)
                {

                    midiOut.Send(new NoteOnEvent(0, channel, S[i].Pitch.MIDI, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, A[i].Pitch.MIDI, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, T[i].Pitch.MIDI, 100, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, B[i].Pitch.MIDI, 100, 50).GetAsShortMessage());

                    System.Threading.Thread.Sleep(i == S.Length - 1 ? 1400 : 800);

                    midiOut.Send(new NoteOnEvent(0, channel, S[i].Pitch.MIDI, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, A[i].Pitch.MIDI, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, T[i].Pitch.MIDI, 0, 50).GetAsShortMessage());
                    midiOut.Send(new NoteOnEvent(0, channel, B[i].Pitch.MIDI, 0, 50).GetAsShortMessage());

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
                    Stave1Events.Add(new Display.ChordSymbol()
                    {
                        BarNumber = i / 4,
                        timeW = (float)i / 4 - (i / 4),
                        Text = ChordNames[i]
                    });
                }

                var stave1 = new Display.Stave() { NumberOfBars = (S.Length - 1) / 4 + 1, Events = Stave1Events.ToArray(), Clef = new Display.Clef(10, 5, "Treble"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var stave2 = new Display.Stave() { NumberOfBars = (S.Length - 1) / 4 + 1, Events = Stave2Events.ToArray(), Clef = new Display.Clef(-2, 5, "Bass"), TimeSignature = Display.TimeSignature.Common, Key = Key };
                var obj = new Display() { Staves = new[] { stave1, stave2 } };
                return obj.Draw();

            }

        }

        private class UserTonality
        {
            public string Name;
            public Note Tonic;

            public override string ToString()
            {
                return Tonic.ToString() + " " + Name;
            }

            public UserTonality(string name, Note tonic)
            {
                Name = name;
                Tonic = tonic;
            }
        }

        private static (string, string) NameChord(Vert chord)
        {
            (string, string) tr;
            if (chord.Chord.Variety.Symbol.Item1)
            { //lower
                tr = (Degree.Roman(chord.Chord.Root.ResidueNumber).ToLower(), chord.Chord.Variety.Symbol.Item2);
            }
            else
            { //upper
                tr = (Degree.Roman(chord.Chord.Root.ResidueNumber).ToUpper(), chord.Chord.Variety.Symbol.Item2);
            }
            switch (chord.Voicing.B.ResidueNumber)
            {
                case 0:
                    break;
                case 2:
                    tr.Item2 += "b";
                    break;
                case 4:
                    tr.Item2 += "c";
                    break;
                case 6:
                    tr.Item2 += "d";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return tr;
        }

        private static string ArrayString<T>(T[] array) => string.Join(" ", Array.ConvertAll(array, (x) => x.ToString()));

        private static bool ArrayParse<T>(string str, Parser<T> parser, out T[] result)
        {
            result = default;
            var items = str.Split(" ,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var tr = new T[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if (!parser(items[i], out tr[i])) return false;
            }
            result = tr;
            return true;
        }

        private delegate bool Parser<T>(string str, out T result);



        public Form1()
        {
            InitializeComponent();
            btnHarmonise.Click += (s, e) => Harmonise();

            foreach (var item in new[] { ("Major", 0, 0), ("Minor", 5, 9) })
            {
                for (int i = -6; i <= 6; i++)
                {
                    var interval = new Note(new IntervalS(new IntervalC(i * 4 + item.Item2, i * 7 + item.Item3)));
                    lbContexts.Items.Add(new UserTonality(item.Item1, interval));
                }
            }
            tbMelody.Text = ArrayString(Settings.S.lines[0]);
        }

        private int[] maxes = Settings.S.maxes[0]; // [here] currently no control over this.
        private double[] tols = Settings.S.tols[0]; // [here] currently no control over this.



        private void Harmonise()
        {

            // user values
            Pitch[] line;
            Context[] contexts;
            int disp;
            bool initial;

            // determine
            if (!ArrayParse(tbMelody.Text, Pitch.TryParse, out line))
            {
                MessageBox.Show("failed to parse melody.");
                return;
            }
            contexts =
                lbContexts.CheckedItems.OfType<UserTonality>() // [here] home key is currently asked of user, but not used by program:
                .Concat(lbContexts.SelectedItems.OfType<UserTonality>())
                .Select((x) => new Context(x.Tonic, Settings.S.Tonalities[x.Name]))
                .ToArray();
            disp = 0; // [here] currently no control uver these.
            initial = true;

            // pass to main routine
            var problem = PhraseSt.Instance(new Climate(contexts), Array.AsReadOnly(Array.ConvertAll(line, (x) => x.MIDI)), disp, initial);
            var solver = new PhraseSolver(maxes, tols);
            Work(problem, solver);

        }

        private void Work(PhraseSt problem, PhraseSolver solver)
        {
            ctr.Controls.Clear();
            foreach (var item in solver.Solve(problem))
            {
                // [TODO] implement key signature
                ctr.Controls.Add(CreatePB(item, new Display.Key(default, Mode.Zero)));
                break; // [here] currently only showing first option.
            }
        }

        // chord symbols are currently disabled here.
        private static PictureBox CreatePB(Phrase Data, Display.Key Key)
        {

            var S = new ChoraleData.Beat[Data.Length];
            var A = new ChoraleData.Beat[Data.Length];
            var T = new ChoraleData.Beat[Data.Length];
            var B = new ChoraleData.Beat[Data.Length];
            var C = new (string, string)[Data.Length];

            for (int i = 0; i < Data.Length; i++)
            {

                S[i] = new ChoraleData.Beat(Data.Moments[i].S);
                A[i] = new ChoraleData.Beat(Data.Moments[i].A);
                T[i] = new ChoraleData.Beat(Data.Moments[i].T);
                B[i] = new ChoraleData.Beat(Data.Moments[i].B);
                //C[i] = NameChord(Data.RVerts[i]); // [here] left chord as well...

            }

            var nData = new ChoraleData()
            {
                S = S,
                A = A,
                T = T,
                B = B,
                ChordNames = C,
                Key = Key
            };

            var pb = new PictureBox();
            pb.Image = nData.GetBitmap();
            pb.Size = pb.Image.Size;

            pb.Click += (s, e) => nData.Play();

            return pb;
        }


    }
}
