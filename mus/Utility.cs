using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus.Gen
{

    public static class Equate
    {
        public static XEqualityComparer<T> When<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            return new XEqualityComparer<T>(equals, getHashCode);
        }
    }

    public class XEqualityComparer<T> : IEqualityComparer<T>
    {
        public XEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            iEquals = equals;
            iGetHashCode = getHashCode;
        }

        private readonly Func<T, T, bool> iEquals;
        private readonly Func<T, int> iGetHashCode;

        public bool Equals(T x, T y)
        {
            return iEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return iGetHashCode(obj);
        }
    }

    public static class Compare
    {
        public static XComparer<T> With<T>(Func<T, T, int> compare)
        {
            return new XComparer<T>(compare);
        }
    }

    public class XComparer<T> : IComparer<T>
    {
        public XComparer(Func<T, T, int> compare)
        {
            iCompare = compare;
        }

        private readonly Func<T, T, int> iCompare;

        public int Compare(T x, T y)
        {
            return iCompare(x, y);
        }
    }

    public static class Ut
    {

#pragma warning disable IDE1006 // Naming Styles
        public static int mod(int b, int a)
        {
            int result = a % b;
            if (result >= 0)
            {
                return result;
            }
            else if (b >= 0)
            {
                return result + b;
            }
            else
            {
                return result - b;
            }
        }
#pragma warning restore IDE1006 // Naming Styles

        // TODO placeholder for a possibly faster method
        public static ReadOnlyCollection<T> Comb<T>(ReadOnlyCollection<T> l, ReadOnlyCollection<T> r)
        {
            return Array.AsReadOnly(l.Take(1).Concat(r).ToArray());
        }

    }

}
