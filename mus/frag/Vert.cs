using mus.Gen;

namespace mus.Chorale
{

    //'based off of' tonic
    // immutable, provided 'Chord' and 'VoicingC' are
    // currently vunerable to invalid inputs
    public class Vert : VQuad<IntervalC>
    {
        public Chord Chord { get; }
        public VoicingC Voicing { get; }

        public Vert(Chord chord, VoicingC voicing) : base(
            chord.Root + voicing.S,
            chord.Root + voicing.A,
            chord.Root + voicing.T,
            chord.Root + voicing.B,
            new TreeValued[] { voicing, chord })
        {
            Chord = chord;
            Voicing = voicing;
        }
    }

}
