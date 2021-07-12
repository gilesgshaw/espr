namespace mus
{
    public static partial class notation
    {

        public class Cadence
        {
            public Vert Antepenultimate { get; }
            public Vert Penultimate { get; }
            public Vert Ultimate { get; }

            public Cadence(Vert antepenultimate, Vert penultimate, Vert ultimate)
            {
                Antepenultimate = antepenultimate;
                Penultimate = penultimate;
                Ultimate = ultimate;
            }
        }

    }
}
