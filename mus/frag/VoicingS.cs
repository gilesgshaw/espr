using System.Collections.Generic;
using mus.Gen;

namespace mus.Chorale
{

    //counts from root
    //constructor trusts the information given.
    // immutable, provided 'Variety' is
    public class VoicingS : VQuad<IntervalS>
    {
        public Variety Variety { get; }

        public override double IntrinsicPenalty
        {
            get
            {
                var tr = base.IntrinsicPenalty;

                //inversion
                if (B.ResidueNumber != 0 && B.ResidueNumber != 2) tr += 100;
                if (Variety.PQ5 == -1 && B.ResidueNumber == 0) tr += 70;

                //doubling
                int[] profile = new int[7];
                profile[B.ResidueNumber]++;
                profile[T.ResidueNumber]++;
                profile[A.ResidueNumber]++;
                profile[S.ResidueNumber]++;
                if (profile[2] >= 2) tr += 30;
                if (profile[4] >= 2) tr += 7;

                return tr;
            }
        }

        public static explicit operator Variety(VoicingS obj) => obj.Variety;

        public VoicingS(IntervalS s, IntervalS a, IntervalS t, IntervalS b, Variety variety) : base(s, a, t, b, 0)
        {
            Variety = variety;
        }

        public static IEnumerable<VoicingS> FromVariety(Variety c)
        {

            int[] profile = new int[7];

            for (int b = 0; b < 7; b++)
            {
                if (!c.PQualByOffset(b).HasValue) continue;
                profile[b]++;

                for (int t = 0; t < 7; t++)
                {
                    if (!c.PQualByOffset(t).HasValue) continue;
                    profile[t]++;

                    for (int a = 0; a < 7; a++)
                    {
                        if (!c.PQualByOffset(a).HasValue) continue;
                        profile[a]++;

                        for (int s = 0; s < 7; s++)
                        {
                            if (!c.PQualByOffset(s).HasValue) continue;
                            profile[s]++;

                            bool complete = true;
                            for (int i = 0; i < 7; i++)
                            {
                                if (c.PQualByOffset(i).HasValue && profile[i] == 0) complete = false;
                            }

                            if (complete)
                            {
                                yield return new VoicingS((IntervalS)c.PIntervalS(s), (IntervalS)c.PIntervalS(a), (IntervalS)c.PIntervalS(t), (IntervalS)c.PIntervalS(b), c);
                            }

                            profile[s]--;
                        }
                        profile[a]--;
                    }
                    profile[t]--;
                }
                profile[b]--;
            }

        }
    }

}
