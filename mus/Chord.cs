using System.Collections.Generic;

namespace mus
{
    public static partial class notation
    {

        public class Chord : Valued
        {
            public IntervalS Root { get; }
            public Variety Variety { get; }

            public Chord(IntervalS root, Variety variety) : base(new Valued[] { })
            {
                Root = root;
                Variety = variety;
            }

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    return tr;
                }
            }

            //returns relative voicings
            public IEnumerable<VoicingC> Instances((int, int) bRange, (int, int) tRange, (int, int) aRange, (int, int) sRange)
            {
                foreach (var v in VoicingS.FromVariety(Variety))
                {
                    foreach (var V in VoicingC.FromSimple(v,
                        (bRange.Item1 - Root.ResidueSemis, bRange.Item2 - Root.ResidueSemis),
                        (tRange.Item1 - Root.ResidueSemis, tRange.Item2 - Root.ResidueSemis),
                        (aRange.Item1 - Root.ResidueSemis, aRange.Item2 - Root.ResidueSemis),
                        (sRange.Item1 - Root.ResidueSemis, sRange.Item2 - Root.ResidueSemis)))
                    {
                        yield return V;
                    }
                }
            }
        }

    }
}
