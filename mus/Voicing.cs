﻿using System.Collections.Generic;
using static System.Math;

namespace mus
{
    public static partial class notation
    {

        public class VoicingS : Valued
        {
            public IntervalS S { get; }
            public IntervalS A { get; }
            public IntervalS T { get; }
            public IntervalS B { get; }

            public static VoicingS operator +(VoicingS a, IntervalS b)
            {
                return new VoicingS(a.S + b, a.A + b, a.T + b, a.B + b);
            }

            public static VoicingS operator +(IntervalS b, VoicingS a)
            {
                return a + b;
            }

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    return tr;
                }
            }

            public VoicingS(IntervalS s, IntervalS a, IntervalS t, IntervalS b) : base()
            {
                S = s;
                A = a;
                T = t;
                B = b;
            }

            public override string ToString()
            {
                return B.ToString() + "; " + T.ToString() + "; " + A.ToString() + "; " + S.ToString();
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
                                    yield return new VoicingS((IntervalS)c.Interval(s), (IntervalS)c.Interval(a), (IntervalS)c.Interval(t), (IntervalS)c.Interval(b));
                                }

                                profile[s]--;
                            }
                            profile[a]--;
                        }
                        profile[t]--;
                    }
                    profile[b]--;
                }

            } // IEnumerable<VoicingS> FromVariety(Variety c)
        }

        public class VoicingC : Valued
        {
            public IntervalC S { get; }
            public IntervalC A { get; }
            public IntervalC T { get; }
            public IntervalC B { get; }

            public static VoicingC operator +(VoicingC a, IntervalC b)
            {
                return new VoicingC(a.S + b, a.A + b, a.T + b, a.B + b);
            }

            public static VoicingC operator +(IntervalC b, VoicingC a)
            {
                return a + b;
            }

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    if (B.ResidueNumber != 0 && B.ResidueNumber != 2) tr += 100;
                    int[] profile = new int[7];
                    profile[B.ResidueNumber]++;
                    profile[T.ResidueNumber]++;
                    profile[A.ResidueNumber]++;
                    profile[S.ResidueNumber]++;
                    if (profile[2] >= 2) tr += 50;
                    if (profile[4] >= 2) tr += 20;
                    if (0 < T.Semis - B.Semis && T.Semis - B.Semis < 10) tr += 10 - (T.Semis - B.Semis);
                    return tr;
                }
            }

            public VoicingC(IntervalC s, IntervalC a, IntervalC t, IntervalC b) : base(new Valued[] { new VoicingS(s.Residue, a.Residue, t.Residue, b.Residue) })
            {
                S = s;
                A = a;
                T = t;
                B = b;
            }

            public override string ToString()
            {
                return B.ToString() + "; " + T.ToString() + "; " + A.ToString() + "; " + S.ToString();
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

                                yield return new VoicingC(S, A, T, B);
                            }
                        }
                    }
                }
            }
        }

    }
}
