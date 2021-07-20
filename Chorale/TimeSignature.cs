using System.Drawing;

namespace Chorale
{
    public partial class Display
    {

        public class TimeSignature
        {
            public static TimeSignature Common { get; } = new TimeSignature();

            public double BarLengthW
            {
                get
                {
                    return 1d;
                }
            }

            private TimeSignature()
            {
            }

            public void Draw(Graphics g, float topLineY, float staveHeight, float startX, float widthX)
            {
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new PointF(widthX + startX - 10, topLineY));
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new PointF(widthX + startX - 10, topLineY + staveHeight / 2));
            }
        }

    }
}
