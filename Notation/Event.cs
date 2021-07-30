using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static mus.notation;
using static Notation.Ut;

namespace Notation
{
    public partial class Display
    {

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
                    int ranknumber = (Info.Clef.MCRankFromTopLine - Pitch.Value.FromMC.Number);

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

    }
}
