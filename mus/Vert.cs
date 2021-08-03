namespace mus
{
    public static partial class notation
    {

        //'based off of' tonic
        // immutable, provided 'Chord' and 'VoicingC' are
        // currently vunerable to invalid inputs
        public class Vert : TreeValued
        {
            public Chord Chord { get; }
            public VoicingC Voicing { get; }

            public Vert(Chord chord, VoicingC voicing) : base(new TreeValued[] { voicing, chord })
            {
                Chord = chord;
                Voicing = voicing;
            }
        }

    }
}
