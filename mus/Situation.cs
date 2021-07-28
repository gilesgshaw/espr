using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        //should make this properly immutable
        public class Situation : IEquatable<Situation>
        {


            #region Static

            //not yet implemented tolerence
            public static IEnumerable<Passage> GetSingletons(Situation situation, double[] tolerence)
            {
                return Array.ConvertAll(situation.Context.Bank[situation.Sop[0]], (x) => new Passage(situation.Context.Tonic, new Vert[] { x }, null, null));
            }

            //not yet implemented tolerence
            public static IEnumerable<Passage> GetPairs(Situation situation, double[] tolerence)
            {
                if (situation.Displacement == 0)
                {
                    foreach (var final in situation.Context.Bank[situation.Sop[1]].Where((x) => x.Chord.Root.ResidueNumber == 0 && x.Voicing.B.ResidueNumber == 0))
                    {
                        foreach (var pp in situation.Context.Bank[situation.Sop[0]].Where((x) => x.Voicing.B.ResidueNumber == 0))
                        {
                            switch (pp.Chord.Root.ResidueNumber)
                            {
                                case 4:
                                    yield return new Passage(
                                        situation.Context.Tonic,
                                        new Vert[] { pp, final },
                                        new Passage(situation.Context.Tonic, new Vert[] { pp }, null, null),
                                        new Passage(situation.Context.Tonic, new Vert[] { final }, null, null)
                                        );
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
                            yield return new Passage(
                                situation.Context.Tonic,
                                new Vert[] { pp, final },
                                new Passage(situation.Context.Tonic, new Vert[] { pp }, null, null),
                                new Passage(situation.Context.Tonic, new Vert[] { final }, null, null)
                                );
                        }
                    }
                }
            }

            //at least 1
            public static Dictionary<Situation, List<Passage>> Cache = new Dictionary<Situation, List<Passage>>();

            //at least 1
            public static IEnumerable<Passage> GetExterenal(Situation situation, double[] tolerence)
            {
                if (situation.Sop.Length == 1)
                {
                    //exactly 1
                    if (!Cache.ContainsKey(situation)) Cache.Add(situation, new List<Passage>(GetSingletons(situation, tolerence)));
                }
                else if (situation.Sop.Length == 2)
                {
                    //exactly 2
                    if (!Cache.ContainsKey(situation)) Cache.Add(situation, new List<Passage>(GetPairs(situation, tolerence)));
                }
                else
                {
                    //at least 3
                    if (!Cache.ContainsKey(situation)) AddInternal(situation, tolerence);
                }
                foreach (var item in Cache[situation])
                {
                    yield return item;
                }
            }

            //at least 3
            private static void AddInternal(Situation situation, double[] tolerence)
            {
                var list = new List<Passage>();
                Cache.Add(situation, list);
                foreach (var l in GetExterenal(situation.Left, tolerence))
                {
                    foreach (var r in GetExterenal(situation.Right, tolerence).Where((x) => x.Left == l.Right))
                    {
                        var obj = new Passage(l.Tonic, l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray(), l, r);
                        if (obj.Penalty <= tolerence[obj.Verts.Length]) list.Add(obj);
                    }
                }
            }

            #endregion


            public Context Context { get; }
            public int[] Sop { get; } //length at least 1
            public int Displacement { get; }

            private Situation(Context context, int[] sop, int displacement)
            {
                Context = context;
                Sop = sop;
                Displacement = displacement;
                if (Sop.Length == 1) return;
                Left = Instance(context, Sop.Take(Sop.Length - 1).ToArray(), displacement + 1);
                Right = Instance(context, Sop.Skip(1).ToArray(), displacement);
            }

            private static List<Situation> Instances = new List<Situation>();

            public static Situation Instance(Context context, int[] sop, int displacement)
            {
                var tempNew = new Situation(context, sop, displacement);
                if (!Instances.Contains(tempNew)) Instances.Add(tempNew);
                return Instances.First((x) => x.Equals(tempNew));
            }


            //These will be null rather then 'empty'
            public Situation Left { get; }
            public Situation Right { get; }



            public override bool Equals(object obj)
            {
                return Equals(obj as Situation);
            }

            public bool Equals(Situation other)
            {
                if (other == null ||
                    !ReferenceEquals(Context, other.Context) ||
                    Displacement != other.Displacement ||
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
                hashCode = hashCode * -1521134295 + Displacement.GetHashCode();
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
