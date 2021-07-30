using System.Drawing;

namespace Notation
{
    public partial class Display
    {

        private class L
        {
            public readonly static float StaveDisplacement = StaveHeight + StaffSpacing;

            public readonly static RectangleF SignatureFromStave = new RectangleF(StaveMargin, 0, SignatureWidth, StaveHeight);

            public readonly static float BarsLeft = SignatureFromStave.Right + PostSignatureMargin;

            public readonly static float MarginR = BarLineMargin - MarginL;

            public const float MarginL = 16;
            public const float stemlength = 17;
            public const float MainMarginX = 30;
            public const float MainMarginY = 30;
            public const float StaveMargin = 13;
            public const float SignatureWidth = 80;
            public const float PostSignatureMargin = 9;
            public const float BarLineMargin = 8;
            public const float StaffSpacing = 67;
            public const float StaveHeight = 40;
            public const float BarWidth = 140;
        }

    }
}
