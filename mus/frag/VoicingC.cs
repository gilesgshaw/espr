using System.Collections.Generic;
using static System.Math;
using mus.Gen;

namespace mus.Chorale
{

    //counts from root
    //constructor trusts the information given.
    // immutable
    public class VoicingC : VQuad<IntervalC>
    {
        public override double IntrinsicPenalty
        {
            get
            {
                var tr = base.IntrinsicPenalty;

                //spacing
                var Spacing = new int[] { T.Semis - B.Semis, A.Semis - T.Semis, S.Semis - A.Semis };
                var Optimal = new int[] { 11, 6, 4 };
                var ZeroVal = new double[] { 6, 3, 1.5 };
                for (int i = 0; i < 3; i++)
                {
                    var spacing = Spacing[i];
                    var optimal = Optimal[i];
                    var zeroVal = ZeroVal[i];
                    if (spacing == 0)
                    {
                        tr += zeroVal;
                    }
                    else
                    {
                        tr += Abs(optimal - spacing);
                    }
                }

                return tr;
            }
        }

        public VoicingC(IntervalC s, IntervalC a, IntervalC t, IntervalC b, Variety variety) : base(
            s, a, t, b, new TreeValued[] { new VoicingS(s.Residue, a.Residue, t.Residue, b.Residue, variety) })
        {
        }

        //Bounds are min and max on semis. Non-decreasing.
        public static IEnumerable<VoicingC> FromSimple(VoicingS v, (int, int) bRange, (int, int) tRange, (int, int) aRange, (int, int) sRange)
        {
            var bMin = (int)Ceiling(((double)(bRange.Item1 - v.B.ResidueSemis)) / 12);
            var tMin = (int)Ceiling(((double)(tRange.Item1 - v.T.ResidueSemis)) / 12);
            var aMin = (int)Ceiling(((double)(aRange.Item1 - v.A.ResidueSemis)) / 12);
            var sMin = (int)Ceiling(((double)(sRange.Item1 - v.S.ResidueSemis)) / 12);

            var bMax = (int)Ceiling(((double)(bRange.Item2 + 1 - v.B.ResidueSemis)) / 12);
            var tMax = (int)Ceiling(((double)(tRange.Item2 + 1 - v.T.ResidueSemis)) / 12);
            var aMax = (int)Ceiling(((double)(aRange.Item2 + 1 - v.A.ResidueSemis)) / 12);
            var sMax = (int)Ceiling(((double)(sRange.Item2 + 1 - v.S.ResidueSemis)) / 12);

            for (int b = bMin; b < bMax; b++)
            {
                var B = new IntervalC(v.B.ResidueNumber, v.B.Quality, b);

                for (int t = tMin; t < tMax; t++)
                {
                    var T = new IntervalC(v.T.ResidueNumber, v.T.Quality, t);
                    if (T < B) continue;

                    for (int a = aMin; a < aMax; a++)
                    {
                        var A = new IntervalC(v.A.ResidueNumber, v.A.Quality, a);
                        if (A < T) continue;

                        for (int s = sMin; s < sMax; s++)
                        {
                            var S = new IntervalC(v.S.ResidueNumber, v.S.Quality, s);
                            if (S < A) continue;

                            yield return new VoicingC(S, A, T, B, v.Variety);
                        }
                    }
                }
            }
        }
    }

}
