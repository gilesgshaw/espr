using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;
using static Notation.Ut;
using static mus.Gen.Ut;

namespace Notation
{
    public partial class Display
    {

        public class Stave
        {
            public Clef Clef;
            public TimeSignature TimeSignature;
            public Key Key;
            public Event[] Events;
            public int NumberOfBars; // this is currently ignored when enumerating bars for voice counting

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

                var iEvents = new List<Event>(Events);
                for (int i = 0; i < NumberOfBars; i++)
                {
                    iEvents.Add(new AccReset(i));
                }
                iEvents.Sort(EventComparer.the);

                DrawInternalEvents(g, iEvents, BarsRects[0], TimeSignature.BarLengthW, L.MarginL, L.MarginR);

                for (int line = 0; line <= Clef.NumSpaces; line++)
                {
                    var prop = (float)line / Clef.NumSpaces;
                    var y = Me___Rect.Top + prop * Me___Rect.Height;
                    g.DrawLine(Pens.Black, Me___Rect.Left, y, Me___Rect.Right, y);
                }

            }

            //currently only considers time and type of event.
            private class EventComparer : IComparer<Event>
            {
                public int Compare(Event x, Event y)
                {
                    int result = x.BarNumber.CompareTo(y.BarNumber);
                    if (result != 0) return result;
                    result = x.timeW.CompareTo(y.timeW);
                    if (result != 0) return result;
                    result = Priority(x).CompareTo(Priority(y));
                    if (result != 0) return result;
                    return 0;
                }

                private EventComparer() { }
                public static readonly EventComparer the = new EventComparer();

                private static int Priority(Event x)
                {
                    if (x is AccReset) return 1;
                    if (x is Note) return 2;
                    if (x is ChordSymbol) return 3;
                    throw new NotImplementedException();
                }
            }

            // probably goes wrong if a bar has no voices, or something like that.
            private void DrawInternalEvents(Graphics g, IEnumerable<Event> iEvents, RectangleF initialRect, float barWidthW, float MarginL, float MarginR)
            {
                float barWidth = initialRect.Width;
                initialRect.Width -= MarginL + MarginR;
                initialRect.X += MarginL;

                //this is correct, assuming there are no 'empty' bars or voices.
                int NumberOfBars = (int)iEvents.Max((z) => (int?)(z.BarNumber + 1));
                int[] NumberOfVoices = new int[NumberOfBars];
                for (int bn = 0; bn < NumberOfBars; bn++)
                {
                    NumberOfVoices[bn] = (int)iEvents.Where((z) => z.BarNumber == bn).Max((z) => (int?)(z.Voice + 1));
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
                var Info = new Bar(Key, Clef, initialRect.Top, initialRect.Height, initialRect.Left);
                foreach (var Event in iEvents)
                {
                    Info.AdvanceTo(initialRect.Left + (Event.timeW / barWidthW * initialRect.Width) + Event.BarNumber * barWidth);
                    Event.Draw(g, Info, arrStems, arrRestRankFromTopLine);
                }
            }
        }

    }
}
