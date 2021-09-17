using System.Collections.Generic;
using System;

namespace mus.Chorale
{

    // immutable
    public class Moment : Quad<Pitch>, IEquatable<Moment>
    {
        public Note Root { get; }
        public VoicingC Voicing { get; }
        public absChord Chord { get; } // for convenience


        public Moment(Note root, VoicingC voicing) :
            base(
                (Pitch)root + voicing.S,
                (Pitch)root + voicing.A,
                (Pitch)root + voicing.T,
                (Pitch)root + voicing.B)
        {
            Root = root;
            Voicing = voicing;
            Chord = new absChord(root, voicing.Variety);
        }


        public override bool Equals(object obj) => Equals(obj as Moment);

        public bool Equals(Moment other)
        {
            return !(other is null) &&
                S == other.S &&
                A == other.A &&
                T == other.T &&
                B == other.B &&
                Root == other.Root;
        }

        public override int GetHashCode()
        {
            int hashCode = -813885500;
            hashCode = hashCode * -1521134295 + S.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + T.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + Root.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Moment left, Moment right) => EqualityComparer<Moment>.Default.Equals(left, right);
        public static bool operator !=(Moment left, Moment right) => !(left == right);
    }

}
