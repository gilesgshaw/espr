using System;
using System.Drawing;
using static mus.notation;

namespace Chorale
{
    public partial class Display
    {

        public struct Key
        {
            public IntervalS Tonic { get; set; }
            public Mode Scale { get; set; }

            public Key(IntervalS tonic, Mode scale)
            {
                Tonic = tonic;
                Scale = scale;
            }

            public void DrawSig(Graphics g, int top, int rankHeight, int left, int width, Clef clef)
            {
                int position = 0;
                for (int ScaleDegree = 0; ScaleDegree <= 6; ScaleDegree++)
                {
                    var AbsoluteNote = Tonic + Scale.Interval(ScaleDegree).Value;
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
                                g.DrawString("#", new Font("calibri", 12f), Brushes.Black, new Point(left + 27 + position * 8, top + currentrank * rankHeight - 10));
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
                                g.DrawString("b", new Font("calibri", 12f), Brushes.Black, new Point(left + 27 + position * 8, top + currentrank * rankHeight - 10));
                                position += 1;
                                break;
                            }

                        default:
                            {
                                throw new NotImplementedException();
                            }
                    }
                }
            }
        }

    }
}
