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

            //at least 1
            public static Dictionary<Situation, List<Passage>> Cache = new Dictionary<Situation, List<Passage>>();

            //at least 1
            public static IEnumerable<Passage> GetExterenal(Situation situation, double[] tolerence)
            {
                if (!Cache.ContainsKey(situation))
                {
                    AddInternal(situation, tolerence);
                }
                return Cache[situation];
            }

            //at least 1
            //tolerence only applies to child routines
            private static IEnumerable<Passage> GetInternal(Situation situation, double[] tolerence)
            {
                if (situation.Sop.Length == 1)
                {
                    //exactly 1
                    foreach (var item in situation.Context.Bank[situation.Sop[0]])
                    {
                        yield return new Passage(situation.Context.Tonic, new Vert[] { item }, null, null);
                    }
                }
                else if (situation.Sop.Length == 2)
                {
                    //exactly 2
                    if (situation.Displacement == 0)
                    {
                        foreach (var final in GetExterenal(situation.Right, tolerence).Where((x) => x.Verts[0].Chord.Root.ResidueNumber == 0 && x.Verts[0].Voicing.B.ResidueNumber == 0))
                        {
                            foreach (var pp in GetExterenal(situation.Left, tolerence).Where((x) => x.Verts[0].Voicing.B.ResidueNumber == 0))
                            {
                                switch (pp.Verts[0].Chord.Root.ResidueNumber)
                                {
                                    case 4:
                                        yield return new Passage(
                                            situation.Context.Tonic,
                                            new Vert[] { pp.Verts[0], final.Verts[0] },
                                            pp,
                                            final
                                            );
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var final in GetExterenal(situation.Right, tolerence))
                        {
                            foreach (var pp in GetExterenal(situation.Left, tolerence))
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
                    foreach (var l in GetExterenal(situation.Left, tolerence))
                    {
                        foreach (var r in GetExterenal(situation.Right, tolerence).Where((x) => ReferenceEquals(x.Left, l.Right)))
                        {
                            yield return new Passage(l.Tonic, l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray(), l, r);
                        }
                    }
                }
            }

            //at least 1
            private static void AddInternal(Situation situation, double[] tolerence)
            {
                Cache.Add(situation, new List<Passage>(GetInternal(situation, tolerence).Where((obj) => obj.Penalty <= tolerence[obj.Verts.Length])));
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
