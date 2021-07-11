using System.Collections.Generic;

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
        }

    }
}
