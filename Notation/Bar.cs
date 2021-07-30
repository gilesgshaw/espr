namespace Notation
{
    public partial class Display
    {

        //private??
        public struct Bar
        {
            public Key Sig { get; }
            public Clef Clef { get; }
            public float Top { get; }
            public float Bottom { get => Top + Height; }
            public float Height { get; }
            public float X { get; }

            public Bar(Key sig, Clef clef, float top, float height, float x)
            {
                Sig = sig;
                Clef = clef;
                Top = top;
                Height = height;
                X = x;
            }
        }

    }
}
