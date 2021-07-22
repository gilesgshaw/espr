namespace mus
{
    public static partial class notation
    {

        //'based off of' tonic
        public class Vert : TreeValued
        {
            public Chord Chord { get; }
            public VoicingC Voicing { get; }

            //public VoicingC Absolute { get; }

            public Vert(Chord chord, VoicingC voicing) : base(new TreeValued[] { voicing, chord })
            {
                Chord = chord;
                Voicing = voicing;
                //Absolute = Chord.Root + voicing;
            }
        }

    }
}
