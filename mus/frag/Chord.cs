using System.Collections.Generic;
using mus.Gen;
using System;

namespace mus
{

    // Is simply (IntervalS Root, Variety)
    // Root might be relative to 'tonic', or to 'C'
    // immutable (struct), provided 'Variety' is
    // currently vunerable to invalid inputs
    public struct Chord : IEquatable<Chord>
    {
        public IntervalS Root { get; }
        public Variety Variety { get; }

        public Chord(IntervalS root, Variety variety)
        {
            Root = root;
            Variety = variety;
        }

        public override bool Equals(object obj) => obj is Chord chord && Equals(chord);

        public bool Equals(Chord other)
        {
            return Root == other.Root
                && EqualityComparer<Variety>.Default.Equals(Variety, other.Variety);
        }

        public override int GetHashCode()
        {
            int hashCode = 2139337724;
            hashCode = hashCode * -1521134295 + Root.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Variety>.Default.GetHashCode(Variety);
            return hashCode;
        }
    }

}
