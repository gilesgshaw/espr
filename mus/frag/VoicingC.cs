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

        public VoicingS Simple { get; }
        public Variety Variety { get; }

        public static explicit operator VoicingS(VoicingC obj) => obj.Simple;

        public static explicit operator Variety(VoicingC obj) => obj.Variety;

        public VoicingC(IntervalC s, IntervalC a, IntervalC t, IntervalC b, VoicingS child)
            : base(s, a, t, b, 0)
        {
            Simple = new VoicingS(s.Residue, a.Residue, t.Residue, b.Residue, child.Variety);
            Variety = child.Variety;
            AddChildren(new TreeValued[] { Simple });
        }

        //Bounds are min and max on semis. Non-decreasing.
        public static IEnumerable<VoicingC> FromSimple(VoicingS v, Quad<(int, int)> ranges)
        {
            var bMin = (int)Ceiling(((double)(ranges.B.Item1 - v.B.ResidueSemis)) / 12);
            var tMin = (int)Ceiling(((double)(ranges.T.Item1 - v.T.ResidueSemis)) / 12);
            var aMin = (int)Ceiling(((double)(ranges.A.Item1 - v.A.ResidueSemis)) / 12);
            var sMin = (int)Ceiling(((double)(ranges.S.Item1 - v.S.ResidueSemis)) / 12);

            var bMax = (int)Ceiling(((double)(ranges.B.Item2 + 1 - v.B.ResidueSemis)) / 12);
            var tMax = (int)Ceiling(((double)(ranges.T.Item2 + 1 - v.T.ResidueSemis)) / 12);
            var aMax = (int)Ceiling(((double)(ranges.A.Item2 + 1 - v.A.ResidueSemis)) / 12);
            var sMax = (int)Ceiling(((double)(ranges.S.Item2 + 1 - v.S.ResidueSemis)) / 12);

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

                            yield return new VoicingC(S, A, T, B, v);
                        }
                    }
                }
            }
        }
    }

}
