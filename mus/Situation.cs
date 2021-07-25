using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        public class Situation : IEquatable<Situation>
        {

            public Context Context;
            public int[] Sop;
            public int Displacement;

            public Situation(Context context, int[] sop, int displacement)
            {
                Context = context;
                Sop = sop;
                Displacement = displacement;
            }

            public Situation Left
            {
                get
                {
                    return new Situation(Context, Sop.Take(Sop.Length - 1).ToArray(), Displacement + 1);
                }
            }

            public Situation Right
            {
                get
                {
                    return new Situation(Context, Sop.Skip(1).ToArray(), Displacement);
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
