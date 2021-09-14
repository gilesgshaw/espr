using System;
using static mus.Notation;

namespace mus
{

    // compared by...
    public struct Pitch : IComparable<Pitch>, IEquatable<Pitch>
    {
        public IntervalC FromC0 { get; set; }
        public IntervalC FromM0 { get => this - MIDI0; set => this = MIDI0 + value; }
        public IntervalC FromMC { get => this - MiddleC; set => this = MiddleC + value; }
        public int MIDI => FromM0.Semis;


        public static Pitch MIDI0 { get; } = new Pitch(new IntervalC(0, 0, -1));
        public static Pitch C0 { get; } = new Pitch(new IntervalC(0, 0, 0));
        public static Pitch MiddleC { get; } = new Pitch(new IntervalC(0, 0, 4));


        public Pitch(IntervalC fromC0) => FromC0 = fromC0;


        public Note Residue => new Note(new IntervalS(FromMC));


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


        public override string ToString() => Residue.ToString() + FromC0.Octaves.ToString();
    }

}
