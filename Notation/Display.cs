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

    }

}
