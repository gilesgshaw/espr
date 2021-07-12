namespace mus
{
    public static partial class notation
    {

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

            public Cadence(Vert antepenultimate, Vert penultimate, Vert ultimate) : base(new Valued[] { antepenultimate, penultimate, ultimate })
            {
                Antepenultimate = antepenultimate;
                Penultimate = penultimate;
                Ultimate = ultimate;
            }
        }

    }
}
