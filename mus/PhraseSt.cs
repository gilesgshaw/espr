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
        public class PhraseSt
        {
            public ReadOnlyCollection<Context> Contexts { get; }
            public ReadOnlyCollection<int> Sop { get; } // length at least 1
            public int Displacement { get; }
            public bool Initial { get; }
            public PhraseSt Left { get; }            // null or  left child
            public PhraseSt Right { get; }           // null or right child

            private PhraseSt(ReadOnlyCollection<Context> contexts, ReadOnlyCollection<int> sop, int displacement, bool initial)
            {
                Contexts = contexts;
                Sop = sop;
                Displacement = displacement;
                Initial = initial;
                if (Sop.Count == 1) return;
                Left = Instance(contexts, Array.AsReadOnly(Sop.Take(Sop.Count - 1).ToArray()), displacement + 1, initial);
                Right = Instance(contexts, Array.AsReadOnly(Sop.Skip(1).ToArray()), displacement, false);
            }

            private static readonly HashSet<PhraseSt> instances = new HashSet<PhraseSt>(Equate.When<PhraseSt>((x, y) =>
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;

                if
                (
                    x.Contexts.Count != y.Contexts.Count ||
                    x.Displacement != y.Displacement ||
                    x.Initial != y.Initial ||
                    x.Sop.Count != y.Sop.Count
                )
                    return false;

                for (int i = 0; i < x.Sop.Count; i++)
                {
                    if (x.Sop[i] != y.Sop[i]) return false;
                }
                
                for (int i = 0; i < x.Contexts.Count; i++)
                {
                    if (!EqualityComparer<Context>.Default.Equals(x.Contexts[i], y.Contexts[i])) return false;
                }

                return true;

            }, (x) =>
            {
                if (x == null) return 0;

                int hashCode = 280880954;

                hashCode = hashCode * -1521134295 + x.Displacement.GetHashCode();
                hashCode = hashCode * -1521134295 + x.Initial.GetHashCode();
                
                for (int i = 0; i < x.Contexts.Count; i++)
                {
                    hashCode = hashCode * -1521134295 + EqualityComparer<Context>.Default.GetHashCode(x.Contexts[i]);
                }

                for (int i = 0; i < x.Sop.Count; i++)
                {
                    hashCode = hashCode * -1521134295 + x.Sop[i].GetHashCode();
                }

                return hashCode;

            }));

            public static PhraseSt Instance(ReadOnlyCollection<Context> contexts, ReadOnlyCollection<int> sop, int displacement, bool initial)
            {
                var tempNew = new PhraseSt(contexts, sop, displacement, initial);
                if (instances.Add(tempNew)) return tempNew;
                return instances.First((x) => instances.Comparer.Equals(x, tempNew));
            }
        }

    }
}
