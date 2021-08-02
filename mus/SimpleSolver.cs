using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        public static class SimpleSolver
        {

            //at least 1
            public static Dictionary<Situation, List<Passage>> Cache = new Dictionary<Situation, List<Passage>>();

            //at least 1
            public static IEnumerable<Passage> GetExterenal(Situation situation, double[] tolerence, int[] maxes)
            {
                if (!Cache.ContainsKey(situation))
                {
                    AddInternal(situation, tolerence, maxes);
                }
                return Cache[situation];
            }

            //at least 1
            //tolerence etc... only applies to child routines
            private static IEnumerable<Passage> GetInternal(Situation situation, double[] tolerence, int[] maxes)
            {
                if (situation.Sop.Length == 1)
                {
                    //exactly 1
                    //could check if this is a final chord (i.e. should be I or V)
                    foreach (var item in situation.Context.Bank[situation.Sop[0]])
                    {
                        // currently enforcing that opening chord is I(a)
                        if (situation.Initial && (item.Chord.Root.ResidueNumber != 0 || item.Voicing.B.ResidueNumber != 0)) continue;
                        yield return new Passage(situation.Context.Tonic, new Vert[] { item }, null, null);
                    }
                }
                else if (situation.Sop.Length == 2)
                {
                    //exactly 2

                    var BankR = GetExterenal(situation.Right, tolerence, maxes);
                    var BankL = GetExterenal(situation.Left, tolerence, maxes);
                    // querey these now to make sure they are actually run, in case the full thing fails
                    // maybe it would be faster to properly evaluate them now.

                    if (situation.Displacement == 0) //check if this is a cadence
                    {
                        //   perfect V-I
                        foreach (var final in BankR.Where((x) => x.Verts[0].Chord.Root.ResidueNumber == 0 && x.Verts[0].Voicing.B.ResidueNumber == 0))
                        {
                            foreach (var pp in BankL.Where((x) => x.Verts[0].Chord.Root.ResidueNumber == 4 && x.Verts[0].Voicing.B.ResidueNumber == 0))
                            {
                                yield return new Passage(
                                    situation.Context.Tonic,
                                    new Vert[] { pp.Verts[0], final.Verts[0] },
                                    pp,
                                    final
                                    );
                            }
                        }
                        // imperfect ?-V
                        foreach (var final in BankR.Where((x) => x.Verts[0].Chord.Root.ResidueNumber == 4 && x.Verts[0].Voicing.B.ResidueNumber == 0
                        && !x.Verts[0].Chord.Variety.PQ7.HasValue))
                        {
                            foreach (var pp in BankL)
                            {
                                yield return new Passage(
                                    situation.Context.Tonic,
                                    new Vert[] { pp.Verts[0], final.Verts[0] },
                                    pp,
                                    final
                                    );
                            }
                        }
                    }
                    else
                    {
                        // anything goes.
                        foreach (var final in BankR)
                        {
                            foreach (var pp in BankL)
                            {
                                yield return new Passage(
                                    situation.Context.Tonic,
                                    new Vert[] { pp.Verts[0], final.Verts[0] },
                                    pp,
                                    final
                                    );
                            }
                        }
                    }
                }
                else
                {
                    //at least 3

                    var BankR = GetExterenal(situation.Right, tolerence, maxes);
                    var BankL = GetExterenal(situation.Left, tolerence, maxes);
                    // see comments above

                    foreach (var l in BankL)
                    {
                        foreach (var r in BankR.Where((x) => ReferenceEquals(x.Left, l.Right)))
                        {
                            yield return new Passage(l.Tonic, l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray(), l, r);
                        }
                    }
                }
            }

            //at least 1
            private static void AddInternal(Situation situation, double[] tolerence, int[] maxes)
            {
                var fullList = GetInternal(situation, tolerence, maxes).Where((obj) => obj.Penalty <= tolerence[obj.Verts.Length]).ToArray();
                Array.Sort(fullList, Valuer<Passage>.instance);
                Cache.Add(situation, new List<Passage>(fullList.Take(maxes[situation.Sop.Length])));
            }

        }

    }
}
