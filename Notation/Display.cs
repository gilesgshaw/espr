using System;
using System.Collections.Generic;
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
            var BodyX = L.BarsLeft + Staves.Aggregate(0, (x, y) => Math.Max(x, y.NumberOfBars)) * L.BarWidth;
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

            //factoring this in now.
            public Event[] Events; //code assumes this has bar numbers, voice numbers, and times already set.

            //this needs to be set
            public int NumberOfBars; //and is currently ignored when enumerating bars for voice counting

            public void Draw(Graphics g, PointF offset)
            {

                var Sig__Rect = L.SignatureFromStave;

                var Bars_Rect = new RectangleF(
                    L.BarsLeft,
                    0,
                    NumberOfBars * L.BarWidth,
                    L.StaveHeight);

                var Me___Rect = new RectangleF(
                    0,
                    0,
                    Bars_Rect.Right,
                    Bars_Rect.Bottom);

                Bars_Rect = Bars_Rect.Translate(offset);
                Me___Rect = Me___Rect.Translate(offset);
                Sig__Rect = Sig__Rect.Translate(offset);


                var BarsRects = Bars_Rect.PartitionH(NumberOfBars);
                g.DrawLine(Pens.Black, Me___Rect.X, Me___Rect.Y, Me___Rect.X, Me___Rect.Bottom);
                for (int index = 0; index < NumberOfBars; index++)
                {
                    g.DrawLine(Pens.Black, BarsRects[index].Right, BarsRects[index].Top, BarsRects[index].Right, BarsRects[index].Bottom);
                }

                TimeSignature.Draw(g, Sig__Rect);
                Clef.Draw(g, Sig__Rect);
                Key.DrawSig(g, Sig__Rect, Clef);

                DrawEvents(g, Clef, Events, BarsRects[0], TimeSignature.BarLengthW, L.MarginL, L.MarginR);

                for (int line = 0; line <= Clef.NumSpaces; line++)
                {
                    var prop = (float)line / Clef.NumSpaces;
                    var y = Me___Rect.Top + prop * Me___Rect.Height;
                    g.DrawLine(Pens.Black, Me___Rect.Left, y, Me___Rect.Right, y);
                }

            }

        }

        // probably goes wrong if a bar has no voices, or something like that.
        private static void DrawEvents(Graphics g, Clef clef, IEnumerable<Event> events, RectangleF initialRect, float barWidthW, float MarginL, float MarginR)
        {
            float barWidth = initialRect.Width;
            initialRect.Width -= MarginL + MarginR;
            initialRect.X += MarginL;

            //this is correct, assuming there are no 'empty' bars or voices.
            int NumberOfBars = (int)events.Max((z) => (int?)(z.BarNumber + 1));
            int[] NumberOfVoices = new int[NumberOfBars];
            for (int bn = 0; bn < NumberOfBars; bn++)
            {
                NumberOfVoices[bn] = (int)events.Where((z) => z.BarNumber == bn).Max((z) => (int?)(z.Voice + 1));
            }

            int[][] arrStems = new int[NumberOfBars][];
            int[][] arrRestRankFromTopLine = new int[NumberOfBars][];
            for (int bn = 0; bn < NumberOfBars; bn++)
            {
                switch (NumberOfVoices[bn])
                {
                    case 1:
                        arrStems[bn] = new int[] { 0, 0 };
                        arrRestRankFromTopLine[bn] = new int[] { 4, 4 };
                        break;
                    case 2:
                        arrStems[bn] = new int[] { -1, 1 };
                        arrRestRankFromTopLine[bn] = new int[] { 1, 7 };
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            // use stem direction (not sure what this comment was meant to mean)
            foreach (var Event in events)
            {
                //HERE - need to consider key.
                var Info = new Bar(default, clef, initialRect.Top, initialRect.Height, initialRect.Left + (Event.timeW / barWidthW * initialRect.Width) + Event.BarNumber * barWidth);
                Event.Draw(g, Info, arrStems, arrRestRankFromTopLine);
            }
        }

        public interface Event
        {

            void Draw(Graphics g, Bar Info, int[][] arrStems, int[][] arrRestRankFromTopLine);
            int BarNumber { get; }
            int Voice { get; }
            float timeW { get; }

        }

        //this is a little hacky at the moment.
        //currently drawn below stave, using calibri 12 and 9.
        public struct ChordSymbol : Event
        {
            public float timeW { get; set; }
            public int BarNumber { get; set; }
            public NamedColor? col { get; set; }
            public int Voice { get => -1; }
            public (string, string) Text { get; set; }

            private static Font BigFont = new Font("calibri", 12);
            private static Font SmallFont = new Font("calibri", 9);

            public void Draw(Graphics g, Bar Info, int[][] arrStems, int[][] arrRestRankFromTopLine)
            {
                Brush brush = Brushes.BlueViolet;
                if (col.HasValue) brush = GetBrush(col.Value);
                var sz1 = g.MeasureString(Text.Item1, BigFont);
                var sz2 = g.MeasureString(Text.Item2, SmallFont);
                g.DrawString(Text.Item1, BigFont, brush, 2 + Info.X - sz1.Width / 2 - sz2.Width / 2, Info.Bottom + 34 - sz1.Height / 2);
                g.DrawString(Text.Item2, SmallFont, brush, -2 + Info.X + sz1.Width / 2 - sz2.Width / 2, Info.Bottom + 34 - sz1.Height / 2);
            }
        }

        public struct Note : Event
        {
            public Pitch? Pitch { get; set; }
            public int WholeDivisionPower { get; set; }
            public int Dot { get; set; }
            public float timeW { get; set; } //currently refactoring in
            public int BarNumber { get; set; } //currently refactoring in
            public NamedColor? col { get; set; }
            public int Voice { get; set; } //currently refactoring in

            public void Draw(Graphics g, Bar Info, int[][] arrStems, int[][] arrRestRankFromTopLine)
            {

                int stems = arrStems[BarNumber][Voice];
                int restRankFromTopLine = arrRestRankFromTopLine[BarNumber][Voice];

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

            public readonly static float MarginR = BarLineMargin - MarginL;

            public const float MarginL = 16;
            public const float stemlength = 17;
            public const float MainMarginX = 30;
            public const float MainMarginY = 30;
            public const float StaveMargin = 13;
            public const float SignatureWidth = 80;
            public const float PostSignatureMargin = 9;
            public const float BarLineMargin = 8;
            public const float StaffSpacing = 67;
            public const float StaveHeight = 40;
            public const float BarWidth = 140;
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
