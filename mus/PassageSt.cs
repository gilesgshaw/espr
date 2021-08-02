using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        //should make this properly immutable
        public class PassageSt : IEquatable<PassageSt>
        {
            public Context Context { get; }
            public int[] Sop { get; } //length at least 1
            public int Displacement { get; }
            public bool Initial { get; }

            private PassageSt(Context context, int[] sop, int displacement, bool initial)
            {
                Context = context;
                Sop = sop;
                Displacement = displacement;
                Initial = initial;
                if (Sop.Length == 1) return;
                Left = Instance(context, Sop.Take(Sop.Length - 1).ToArray(), displacement + 1, initial);
                Right = Instance(context, Sop.Skip(1).ToArray(), displacement, false);
            }

            private static List<PassageSt> Instances = new List<PassageSt>();

            public static PassageSt Instance(Context context, int[] sop, int displacement, bool initial)
            {
                var tempNew = new PassageSt(context, sop, displacement, initial);
                if (!Instances.Contains(tempNew)) Instances.Add(tempNew);
                return Instances.First((x) => x.Equals(tempNew));
            }


            //These will be null rather then 'empty'
            public PassageSt Left { get; }
            public PassageSt Right { get; }



            public override bool Equals(object obj)
            {
                return Equals(obj as PassageSt);
            }

            public bool Equals(PassageSt other)
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

            public static bool operator ==(PassageSt left, PassageSt right)
            {
                return EqualityComparer<PassageSt>.Default.Equals(left, right);
            }

            public static bool operator !=(PassageSt left, PassageSt right)
            {
                return !(left == right);
            }
        }

    }
}
