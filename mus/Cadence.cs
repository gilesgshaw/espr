namespace mus
{
    public static partial class notation
    {

        //as currently using, fields may be null.
        public class Cadence : Valued
        {
            public Vert Antepenultimate { get; }
            public Vert Penultimate { get; }
            public Vert Ultimate { get; }

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    return tr;
                }
            }

            public Cadence(Vert antepenultimate, Vert penultimate, Vert ultimate) : base()
            {
                Antepenultimate = antepenultimate;
                Penultimate = penultimate;
                Ultimate = ultimate;
            }
        }

    }
}
