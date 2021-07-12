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



        public struct Key
        {
            public Key(IntervalS pTonic, Mode pScale)
            {
                Tonic = pTonic;
                Scale = pScale;
            }

            public IntervalS Tonic;
            public Mode Scale;
        }

        public class Display
        {

            // what supported, no key signatures, no ledger lines, all as dots with no accidentals, or squares
            // update for degree
            // accidentals and key signature agreement

            public sealed class Clef
            {
                public int MCRankFromTopLine { get; }
                public string Name { get; }

                public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
                {
                    if (MCRankFromTopLine == 10)
                    {
                        g.DrawString("g", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 6 - 15));
                    }
                    else
                    {
                        g.DrawString("f", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 2 - 10));
                    }
                }

                public Clef(int mCRankFromTopLine, string name)
                {
                    MCRankFromTopLine = mCRankFromTopLine;
                    Name = name;
                }

                public override string ToString()
                {
                    return Name;
                }
            }

            public class TimeSignature
            {
                public static TimeSignature Common { get; private set; } = new TimeSignature();

                public double BarLengthW
                {
                    get
                    {
                        return 1d;
                    }
                }

                private TimeSignature()
                {
                }

                public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
                {
                    g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY));
                    g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY + rankHeightY * 4));
                }
            }

            public class Part
            {
                public Clef Clef;
                public Event[][][] Bars;
            }

            public struct Event
            {
                public Pitch? Pitch;
                public int WholeDivisionPower;
                public int Dot;
            }

            public TimeSignature TS;
            public Key Key;
            public Part[] Parts;

            public Bitmap Draw()
            {
                return Draw(Parts, TS, Key.Tonic, Key.Scale);
            }

            private const int MainMarginX = 30;
            private const int MainMarginY = 30;
            private const int StaveMargin = 13;
            private const int SignatureWidth = 80;
            private const int PostSignatureMargin = 13;
            private const int BarLineMargin = 8;
            private const int StaffSpacing = 57;
            private const int RankSpacing = 5;
            private const int BarWidth = 85;
            public const int stemlength = 17;

            private static Bitmap Draw(Part[] parts, TimeSignature ts, IntervalS key, Mode Scale)
            {
                Bitmap DrawRet = default;
                DrawRet = new Bitmap(2 * MainMarginX + StaveMargin + SignatureWidth + PostSignatureMargin + parts.Aggregate(0, (x, y) => Math.Max(x, y.Bars.Count())) * BarWidth - BarLineMargin / 2, MainMarginY * 2 + parts.Length * (8 * RankSpacing + StaffSpacing) - StaffSpacing);
                using (var g = Graphics.FromImage(DrawRet))
                {
                    for (int index = 0, loopTo = parts.Length - 1; index <= loopTo; index++)
                        DrawStave(g, parts[index].Clef, parts[index].Bars, MainMarginY + index * (8 * RankSpacing + StaffSpacing), RankSpacing, MainMarginX, BarWidth, ts, key, Scale);
                }

                return DrawRet;
            }

            private static void DrawKeySignature(Graphics g, int topLineY, int rankHeightY, int startX, int widthX, IntervalS Key, Mode Scale, Clef clef)
            {
                int position = 0;
                for (int ScaleDegree = 0; ScaleDegree <= 6; ScaleDegree++)
                {
                    var AbsoluteNote = Key + Scale.Interval(ScaleDegree).Value;
                    int offset = AbsoluteNote.ResidueSemis - Mode.Zero.Interval(AbsoluteNote.ResidueNumber).Value.ResidueSemis;
                    switch (offset)
                    {
                        case 0:
                            {
                                break;
                            }

                        case 1:
                            {
                                int currentrank = clef.MCRankFromTopLine - (int)AbsoluteNote.ResidueNumber;
                                while (currentrank < 0)
                                    currentrank += 7;
                                while (currentrank >= 7)
                                    currentrank -= 7;
                                g.DrawString("#", new Font("calibri", 12f), Brushes.Black, new Point(startX + 27 + position * 8, topLineY + currentrank * rankHeightY - 10));
                                position += 1;
                                break;
                            }

                        case -1:
                            {
                                int currentrank = clef.MCRankFromTopLine - (int)AbsoluteNote.ResidueNumber;
                                while (currentrank < 0)
                                    currentrank += 7;
                                while (currentrank >= 7)
                                    currentrank -= 7;
                                g.DrawString("b", new Font("calibri", 12f), Brushes.Black, new Point(startX + 27 + position * 8, topLineY + currentrank * rankHeightY - 10));
                                position += 1;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                            // CRITICAL
                            // Throw New NotImplementedException()
                    }
                }
            }

            private static void DrawStave(Graphics g, Clef clef, Event[][][] bars, int topLineY, int rankHeightY, int startX, int barWidth, TimeSignature TimeSignature, IntervalS Key, Mode Scale)
            {
                g.DrawLine(Pens.Black, startX, topLineY, startX, topLineY + rankHeightY * 8);
                TimeSignature.Draw(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth);
                clef.Draw(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth);
                DrawKeySignature(g, topLineY, rankHeightY, startX + StaveMargin, SignatureWidth, Key, Scale, clef);
                int StartPosition = startX + StaveMargin + SignatureWidth + PostSignatureMargin;
                for (int line = 0; line <= 4; line++)
                    g.DrawLine(Pens.Black, startX, topLineY + line * rankHeightY * 2, StartPosition + bars.Count() * barWidth - BarLineMargin / 2, topLineY + line * rankHeightY * 2);
                for (int index = 0, loopTo = bars.Count() - 1; index <= loopTo; index++)
                {
                    g.DrawLine(Pens.Black, StartPosition + (index + 1) * barWidth - BarLineMargin / 2, topLineY, StartPosition + (index + 1) * barWidth - BarLineMargin / 2, topLineY + rankHeightY * 8);
                    DrawBar(g, clef, bars[index], topLineY, rankHeightY, StartPosition + index * barWidth + BarLineMargin, barWidth - BarLineMargin, TimeSignature.BarLengthW);
                }
            }

            // must have at least one voice
            private static void DrawBar(Graphics g, Clef clef, Event[][] voices, int topLineY, int rankHeightY, int startX, int barWidth, double barWidthW)
            {
                switch (voices.Length)
                {
                    case 1:
                        {
                            DrawVoice(g, clef, voices[0], 0, 4, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            break;
                        }

                    case 2:
                        {
                            DrawVoice(g, clef, voices[0], -1, 1, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            DrawVoice(g, clef, voices[1], 1, 7, topLineY, rankHeightY, startX, barWidth, barWidthW);
                            break;
                        }

                    default:
                        {
                            throw new NotImplementedException();
                            break;
                        }
                }
            }

            // use stem direction
            private static void DrawVoice(Graphics g, Clef clef, Event[] voice, int stems, int restRankFromTopLine, int topLineY, int rankHeightY, int startX, int barWidth, double barWidthW)
            {
                double timeW = 0d;
                for (int index = 0, loopTo = voice.Length - 1; index <= loopTo; index++)
                {
                    int PositionY;
                    if (voice[index].Pitch.HasValue)
                    {
                        int ranknumber = clef.MCRankFromTopLine - voice[index].Pitch.Value.IntervalFromC0.Number - 4 * 7;
                        int X1 = startX + (int)Math.Round(timeW / barWidthW * barWidth) - rankHeightY;
                        int X2 = startX + (int)Math.Round(timeW / barWidthW * barWidth) + rankHeightY;
                        for (int ledgerRank = -2, loopTo1 = ranknumber; ledgerRank >= loopTo1; ledgerRank -= 2)
                            g.DrawLine(Pens.Black, X1 - 2, topLineY + rankHeightY * ledgerRank, X2 + 2, topLineY + rankHeightY * ledgerRank);
                        for (int ledgerRank = 10, loopTo2 = ranknumber; ledgerRank <= loopTo2; ledgerRank += 2)
                            g.DrawLine(Pens.Black, X1 - 2, topLineY + rankHeightY * ledgerRank, X2 + 2, topLineY + rankHeightY * ledgerRank);
                        PositionY = topLineY + rankHeightY * ranknumber;
                        g.FillEllipse(Brushes.Black, new Rectangle(X1, PositionY - rankHeightY, rankHeightY * 2, rankHeightY * 2));
                        if (stems == -1 || stems == 0 && ranknumber <= 4)
                        {
                            g.DrawLine(Pens.Black, X2, PositionY, X2, PositionY - stemlength);
                        }
                        else
                        {
                            g.DrawLine(Pens.Black, X1, PositionY, X1, PositionY + stemlength);
                        }
                    }
                    else
                    {
                        PositionY = topLineY + rankHeightY * restRankFromTopLine;
                        g.FillEllipse(Brushes.Blue, new Rectangle(startX + (int)Math.Round(timeW / barWidthW * barWidth) - rankHeightY, PositionY - rankHeightY, rankHeightY * 2, rankHeightY * 2));
                    }

                    switch (voice[index].Dot)
                    {
                        case 0:
                            {
                                timeW += Math.Pow(2d, -voice[index].WholeDivisionPower);
                                break;
                            }

                        case 1:
                            {
                                timeW += Math.Pow(2d, -voice[index].WholeDivisionPower) * 1.5d;
                                break;
                            }

                        default:
                            {
                                throw new NotImplementedException();
                                break;
                            }
                    }
                }
            }
        }


        public class MIDIData
        {
            public Pitch[][] Notes;
            public Key Key;

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

                var part1 = new Display.Part() { Bars = bars1.ToArray(), Clef = new Display.Clef(10, "Treble") };
                var part2 = new Display.Part() { Bars = bars2.ToArray(), Clef = new Display.Clef(-2, "Bass") };
                var obj = new Display() { Parts = new[] { part1, part2 }, TS = Display.TimeSignature.Common, Key = Key };
                return obj.Draw();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
             MessageBox.Show(mus.notation.test());
        }
    }
}
