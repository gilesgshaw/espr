using System;
using System.Drawing;
using System.Linq;
using static mus.notation;
using static Notation.Ut;

namespace Notation
{

    // what supported, no key signatures, no ledger lines, all as dots with no accidentals, or squares
    // update for degree
    // accidentals and key signature agreement

    public partial class Display
    {
        public Stave[] Staves;

        public Bitmap Draw()
        {
            var BodyX = L.BarsLeft + Staves.Aggregate(0, (x, y) => Math.Max(x, y.Bars.Count())) * L.BarWidth;
            var BodyY = L.StaveHeight + (Staves.Length - 1) * L.StaveDisplacement;
            Bitmap DrawRet = new Bitmap(
                (int)(L.MainMarginX + BodyX + L.MainMarginX),
                (int)(L.MainMarginY + BodyY + L.MainMarginY));
            using (var g = Graphics.FromImage(DrawRet))
            {
                g.DrawLine(Pens.Black, L.MainMarginX, L.MainMarginY, L.MainMarginX, L.MainMarginY + BodyY);
                for (int index = 0; index < Staves.Length; index++)
                {
                    Staves[index].Draw(g, new PointF(L.MainMarginX, L.MainMarginY + index * L.StaveDisplacement));
                }
            }
            return DrawRet;
        }

        public class Stave
        {

            public Clef Clef;
            public TimeSignature TimeSignature;
            public Key Key;
            public Event[][][] Bars;

            public void Draw(Graphics g, PointF offset)
            {
                var Sig__Rect = L.SignatureFromStave;

                var Bars_Rect = new RectangleF(
                    L.BarsLeft,
                    0,
                    Bars.Length * L.BarWidth,
                    L.StaveHeight);

                var Me___Rect = new RectangleF(
                    0,
                    0,
                    Bars_Rect.Right,
                    Bars_Rect.Bottom);

                Bars_Rect = Bars_Rect.Translate(offset);
                Me___Rect = Me___Rect.Translate(offset);
                Sig__Rect = Sig__Rect.Translate(offset);


                var BarsRects = Bars_Rect.PartitionH(Bars.Length);
                g.DrawLine(Pens.Black, Me___Rect.X, Me___Rect.Y, Me___Rect.X, Me___Rect.Bottom);
                for (int index = 0; index < Bars.Length; index++)
                {
                    g.DrawLine(Pens.Black, BarsRects[index].Right, BarsRects[index].Top, BarsRects[index].Right, BarsRects[index].Bottom);
                }

                TimeSignature.Draw(g, Sig__Rect);
                Clef.Draw(g, Sig__Rect);
                Key.DrawSig(g, Sig__Rect, Clef);

                for (int index = 0; index < Bars.Length; index++)
                {
                    DrawBar(g, Clef, Bars[index], BarsRects[index], TimeSignature.BarLengthW, L.MarginL, L.MarginR);
                }

                for (int line = 0; line <= Clef.NumSpaces; line++)
                {
                    var prop = (float)line / Clef.NumSpaces;
                    var y = Me___Rect.Top + prop * Me___Rect.Height;
                    g.DrawLine(Pens.Black, Me___Rect.Left, y, Me___Rect.Right, y);
                }
            }

        }

        // must have at least one voice
        private static void DrawBar(Graphics g, Clef clef, Event[][] voices, RectangleF rect, float barWidthW, float MarginL, float MarginR)
        {
            rect.Width -= MarginL + MarginR;
            rect.X += MarginL;
            switch (voices.Length)
            {
                case 1:
                    DrawVoice(g, clef, voices[0], 0, 4, rect, barWidthW);
                    break;
                case 2:
                    DrawVoice(g, clef, voices[1], 1, 7, rect, barWidthW);
                    DrawVoice(g, clef, voices[0], -1, 1, rect, barWidthW);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // use stem direction
        private static void DrawVoice(Graphics g, Clef clef, Event[] events, int stems, int restRankFromTopLine, RectangleF rect, float barWidthW)
        {

            float timeWh = 0;
            for (int i = 0; i < events.Length; i++)
            {
                events[i].timeW = timeWh;
                timeWh += (float)Math.Pow(2, -events[i].WholeDivisionPower) * (2 - (float)Math.Pow(2, -events[i].Dot));
            }

            foreach (Event Event in events)
            {
                //HERE - need to consider key.
                var Info = new Bar(default, clef, rect.Top, rect.Height, rect.Left + (Event.timeW / barWidthW * rect.Width));
                Event.Draw(g, Info, stems, restRankFromTopLine);
            }

        }

        public struct Event
        {
            public Pitch? Pitch;
            public int WholeDivisionPower;
            public int Dot;
            public float timeW; //temporary
            public NamedColor? col;

            public void Draw(Graphics g, Bar Info, int stems, int restRankFromTopLine)
            {

                var RankSp = Info.Height / Info.Clef.NumSpaces / 2;

                Pen pen;
                Brush brush;
                float PositionY;

                if (Pitch.HasValue)
                {
                    int ranknumber = (Info.Clef.MCRankFromTopLine - Pitch.Value.IntervalFromC0.Number + 4 * 7);

                    PositionY = Info.Top + RankSp * ranknumber;
                    pen = Pens.Black;
                    brush = Brushes.Black;
                    if (col.HasValue)
                    {
                        pen = GetPen(col.Value);
                        brush = GetBrush(col.Value);
                    }

                    if (stems == -1 || stems == 0 && ranknumber <= 4) //up
                    {
                        g.DrawLine(pen, Info.X + RankSp, PositionY, Info.X + RankSp, PositionY - L.stemlength);
                    }
                    else                                              //down
                    {
                        g.DrawLine(pen, Info.X - RankSp, PositionY, Info.X - RankSp, PositionY + L.stemlength);
                    }

                    for (int ledgerRank = -2; ledgerRank >= ranknumber; ledgerRank -= 2)
                        g.DrawLine(Pens.Black, Info.X - RankSp - 2, Info.Top + RankSp * ledgerRank, Info.X + RankSp + 2, Info.Top + RankSp * ledgerRank);
                    for (int ledgerRank = 10; ledgerRank <= ranknumber; ledgerRank += 2)
                        g.DrawLine(Pens.Black, Info.X - RankSp - 2, Info.Top + RankSp * ledgerRank, Info.X + RankSp + 2, Info.Top + RankSp * ledgerRank);
                }
                else
                {

                    PositionY = Info.Top + RankSp * restRankFromTopLine;
                    pen = Pens.Blue;
                    brush = Brushes.Blue;
                    if (col.HasValue)
                    {
                        pen = GetPen(col.Value);
                        brush = GetBrush(col.Value);
                    }

                }

                bool solid;
                switch (WholeDivisionPower) { case 2: solid = true; break; case 1: solid = false; break; default: throw new NotImplementedException(); }
                g.DrawCenteredEllipse(pen, brush, solid, new PointF(Info.X, PositionY), new SizeF(RankSp * 2, RankSp * 2));
            }
        }

        private class L
        {
            public readonly static float StaveDisplacement = StaveHeight + StaffSpacing;

            public readonly static RectangleF SignatureFromStave = new RectangleF(StaveMargin, 0, SignatureWidth, StaveHeight);

            public readonly static float BarsLeft = SignatureFromStave.Right + PostSignatureMargin;

            public readonly static float MarginL = BarLineMargin * 1.5f;
            public readonly static float MarginR = BarLineMargin * -0.5f;

            public const float stemlength = 17;
            public const float MainMarginX = 30;
            public const float MainMarginY = 30;
            public const float StaveMargin = 13;
            public const float SignatureWidth = 80;
            public const float PostSignatureMargin = 9;
            public const float BarLineMargin = 8;
            public const float StaffSpacing = 57;
            public const float StaveHeight = 40;
            public const float BarWidth = 85;
        }

        //private??
        public struct Bar
        {
            public Key Sig { get; }
            public Clef Clef { get; }
            public float Top { get; }
            public float Bottom { get => Top + Height; }
            public float Height { get; }
            public float X { get; }

            public Bar(Key sig, Clef clef, float top, float height, float x)
            {
                Sig = sig;
                Clef = clef;
                Top = top;
                Height = height;
                X = x;
            }
        }
    }

}
