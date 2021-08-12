using System;
using System.Drawing;
using static Notation.Ut;
using static mus.Ut;
using mus;

namespace Notation
{
    public partial class Display
    {

        public struct Key
        {
            public IntervalS Tonic { get; set; } // from C
            public Mode Mode { get; set; } // from Tonic

            public Mode GetModeFromC()
            {
                return new Mode(
                    (Tonic + Mode.IntervalS(0 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(1 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(2 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(3 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(4 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(5 - Tonic.ResidueNumber)).Quality,
                    (Tonic + Mode.IntervalS(6 - Tonic.ResidueNumber)).Quality, Mode.Symbol);
            }

            private static readonly Mode naturalMode = Mode.Zero; // the 'white notes' (i.e. from C)

            public Key(IntervalS tonic, Mode scale)
            {
                Tonic = tonic;
                Mode = scale;
            }

            public void DrawSig(Graphics g, RectangleF rect, Clef clef)
            {
                var rankHeight = rect.Height / clef.NumSpaces / 2;
                int position = 0;
                for (int ScaleDegree = 0; ScaleDegree < 7; ScaleDegree++)
                {
                    var AbsoluteNote = Tonic + Mode.IntervalS(ScaleDegree);
                    switch (naturalMode.Accidental(AbsoluteNote))
                    {
                        case 1:
                            {
                                int currentrank = mod(7, clef.MCRankFromTopLine - AbsoluteNote.ResidueNumber);
                                g.DrawString("#", new Font("calibri", 12f), Brushes.Black, new PointF(rect.Left + 27 + position * 8, rect.Top + currentrank * rankHeight - 10));
                                position += 1;
                                break;
                            }

                        case -1:
                            {
                                int currentrank = mod(7, clef.MCRankFromTopLine - AbsoluteNote.ResidueNumber);
                                g.DrawString("b", new Font("calibri", 12f), Brushes.Black, new PointF(rect.Left + 27 + position * 8, rect.Top + currentrank * rankHeight - 10));
                                position += 1;
                                break;
                            }

                        case 0: break;
                        default: throw new NotImplementedException();
                    }
                }
            }
        }

    }
}
