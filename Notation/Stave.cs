using System.Drawing;

namespace Notation
{

    public partial class Display
    {
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

    }

}
