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
            public int NumLines { get; }

            public int NumSpaces { get => NumLines - 1; }

            public void Draw(Graphics g, float topLineY, float staveHeight, float startX, float widthX)
            {
                switch (MCRankFromTopLine)
                {
                    case 10:
                        g.DrawString("g", new Font("calibri", 14f), Brushes.Black, new PointF(startX, topLineY + (staveHeight / 2 / NumSpaces) * 6 - 15));
                        break;
                    case -2:
                        g.DrawString("f", new Font("calibri", 14f), Brushes.Black, new PointF(startX, topLineY + (staveHeight / 2 / NumSpaces) * 2 - 10));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            public Clef(int mCRankFromTopLine, int numLines, string name)
            {
                MCRankFromTopLine = mCRankFromTopLine;
                Name = name;
                NumLines = numLines;
            }

            public override string ToString()
            {
                return Name;
            }
        }

    }
}
