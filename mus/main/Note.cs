using System;
using System.Linq;
using static mus.Notation;

namespace mus
{

    public struct Note : IEquatable<Note>
    {
        public IntervalS FromC { get; set; }
        public Note(IntervalS fromC) => FromC = fromC;

        public static Note C { get; } = new Note(new IntervalS(0, 0));

        public static bool operator ==(Note left, Note right) => left.FromC == right.FromC;
        public static bool operator !=(Note left, Note right) => left.FromC != right.FromC;
        public bool Equals(Note other) => FromC == other.FromC;
        public override bool Equals(object obj) => obj is Note note && Equals(note);
        public override int GetHashCode() => -363936462 + FromC.GetHashCode();

        public static IntervalS operator -(Note a, Note b) => a.FromC - b.FromC;
        public static Note operator +(Note a, IntervalS b) => new Note(a.FromC + b);

        #region CDEFGAB

        public static bool TryGetDegree(char letter, out int degree)
        {
            if (names.Contains(letter))
            {
                degree = names.IndexOf(letter);
                return true;
            }
            else
            {
                degree = 0;
                return false;
            }
        }

        public static char GetLetter(int degree) => names[degree];

        private static readonly string names = "CDEFGAB";

        #endregion

        public override string ToString() => GetLetter(FromC.ResidueNumber) + AccidentalSymbol(Mode.Zero.Accidental(FromC), false, false);
    }

}
