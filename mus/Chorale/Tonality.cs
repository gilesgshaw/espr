using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mus.Chorale
{

    // immutable
    // currently vunerable to invalid inputs
    // currently no comparisons / hashing implemented
    public class Tonality
    {
        public ReadOnlyDictionary<relChord, double> Chords { get; }

        public Mode SignatureMode { get; }


        // does not wrap dictionary
        public Tonality(ReadOnlyDictionary<relChord, double> chords, Mode signatureMode)
        {
            Chords = chords;
            SignatureMode = signatureMode;
        }

        // wraps dictionary
        public Tonality(IDictionary<relChord, double> chords, Mode signatureMode)
            : this(new ReadOnlyDictionary<relChord, double>(chords), signatureMode) { }
    }

}
