using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Notation.Ut;
using static mus.Gen.Ut;

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

    }

}
