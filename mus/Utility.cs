using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus
{

    public static class Equate
    {
        public static xEqualityComparer<T> When<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            return new xEqualityComparer<T>(equals, getHashCode);
        }
    }

    public class xEqualityComparer<T> : IEqualityComparer<T>
    {
        public xEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            iEquals = equals;
            iGetHashCode = getHashCode;
        }

        private Func<T, T, bool> iEquals;
        private Func<T, int> iGetHashCode;

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
        public static xComparer<T> With<T>(Func<T, T, int> compare)
        {
            return new xComparer<T>(compare);
        }
    }

    public class xComparer<T> : IComparer<T>
    {
        public xComparer(Func<T, T, int> compare)
        {
            iCompare = compare;
        }

        private Func<T, T, int> iCompare;

        public int Compare(T x, T y)
        {
            return iCompare(x, y);
        }
    }

    public static class Ut
    {

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

        // TODO placeholder for a possibly faster method
        public static ReadOnlyCollection<T> Comb<T>(ReadOnlyCollection<T> l, ReadOnlyCollection<T> r)
        {
            return Array.AsReadOnly(l.Take(1).Concat(r).ToArray());
        }

    }

}
