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
            public Context Context { get; }
            public int[] Sop { get; } //length at least 1
            public int Displacement { get; }
            public bool Initial { get; }

            private Situation(Context context, int[] sop, int displacement, bool initial)
            {
                Context = context;
                Sop = sop;
                Displacement = displacement;
                Initial = initial;
                if (Sop.Length == 1) return;
                Left = Instance(context, Sop.Take(Sop.Length - 1).ToArray(), displacement + 1, initial);
                Right = Instance(context, Sop.Skip(1).ToArray(), displacement, false);
            }

            private static List<Situation> Instances = new List<Situation>();

            public static Situation Instance(Context context, int[] sop, int displacement, bool initial)
            {
                var tempNew = new Situation(context, sop, displacement, initial);
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
                    Initial != other.Initial ||
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
