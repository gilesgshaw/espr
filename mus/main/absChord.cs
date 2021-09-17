using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace mus
{

    // Is simply (Note Root, Variety)
    // immutable (struct), provided 'Variety' is
    // currently vunerable to invalid inputs
    public struct absChord : IEquatable<absChord>
    {
        public Note Root { get; }
        public Variety Variety { get; }

        public absChord(Note root, Variety variety)
        {
            Root = root;
            Variety = variety;
        }

        public override bool Equals(object obj) => obj is absChord chord && Equals(chord);

        public bool Equals(absChord other)
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
