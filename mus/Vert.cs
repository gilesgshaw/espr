namespace mus
{
    public static partial class notation
    {

        public class Vert
        {
            public Chord Chord { get; }
            public VoicingC Voicing { get; }
            public VoicingC Absolute { get; }

            public Vert(Chord chord, VoicingC voicing)
            {
                Chord = chord;
                Voicing = voicing;
                Absolute = Chord.Root + voicing;
            }
        }

    }
}
