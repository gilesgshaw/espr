using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        // immutable, provided 'Context' is
        // currently vunerable to invalid inputs
        public class PassageSt : IEquatable<PassageSt>
        {
            public Context Context { get; }
            public ReadOnlyCollection<int> Sop { get; } //length at least 1
            public int Displacement { get; }
            public bool Initial { get; }

            private PassageSt(Context context, ReadOnlyCollection<int> sop, int displacement, bool initial)
            {
                Context = context;
                Sop = sop;
                Displacement = displacement;
                Initial = initial;
                if (Sop.Count == 1) return;
                Left = Instance(context, Array.AsReadOnly(Sop.Take(Sop.Count - 1).ToArray()), displacement + 1, initial);
                Right = Instance(context, Array.AsReadOnly(Sop.Skip(1).ToArray()), displacement, false);
            }

            private static readonly List<PassageSt> Instances = new List<PassageSt>();

            public static PassageSt Instance(Context context, ReadOnlyCollection<int> sop, int displacement, bool initial)
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
                    !Context.Equals(other.Context) ||
                    Displacement != other.Displacement ||
                    Initial != other.Initial ||
                    Sop.Count != other.Sop.Count) return false;
                for (int i = 0; i < Sop.Count; i++)
                {
                    if (Sop[i] != other.Sop[i]) return false;
                }
                return true;
            }

            //This may not be great
            public override int GetHashCode()
            {
                int hashCode = 280880954;
                hashCode = hashCode * -1521134295 + Context.GetHashCode();
                hashCode = hashCode * -1521134295 + Displacement.GetHashCode();
                hashCode = hashCode * -1521134295 + Sop.GetHashCode();
                hashCode = hashCode * -1521134295 + Initial.GetHashCode();
                return hashCode;
            }
        }

    }
}
