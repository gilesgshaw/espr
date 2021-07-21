using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chorale
{
    public static class Ut
    {
        
        public static void DrawCenteredEllipse(this Graphics g, Pen pen, Brush bsh, bool solid, PointF pos, SizeF size)
        {
            if (solid)
            {
                g.FillEllipse(bsh, new RectangleF(pos.X - size.Width / 2, pos.Y - size.Height / 2, size.Width, size.Height));
            }
            else
            {
                g.DrawEllipse(pen, new RectangleF(pos.X - size.Width / 2, pos.Y - size.Height / 2, size.Width, size.Height));
            }
        }

        public static RectangleF[] PartitionH(this RectangleF Parent, int number)
        {
            var tr = new RectangleF[number];
            for (int i = 0; i < number; i++)
            {
                var prop = (float)i / number;
                tr[i] = new RectangleF(Parent.Left + prop * Parent.Width, Parent.Top, Parent.Width / number, Parent.Height);
            }
            return tr;
        }
        
        public static RectangleF[] PartitionV(this RectangleF Parent, int number)
        {
            var tr = new RectangleF[number];
            for (int i = 0; i < number; i++)
            {
                var prop = (float)i / number;
                tr[i] = new RectangleF(Parent.Left, Parent.Top + prop * Parent.Height, Parent.Width, Parent.Height / number);
            }
            return tr;
        }

        public static RectangleF Translate(this RectangleF rect, PointF offset)
        {
            return new RectangleF(rect.Location + new SizeF(offset), rect.Size);
        }

    }
}
