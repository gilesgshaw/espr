using System;
using System.Drawing;

namespace Chorale
{
    public partial class Display
    {

        public class Clef
        {
            public int MCRankFromTopLine { get; }
            public string Name { get; }

            public void Draw(Graphics g, int topLineY, int rankHeightY, int startX, int widthX)
            {
                switch (MCRankFromTopLine)
                {
                    case 10:
                        g.DrawString("g", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 6 - 15));
                        break;
                    case -2:
                        g.DrawString("f", new Font("calibri", 14f), Brushes.Black, new Point(startX, topLineY + rankHeightY * 2 - 10));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            public Clef(int mCRankFromTopLine, string name)
            {
                MCRankFromTopLine = mCRankFromTopLine;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

    }
}
