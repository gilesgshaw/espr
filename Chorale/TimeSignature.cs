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

            public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
            {
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY));
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new Point(widthX + startX - 10, topLineY + rankHeightY * 4));
            }
        }

    }
}
