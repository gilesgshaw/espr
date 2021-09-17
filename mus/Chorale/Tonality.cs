using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mus.Chorale
{

    // immutable
    // currently vunerable to invalid inputs
    // currently no comparisons / hashing implemented
    public class Tonality
    {
        private readonly Dictionary<relChord, double> _Chords;

        public ReadOnlyDictionary<relChord, double> Chords { get; }

        public Tonality(Dictionary<relChord, double> chords)
        {
            _Chords = chords;
            Chords = new ReadOnlyDictionary<relChord, double>(_Chords);
        }
    }

}
