using System.Collections.Generic;
using System;

namespace mus.Chorale
{

    // immutable
    public class Sound : Quad<Pitch>, IEquatable<Sound>
    {
        public Sound(Pitch s, Pitch a, Pitch t, Pitch b) : base(s, a, t, b) { }
        public Sound(Pitch reference, IntervalC s, IntervalC a, IntervalC t, IntervalC b) : base(reference + s, reference + a, reference + t, reference + b) { }

        public override bool Equals(object obj) => Equals(obj as Sound);

        public bool Equals(Sound other)
        {
            return !(other is null) &&
                S == other.S &&
                A == other.A &&
                T == other.T &&
                B == other.B;
        }

        public override int GetHashCode()
        {
            int hashCode = -813885500;
            hashCode = hashCode * -1521134295 + S.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + T.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Sound left, Sound right) => EqualityComparer<Sound>.Default.Equals(left, right);
        public static bool operator !=(Sound left, Sound right) => !(left == right);
    }

}
