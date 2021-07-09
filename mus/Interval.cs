using System;

namespace mus
{

    public static partial class notation
    {
        
        // compared by...
        // consistent naming - rem vs res
        public struct Interval : IEquatable<Interval>, IComparable<Interval>
        {

            //private static string GetOrdinalSuffix(int n)
            //{
            //    if (n < 0)
            //        throw new ArgumentException();
            //    n = n % 100;
            //    if (n == 11 || n == 12 || n == 13)
            //        return "th";
            //    if (n % 10 == 1)
            //        return "st";
            //    if (n % 10 == 2)
            //        return "nd";
            //    if (n % 10 == 3)
            //        return "rd";
            //    return "th";
            //}

            public int Semis;
            public int Number;

            public Interval(int number, int semis)
            {
                Number = number;
                Semis = semis;
            }

            public Interval(int number, int quality, int octaves)
            {
                Number = number + octaves * 7;
                Semis = 0;
                Semis = quality - Quality;
            }

            public int NumberRem
            {
                get
                {
                    return mod(7, Number);
                }
            }

            public int SemisRem
            {
                get
                {
                    return Semis - 12 * Octaves;
                }
            }

            public int Octaves
            {
                get
                {
                    return (Number - NumberRem) / 7;
                }
            }

            public int Quality
            {
                get
                {
                    return SemisRem - Degree.Semis(NumberRem);
                }
            }

            public override bool Equals(object obj)
            {
                return obj is Interval interval && Equals(interval);
            }

            public bool Equals(Interval other)
            {
                return Semis == other.Semis &&
                       Number == other.Number;
            }

            public override int GetHashCode()
            {
                int hashCode = 1960790096;
                hashCode = hashCode * -1521134295 + Semis.GetHashCode();
                hashCode = hashCode * -1521134295 + Number.GetHashCode();
                return hashCode;
            }

            public int CompareTo(Interval other)
            {
                int result = Number.CompareTo(other.Number);
                if (result != 0) return result;
                return Semis.CompareTo(other.Semis);
            }

            public static bool operator ==(Interval left, Interval right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Interval left, Interval right)
            {
                return !(left == right);
            }

            public static Interval operator +(Interval a, Interval b)
            {
                return new Interval(a.Number + b.Number, a.Semis + b.Semis);
            }

            public static Interval operator -(Interval a)
            {
                return new Interval(-a.Number, -a.Semis);
            }

            public static Interval operator -(Interval a, Interval b)
            {
                return new Interval(a.Number - b.Number, a.Semis - b.Semis);
            }

            public static bool operator <(Interval left, Interval right)
            {
                return left.CompareTo(right) < 0;
            }

            public static bool operator <=(Interval left, Interval right)
            {
                return left.CompareTo(right) <= 0;
            }

            public static bool operator >(Interval left, Interval right)
            {
                return left.CompareTo(right) > 0;
            }

            public static bool operator >=(Interval left, Interval right)
            {
                return left.CompareTo(right) >= 0;
            }

            public Interval Residue
            {
                get
                {
                    return this - new Interval(0, 0, Octaves);
                }
            }
        }

    }
}
