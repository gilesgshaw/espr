using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        public class Situation : IEquatable<Situation>
        {

            #region Static

            //not yet implemented tolerence
            public static IEnumerable<Passage> GetPairs(Situation situation, double[] tolerence)
            {
                if (situation.Terminal)
                {
                    foreach (var final in situation.Context.Bank[situation.Sop[1]].Where((x) => x.Chord.Root.ResidueNumber == 0 && x.Voicing.B.ResidueNumber == 0))
                    {
                        foreach (var pp in situation.Context.Bank[situation.Sop[0]].Where((x) => x.Voicing.B.ResidueNumber == 0))
                        {
                            switch (pp.Chord.Root.ResidueNumber)
                            {
                                case 4:
                                    yield return new Passage(new Vert[] { pp, final }, new Passage(new Vert[] { pp }, null, null), new Passage(new Vert[] { final }, null, null));
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var final in situation.Context.Bank[situation.Sop[1]])
                    {
                        foreach (var pp in situation.Context.Bank[situation.Sop[0]])
                        {
                            yield return new Passage(new Vert[] { pp, final }, new Passage(new Vert[] { pp }, null, null), new Passage(new Vert[] { final }, null, null));
                        }
                    }
                }
            }

            //at least 2
            public static Dictionary<Situation, List<Passage>> Cache = new Dictionary<Situation, List<Passage>>();

            //at least 2
            public static IEnumerable<Passage> GetExterenal(Situation situation, double[] tolerence)
            {
                if (situation.Sop.Length == 2)
                {
                    //exactly 2
                    if (!Cache.ContainsKey(situation)) Cache.Add(situation, new List<Passage>(GetPairs(situation, tolerence)));
                }
                else
                {
                    //at least 3
                    AddExterenal(situation, tolerence);
                }
                foreach (var item in Cache[situation])
                {
                    yield return item;
                }
            }

            //at least 3
            //tolerence dosen't matter if already present.
            public static void AddExterenal(Situation situation, double[] tolerence)
            {
                if (Cache.ContainsKey(situation)) return;

                var list = new List<Passage>();
                Cache.Add(situation, list);
                foreach (var l in GetExterenal(situation.Left, tolerence))
                {
                    foreach (var r in GetExterenal(situation.Right, tolerence).Where((x) => x.Left == l.Right))
                    {
                        var obj = new Passage(l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray(), l, r);
                        if (obj.Penalty <= tolerence[obj.Verts.Length]) list.Add(obj);
                    }
                }
            }

            #endregion

            public Context Context;
            public int[] Sop;
            public bool Terminal;

            public Situation(Context context, int[] sop, bool terminal)
            {
                Context = context;
                Sop = sop;
                Terminal = terminal;
            }

            public Situation Left
            {
                get
                {
                    return new Situation(Context, Sop.Take(Sop.Length - 1).ToArray(), false);
                }
            }

            public Situation Right
            {
                get
                {
                    return new Situation(Context, Sop.Skip(1).ToArray(), Terminal);
                }
            }
            
            public override bool Equals(object obj)
            {
                return Equals(obj as Situation);
            }

            public bool Equals(Situation other)
            {
                if (other == null ||
                    !ReferenceEquals(Context, other.Context) ||
                    Terminal != other.Terminal ||
                    Sop.Length != other.Sop.Length) return false;
                for (int i = 0; i < Sop.Length; i++)
                {
                    if (Sop[i] != other.Sop[i]) return false;
                }
                return true;
            }

            //This is VERY RUDAMENTORY
            public override int GetHashCode()
            {
                int hashCode = 280880954;
                hashCode = hashCode * -1521134295 + EqualityComparer<Context>.Default.GetHashCode(Context);
                hashCode = hashCode * -1521134295 + Terminal.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(Situation left, Situation right)
            {
                return EqualityComparer<Situation>.Default.Equals(left, right);
            }

            public static bool operator !=(Situation left, Situation right)
            {
                return !(left == right);
            }
        }

    }
}
