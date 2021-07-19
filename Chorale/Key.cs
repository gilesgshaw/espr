using static mus.notation;

namespace Chorale
{
    public partial class Display
    {

        public struct Key
        {
            public Key(IntervalS pTonic, Mode pScale)
            {
                Tonic = pTonic;
                Scale = pScale;
            }

            public IntervalS Tonic;
            public Mode Scale;
        }

    }
}
