using System.Collections.Generic;
using static System.Math;

namespace mus
{
    public static partial class notation
    {

        public struct VoicingS
        {
            public IntervalS S { get; set; }
            public IntervalS A { get; set; }
            public IntervalS T { get; set; }
            public IntervalS B { get; set; }

            public static VoicingS operator +(VoicingS a, IntervalS b)
            {
                return new VoicingS(a.S + b, a.A + b, a.T + b, a.B + b);
            }

            public static VoicingS operator +(IntervalS b, VoicingS a)
            {
                return a + b;
            }

            public VoicingS(IntervalS s, IntervalS a, IntervalS t, IntervalS b)
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

            public static IEnumerable<VoicingS> FromCharacter(Character c)
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

            } // IEnumerable<VoicingS> FromCharacter(Character c)
        }

        public struct VoicingC
        {
            public IntervalC S { get; set; }
            public IntervalC A { get; set; }
            public IntervalC T { get; set; }
            public IntervalC B { get; set; }

            public static VoicingC operator +(VoicingC a, IntervalC b)
            {
                return new VoicingC(a.S + b, a.A + b, a.T + b, a.B + b);
            }

            public static VoicingC operator +(IntervalC b, VoicingC a)
            {
                return a + b;
            }

            public VoicingC(IntervalC s, IntervalC a, IntervalC t, IntervalC b)
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

            //refers to MIDI pitches, as if relative to C0. Non-decreasing. Min and max.
            public static IEnumerable<VoicingC> FromSimple(VoicingS v, (int, int) bRange, (int, int) tRange, (int, int) aRange, (int, int) sRange)
            {
                var bGround = new Pitch(new IntervalC(v.B.ResidueNumber, v.B.Quality, 0));
                var tGround = new Pitch(new IntervalC(v.T.ResidueNumber, v.T.Quality, 0));
                var aGround = new Pitch(new IntervalC(v.A.ResidueNumber, v.A.Quality, 0));
                var sGround = new Pitch(new IntervalC(v.S.ResidueNumber, v.S.Quality, 0));

                var bMin = (int)Ceiling(((double)(bRange.Item1 - bGround.MIDIPitch)) / 12);
                var tMin = (int)Ceiling(((double)(tRange.Item1 - tGround.MIDIPitch)) / 12);
                var aMin = (int)Ceiling(((double)(aRange.Item1 - aGround.MIDIPitch)) / 12);
                var sMin = (int)Ceiling(((double)(sRange.Item1 - sGround.MIDIPitch)) / 12);

                var bMax = (int)Ceiling(((double)(bRange.Item2 + 1 - bGround.MIDIPitch)) / 12);
                var tMax = (int)Ceiling(((double)(tRange.Item2 + 1 - tGround.MIDIPitch)) / 12);
                var aMax = (int)Ceiling(((double)(aRange.Item2 + 1 - aGround.MIDIPitch)) / 12);
                var sMax = (int)Ceiling(((double)(sRange.Item2 + 1 - sGround.MIDIPitch)) / 12);

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
