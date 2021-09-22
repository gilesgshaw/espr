using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using mus.Gen;

namespace mus.Chorale
{

    // immutable
    // currently vunerable to invalid inputs
    public class PhraseSt
    {
        public Climate Climate { get; }
        public ReadOnlyCollection<int> Sop { get; } // length at least 1
        public int MarginL { get; }
        public int MarginR { get; }
        public bool Initial { get; }
        public bool Final { get; }

        public PhraseSt Left { get; }            // null or  left child
        public PhraseSt Right { get; }           // null or right child

        private PhraseSt(Climate climate, ReadOnlyCollection<int> sop, int marginL, int marginR, bool initial, bool final)
        {
            Climate = climate;
            Sop = sop;
            MarginL = marginL;
            MarginR = marginR;
            Initial = initial;
            Final = final;
            if (Sop.Count == 1) return;
            Left = Instance(climate, Array.AsReadOnly(Sop.Take(Sop.Count - 1).ToArray()), marginL, marginR + 1, initial, final);
            Right = Instance(climate, Array.AsReadOnly(Sop.Skip(1).ToArray()), marginL + 1, marginR, initial, final);
        }

        private static readonly HashSet<PhraseSt> instances = new HashSet<PhraseSt>(Equate.When<PhraseSt>((x, y) =>
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            if
            (
                !EqualityComparer<Climate>.Default.Equals(x.Climate, y.Climate) ||
                x.MarginL != y.MarginL ||
                x.MarginR != y.MarginR ||
                x.Initial != y.Initial ||
                x.Final != y.Final ||
                x.Sop.Count != y.Sop.Count
            )
                return false;

            for (int i = 0; i < x.Sop.Count; i++)
            {
                if (x.Sop[i] != y.Sop[i]) return false;
            }

            return true;

        }, (x) =>
        {
            if (x == null) return 0;

            int hashCode = 280880954;

            hashCode = hashCode * -1521134295 + x.MarginL.GetHashCode();
            hashCode = hashCode * -1521134295 + x.MarginR.GetHashCode();
            hashCode = hashCode * -1521134295 + x.Initial.GetHashCode();
            hashCode = hashCode * -1521134295 + x.Final.GetHashCode();

            hashCode = hashCode * -1521134295 + EqualityComparer<Climate>.Default.GetHashCode(x.Climate);

            for (int i = 0; i < x.Sop.Count; i++)
            {
                hashCode = hashCode * -1521134295 + x.Sop[i].GetHashCode();
            }

            return hashCode;

        }));

        public static PhraseSt Instance(Climate climate, ReadOnlyCollection<int> sop, int marginL, int marginR, bool initial, bool final)
        {
            var tempNew = new PhraseSt(climate, sop, marginL, marginR, initial, final);
            if (instances.Add(tempNew)) return tempNew;
            return instances.First((x) => instances.Comparer.Equals(x, tempNew));
        }
    }

}
