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
            public const int stemlength = 17;
            public const int MainMarginX = 30;
            public const int MainMarginY = 30;
            public const int StaveMargin = 13;
            public const int SignatureWidth = 80;
            public const int PostSignatureMargin = 13;
            public const int BarLineMargin = 8;
            public const int StaffSpacing = 57;
            public const int RankSpacing = 5;
            public const int BarWidth = 85;
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

            public void Draw(Graphics g, int top, int left, int barWidth)
            {
                g.DrawLine(Pens.Black, left, top, left, top + L.RankSpacing * 8);
                TimeSignature.Draw(g, top, L.RankSpacing, left + L.StaveMargin, L.SignatureWidth);
                Clef.Draw(g, top, L.RankSpacing, left + L.StaveMargin, L.SignatureWidth);
                Key.DrawSig(g, top, L.RankSpacing, left + L.StaveMargin, L.SignatureWidth, Clef);
                int StartPosition = left + L.StaveMargin + L.SignatureWidth + L.PostSignatureMargin;
                for (int line = 0; line <= 4; line++)
                    g.DrawLine(Pens.Black, left, top + line * L.RankSpacing * 2, StartPosition + Bars.Count() * barWidth - L.BarLineMargin / 2, top + line * L.RankSpacing * 2);
                for (int index = 0, loopTo = Bars.Length - 1; index <= loopTo; index++)
                {
                    g.DrawLine(Pens.Black, StartPosition + (index + 1) * barWidth - L.BarLineMargin / 2, top, StartPosition + (index + 1) * barWidth - L.BarLineMargin / 2, top + L.RankSpacing * 8);
                    DrawBar(g, Clef, Bars[index], top, L.RankSpacing, StartPosition + index * barWidth + L.BarLineMargin, barWidth - L.BarLineMargin, TimeSignature.BarLengthW);
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
            Bitmap DrawRet = default;
            DrawRet = new Bitmap(
                2 * L.MainMarginX + L.StaveMargin + L.SignatureWidth + L.PostSignatureMargin + Staves.Aggregate(0, (x, y) => Math.Max(x, y.Bars.Count())) * L.BarWidth - L.BarLineMargin / 2,
                L.MainMarginY * 2 + Staves.Length * (8 * L.RankSpacing + L.StaffSpacing) - L.StaffSpacing);
            using (var g = Graphics.FromImage(DrawRet))
            {
                for (int index = 0; index < Staves.Length; index++)
                {
                    Staves[index].Draw(g, L.MainMarginY + index * (8 * L.RankSpacing + L.StaffSpacing), L.MainMarginX, L.BarWidth);
                }
            }

            return DrawRet;
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
                    int ranknumber = clef.MCRankFromTopLine - voice[index].Pitch.Value.IntervalFromC0.Number + 4 * 7;
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
                        g.DrawLine(Pens.Black, X2, PositionY, X2, PositionY - L.stemlength);
                    }
                    else
                    {
                        g.DrawLine(Pens.Black, X1, PositionY, X1, PositionY + L.stemlength);
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
                        }
                }
            }
        }

    }

}
