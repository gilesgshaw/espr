using mus.Gen;

namespace mus.Chorale
{

    // immutable
    // currently vunerable to invalid inputs
    // 'actual' pitches are implied relative to tonic,
    // but will go down by an octave if Tonic+Root overflows
    // i.e. pitch = (tonic +s root) +c voicing
    // retains reference to owner, i.e. tonality, for valuation
    public class Vert : TreeValued
    {
        public relChord Chord { get; }
        public VoicingC Voicing { get; }
        public Tonality Owner { get; }

        public override double IntrinsicPenalty
        {
            get
            {
                var tr = base.IntrinsicPenalty;
                tr += Owner.Chords[Chord];
                return tr;
            }
        }

        public Vert(relChord chord, VoicingC voicing, Tonality owner) : base(
            new TreeValued[] { voicing })
        {
            Chord = chord;
            Voicing = voicing;
            Owner = owner;
        }
    }

}
