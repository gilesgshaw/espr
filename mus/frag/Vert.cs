using mus.Gen;

namespace mus.Chorale
{

    // immutable, provided 'VoicingC' and 'Context' are
    // currently vunerable to invalid inputs
    // 'actual' pitches are implied relative to tonic,
    // but will go down by an octave if Tonic+Root overflows
    // i.e. pitch = (tonic +s root) +c voicing
    // retains reference to owner, i.e. 'Context', for valuation
    public class Vert : TreeValued
    {
        public Chord Chord { get; }
        public VoicingC Voicing { get; }
        public Context Context { get; }

        public Vert(Chord chord, VoicingC voicing, Context context) : base(
            new TreeValued[] { voicing })
        {
            Chord = chord;
            Voicing = voicing;
            Context = context;
        }
    }

}
