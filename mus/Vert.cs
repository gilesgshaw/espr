namespace mus
{
    public static partial class notation
    {

        public class Vert : Valued
        {
            public Chord Chord { get; }
            public VoicingC Voicing { get; }
            public VoicingC Absolute { get; }

            public Vert(Chord chord, VoicingC voicing) : base(new Valued[] { voicing, chord })
            {
                Chord = chord;
                Voicing = voicing;
                Absolute = Chord.Root + voicing;
            }
        }

    }
}
