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

        private static bool ArrayParse<T>(string[] strs, Parser<T> parser, out T[] result)
        {
            result = default;
            var tr = new T[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (!parser(strs[i], out tr[i])) return false;
            }
            result = tr;
            return true;
        }

        private static bool ArrayParse<T>(string str, Parser<T> parser, out T[] result)
        {
            var items = str.Split(" ,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return ArrayParse(items, parser, out result);
        }

        private delegate bool Parser<T>(string str, out T result);



        Dictionary<UserTonality, Context> Contexts = new Dictionary<UserTonality, Context>();

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
            tbMelody.Lines = Array.ConvertAll(Settings.S.lines, ArrayString);

            foreach (var item in lbContexts.Items.OfType<UserTonality>())
            {
                Contexts[item] = new Context(item.Tonic, Settings.S.Tonalities[item.Name]);
            }
        }

        private int[] maxes = Settings.S.maxes[0]; // [here] currently no control over this.
        private double[] tols = Settings.S.tols[0]; // [here] currently no control over this.



        private void Harmonise()
        {

            // user values
            Pitch[][] lines;
            Climate climate;
            int disp;
            bool initial;

            // determine
            if (!ArrayParse(tbMelody.Lines, (string x, out Pitch[] o) => ArrayParse(x, Pitch.TryParse, out o), out lines))
            {
                MessageBox.Show("failed to parse melody.");
                return;
            }
            climate = new Climate(
                lbContexts.CheckedItems.OfType<UserTonality>()
                .Concat(lbContexts.SelectedItems.OfType<UserTonality>())
                .Select((x) => Contexts[x])
                .ToArray(),
                Contexts[(UserTonality)lbContexts.SelectedItem]);
            disp = 0; // [here] currently no control uver these.
            initial = true;

            // pass to main routine
            var problems = Array.ConvertAll(lines, (line) => PhraseSt.Instance(climate, Array.AsReadOnly(Array.ConvertAll(line, (x) => x.MIDI)), disp, initial));
            var solver = new PhraseSolver(maxes, tols);
            Work(problems, solver);

        }

        private void Work(PhraseSt[] problems, PhraseSolver solver)
        {

            var solutions = new Phrase[problems.Length];
            for (int i = 0; i < problems.Length; i++)
            {
                foreach (var item in solver.Solve(problems[i]))
                {
                    solutions[i] = item;
                    break; // [here] currently only considering the first option.
                }
            }

            ctr.Controls.Clear();
            // takes key signature from first problem
            var key = problems[0].Climate.Home;
            ctr.Controls.Add(CreatePB(solutions, new Display.Key(key.Tonic.FromC, key.Tonality.SignatureMode)));
        }

        // chord symbols are currently disabled here.
        private static PictureBox CreatePB(Phrase[] input, Display.Key Key)
        {

            var Data = input
                .SelectMany((x) => Enumerable.Zip(Enumerable.Zip(x.Moments, x.LContext, (m, l) => (m, l)), x.RContext, (ml, r) => (ml.m, ml.l, r)))
                .ToArray();

            var S = new ChoraleData.Beat[Data.Length];
            var A = new ChoraleData.Beat[Data.Length];
            var T = new ChoraleData.Beat[Data.Length];
            var B = new ChoraleData.Beat[Data.Length];
            var C = new (string, string)[Data.Length];

            for (int i = 0; i < Data.Length; i++)
            {

                S[i] = new ChoraleData.Beat(Data[i].m.S);
                A[i] = new ChoraleData.Beat(Data[i].m.A);
                T[i] = new ChoraleData.Beat(Data[i].m.T);
                B[i] = new ChoraleData.Beat(Data[i].m.B);
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
