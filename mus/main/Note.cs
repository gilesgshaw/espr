using System;
using System.Linq;
using static mus.Notation;

namespace mus
{

    public struct Note : IEquatable<Note>
    {
        public IntervalS FromC { get; set; }
        public Note(IntervalS fromC) => FromC = fromC;

        public static implicit operator Pitch(Note obj) => new Pitch(obj.FromC);

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

        public static bool TryParse(string value, out Note result)
        {

            result = default;
            value = value.Trim().ToUpperInvariant();
            if (value.Length == 1) value = value + " ";
            if (value.Length != 2) return false;
            // have length 2 string

            // parse first character
            int degree;
            if (!TryGetDegree(value[0], out degree)) return false;

            // parse second character
            switch (value[1])
            {
                case 'B':
                    result = new Note(IntervalS.GetNew(degree, -1));
                    return true;
                case '#':
                    result = new Note(IntervalS.GetNew(degree, 1));
                    return true;
                case ' ':
                    result = new Note(IntervalS.GetNew(degree, 0));
                    return true;
                default:
                    return false;
            }
        }
    }

}
