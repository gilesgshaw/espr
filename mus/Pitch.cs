using System;

namespace mus
{

    public static partial class notation
    {
        // compared by...
        public struct Pitch : IEquatable<Pitch>, IComparable<Pitch>
        {
            public IntervalC IntervalFromC0;

            public Pitch(IntervalC IntervalFromC0)
            {
                this.IntervalFromC0 = IntervalFromC0;
            }

            public int MIDIPitch
            {
                get
                {
                    return 12 + IntervalFromC0.Semis;
                }
            }

            public override int GetHashCode()
            {
                return IntervalFromC0.GetHashCode();
            }

            public int CompareTo(Pitch other)
            {
                return IntervalFromC0.CompareTo(other.IntervalFromC0);
            }

            public static bool operator <(Pitch left, Pitch right)
            {
                return left.CompareTo(right) < 0;
            }

            public static bool operator >(Pitch left, Pitch right)
            {
                return left.CompareTo(right) > 0;
            }

            public static bool operator <=(Pitch left, Pitch right)
            {
                return left.CompareTo(right) <= 0;
            }

            public static bool operator >=(Pitch left, Pitch right)
            {
                return left.CompareTo(right) >= 0;
            }

            public static bool operator ==(Pitch a, Pitch b)
            {
                return a.IntervalFromC0 == b.IntervalFromC0;
            }

            public static bool operator !=(Pitch a, Pitch b)
            {
                return !(a == b);
            }

            public bool Equals(Pitch other)
            {
                return (this) == other;
            }

            public override bool Equals(object obj)
            {
                return obj is Pitch && (Pitch)obj == (this);
            }

            public static IntervalC operator -(Pitch a, Pitch b)
            {
                return a.IntervalFromC0 - b.IntervalFromC0;
            }

            //should this throw the exception?
            public override string ToString()
            {
                string acc = AccidentalSymbol(IntervalFromC0.Quality - Mode.Zero.QualByOffset(IntervalFromC0.NumberRem));
                switch (IntervalFromC0.NumberRem)
                {
                    case 0:
                        {
                            return "C" + acc + IntervalFromC0.Octaves;
                        }

                    case 1:
                        {
                            return "D" + acc + IntervalFromC0.Octaves;
                        }

                    case 2:
                        {
                            return "E" + acc + IntervalFromC0.Octaves;
                        }

                    case 3:
                        {
                            return "F" + acc + IntervalFromC0.Octaves;
                        }

                    case 4:
                        {
                            return "G" + acc + IntervalFromC0.Octaves;
                        }

                    case 5:
                        {
                            return "A" + acc + IntervalFromC0.Octaves;
                        }

                    case 6:
                        {
                            return "B" + acc + IntervalFromC0.Octaves;
                        }

                }
                throw new ArgumentException();
            }
        }

    }
}
