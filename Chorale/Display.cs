using System;
using System.Drawing;
using System.Linq;
using static mus.notation;

namespace Chorale
{
    public partial class Display
    {

        private class L
        {
            public readonly static float StaveHeight = RankSpacing * 8;
            public readonly static float StaveDisplacement = StaveHeight + StaffSpacing;

            public const float stemlength = 17;
            public const float MainMarginX = 30;
            public const float MainMarginY = 30;
            public const float StaveMargin = 13;
            public const float SignatureWidth = 80;
            public const float PostSignatureMargin = 13;
            public const float BarLineMargin = 8;
            public const float StaffSpacing = 57;
            public const float RankSpacing = 5;
            public const float BarWidth = 85;
        }

        // what supported, no key signatures, no ledger lines, all as dots with no accidentals, or squares
        // update for degree
        // accidentals and key signature agreement

        public class Stave
        {
            public Clef Clef;
            public TimeSignature TimeSignature;
            public Key Key;
            public Event[][][] Bars;

            public void Draw(Graphics g, float top, float left)
            {
                g.DrawLine(Pens.Black, left, top, left, top + L.StaveHeight);
                TimeSignature.Draw(g, top, L.StaveHeight, left + L.StaveMargin, L.SignatureWidth);
                Clef.Draw(g, top, L.RankSpacing, left + L.StaveMargin, L.SignatureWidth);
                Key.DrawSig(g, top, L.RankSpacing, left + L.StaveMargin, L.SignatureWidth, Clef);
                float StartPosition = left + L.StaveMargin + L.SignatureWidth + L.PostSignatureMargin;
                for (int line = 0; line <= 4; line++)
                    g.DrawLine(Pens.Black, left, top + line * L.RankSpacing * 2, StartPosition + Bars.Count() * L.BarWidth - L.BarLineMargin / 2, top + line * L.RankSpacing * 2);
                for (int index = 0; index < Bars.Length; index++)
                {
                    g.DrawLine(Pens.Black, StartPosition + (index + 1) * L.BarWidth - L.BarLineMargin / 2, top, StartPosition + (index + 1) * L.BarWidth - L.BarLineMargin / 2, top + L.StaveHeight);
                    DrawBar(g, Clef, Bars[index], new RectangleF(StartPosition + index * L.BarWidth + L.BarLineMargin, top, L.BarWidth - L.BarLineMargin, L.StaveHeight), TimeSignature.BarLengthW);
                }
            }


        }

        public struct Event
        {
            public Pitch? Pitch;
            public int WholeDivisionPower;
            public int Dot;
        }

        public Stave[] Staves;

        public Bitmap Draw()
        {
            Bitmap DrawRet = new Bitmap(
                (int)(2 * L.MainMarginX + L.StaveMargin + L.SignatureWidth + L.PostSignatureMargin + Staves.Aggregate(0, (x, y) => Math.Max(x, y.Bars.Count())) * L.BarWidth - L.BarLineMargin / 2),
                (int)(L.MainMarginY * 2 + Staves.Length * (L.StaveHeight + L.StaffSpacing) - L.StaffSpacing));
            using (var g = Graphics.FromImage(DrawRet))
            {
                g.DrawLine(Pens.Black, L.MainMarginX, L.MainMarginY, L.MainMarginX, L.MainMarginY + (Staves.Length - 1) * L.StaveDisplacement + L.StaveHeight);
                for (int index = 0; index < Staves.Length; index++)
                {
                    Staves[index].Draw(g, L.MainMarginY + index * L.StaveDisplacement, L.MainMarginX);
                }
            }

            return DrawRet;
        }

        // must have at least one voice
        private static void DrawBar(Graphics g, Clef clef, Event[][] voices, RectangleF rect, double barWidthW)
        {
            switch (voices.Length)
            {
                case 1:
                    {
                        DrawVoice(g, clef, voices[0], 0, 4, rect.Top, rect.Height, rect.Left, rect.Width, barWidthW);
                        break;
                    }

                case 2:
                    {
                        DrawVoice(g, clef, voices[0], -1, 1, rect.Top, rect.Height, rect.Left, rect.Width, barWidthW);
                        DrawVoice(g, clef, voices[1], 1, 7, rect.Top, rect.Height, rect.Left, rect.Width, barWidthW);
                        break;
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        // use stem direction
        private static void DrawVoice(Graphics g, Clef clef, Event[] events, int stems, int restRankFromTopLine, float top, float staveHeight, float left, float barWidth, double barWidthW)
        {
            var RankSp = staveHeight / 8;
            double timeW = 0d;
            for (int index = 0; index < events.Length; index++)
            {
                float PositionY;
                if (events[index].Pitch.HasValue)
                {
                    int ranknumber = clef.MCRankFromTopLine - events[index].Pitch.Value.IntervalFromC0.Number + 4 * 7;
                    float X1 = left + (float)Math.Round(timeW / barWidthW * barWidth) - RankSp;
                    float X2 = left + (float)Math.Round(timeW / barWidthW * barWidth) + RankSp;
                    for (int ledgerRank = -2; ledgerRank >= ranknumber; ledgerRank -= 2)
                        g.DrawLine(Pens.Black, X1 - 2, top + RankSp * ledgerRank, X2 + 2, top + RankSp * ledgerRank);
                    for (int ledgerRank = 10; ledgerRank <= ranknumber; ledgerRank += 2)
                        g.DrawLine(Pens.Black, X1 - 2, top + RankSp * ledgerRank, X2 + 2, top + RankSp * ledgerRank);
                    PositionY = top + RankSp * ranknumber;
                    g.FillEllipse(Brushes.Black, new RectangleF(X1, PositionY - RankSp, RankSp * 2, RankSp * 2));
                    if (stems == -1 || stems == 0 && ranknumber <= 4)
                    {
                        g.DrawLine(Pens.Black, X2, PositionY, X2, PositionY - L.stemlength);
                    }
                    else
                    {
                        g.DrawLine(Pens.Black, X1, PositionY, X1, PositionY + L.stemlength);
                    }
                }
                else
                {
                    PositionY = top + RankSp * restRankFromTopLine;
                    g.FillEllipse(Brushes.Blue, new RectangleF(left + (float)Math.Round(timeW / barWidthW * barWidth) - RankSp, PositionY - RankSp, RankSp * 2, RankSp * 2));
                }

                switch (events[index].Dot)
                {
                    case 0:
                        {
                            timeW += Math.Pow(2d, -events[index].WholeDivisionPower);
                            break;
                        }

                    case 1:
                        {
                            timeW += Math.Pow(2d, -events[index].WholeDivisionPower) * 1.5d;
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
