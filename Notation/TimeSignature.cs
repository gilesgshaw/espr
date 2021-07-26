using System.Drawing;

namespace Notation
{
    public partial class Display
    {

        public class TimeSignature
        {
            public static TimeSignature Common { get; } = new TimeSignature();

            public float BarLengthW
            {
                get
                {
                    return 1;
                }
            }

            private TimeSignature()
            {
            }

            public void Draw(Graphics g, RectangleF SignatureRect)
            {
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new PointF(SignatureRect.Right - 10, SignatureRect.Top));
                g.DrawString("4", new Font("calibri", 14f), Brushes.Black, new PointF(SignatureRect.Right - 10, SignatureRect.Top + SignatureRect.Height / 2));
            }
        }

    }
}
