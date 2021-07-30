using System;

namespace mus
{
    public static partial class notation
    {

        //should this throw the exception?
        public static string AccidentalSymbol(int alt)
        {
            switch (alt)
            {
                case -2:
                    {
                        return "𝄫";
                    }

                case -1:
                    {
                        return "♭";
                    }

                case 0:
                    {
                        return string.Empty;
                    }

                case 1:
                    {
                        return "♯";
                    }

                case 2:
                    {
                        return "𝄪";
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        // compared by...
        public struct Pitch : IComparable<Pitch>, IEquatable<Pitch>
        {
            public IntervalC FromC0 { get; set; }
            public IntervalC FromM0 { get => this - MIDI0; set => this = MIDI0 + value; }
            public IntervalC FromMC { get => this - MiddleC; set => this = MiddleC + value; }
            public int MIDI => FromM0.Semis;


            public static readonly Pitch MIDI0 = new Pitch(new IntervalC(0, 0, -1));
            public static readonly Pitch C0 = new Pitch(new IntervalC(0, 0, 0));
            public static readonly Pitch MiddleC = new Pitch(new IntervalC(0, 0, 4));


            public Pitch(IntervalC fromC0) => FromC0 = fromC0;


            public static IntervalC operator -(Pitch a, Pitch b) => a.FromC0 - b.FromC0;
            public static Pitch operator +(Pitch a, IntervalC b) => new Pitch(a.FromC0 + b);

            public static bool operator ==(Pitch left, Pitch right) => left.FromC0 == right.FromC0;
            public static bool operator !=(Pitch left, Pitch right) => left.FromC0 != right.FromC0;
            public bool Equals(Pitch other) => FromC0 == other.FromC0;
            public int CompareTo(Pitch other) => FromC0.CompareTo(other.FromC0);

            public override bool Equals(object obj) => obj is Pitch pitch && Equals(pitch);
            public static bool operator <(Pitch left, Pitch right) => left.CompareTo(right) < 0;
            public static bool operator <=(Pitch left, Pitch right) => left.CompareTo(right) <= 0;
            public static bool operator >(Pitch left, Pitch right) => left.CompareTo(right) > 0;
            public static bool operator >=(Pitch left, Pitch right) => left.CompareTo(right) >= 0;
            public override int GetHashCode() => 649032966 + FromC0.GetHashCode();


            //should this throw the exception?
            public override string ToString()
            {
                string acc = AccidentalSymbol(FromC0.Quality - Mode.Zero.QualByOffset(FromC0.ResidueNumber));
                switch (FromC0.ResidueNumber)
                {
                    case 0:
                        {
                            return "C" + acc + FromC0.Octaves;
                        }

                    case 1:
                        {
                            return "D" + acc + FromC0.Octaves;
                        }

                    case 2:
                        {
                            return "E" + acc + FromC0.Octaves;
                        }

                    case 3:
                        {
                            return "F" + acc + FromC0.Octaves;
                        }

                    case 4:
                        {
                            return "G" + acc + FromC0.Octaves;
                        }

                    case 5:
                        {
                            return "A" + acc + FromC0.Octaves;
                        }

                    case 6:
                        {
                            return "B" + acc + FromC0.Octaves;
                        }

                }
                throw new ArgumentException();
            }
        }

    }
}
