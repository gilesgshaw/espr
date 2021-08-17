using mus.Gen;

namespace mus.Chorale
{

    // immutable, provided 'Chord' and 'VoicingC' are
    // currently vunerable to invalid inputs
    // 'actual' pitches are implied relative to tonic,
    // but will go down by an octave if Tonic+Root overflows
    // i.e. pitch = (tonic +s root) +c voicing
    public class Vert : TreeValued
    {
        public Chord Chord { get; }
        public VoicingC Voicing { get; }

        public Vert(Chord chord, VoicingC voicing) : base(
            new TreeValued[] { voicing, chord })
        {
            Chord = chord;
            Voicing = voicing;
        }
    }

}
