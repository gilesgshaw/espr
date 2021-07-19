using System.Collections.Generic;

namespace mus
{
    public static partial class notation
    {

        public class Context
        {

            public IntervalS Tonic { get; }
            //relative to tonic.
            public (int, int) bRange { get; }
            public (int, int) tRange { get; }
            public (int, int) aRange { get; }
            public (int, int) sRange { get; }
            public Chord[] Chords { get; }

            //key is MIDI pitches
            public Vert[][] Bank { get; }

            public Context()
            {

                Tonic = default;

                bRange = (31 - Tonic.ResidueSemis, 45 - Tonic.ResidueSemis);
                tRange = (42 - Tonic.ResidueSemis, 53 - Tonic.ResidueSemis);
                aRange = (45 - Tonic.ResidueSemis, 59 - Tonic.ResidueSemis);
                sRange = (48 - Tonic.ResidueSemis, 65 - Tonic.ResidueSemis);

                var tChords = new List<Chord>();

                var major = new Variety(0, null, 0, null, 0, null, null);
                var majorR = new Variety(0, null, 0, null, null, null, null);
                var minor = new Variety(0, null, -1, null, 0, null, null);
                var dominant = new Variety(0, null, 0, null, 0, null, -1);

                var I = IntervalS.GetNew(0, 0);
                var II = IntervalS.GetNew(1, 0);
                var IV = IntervalS.GetNew(3, 0);
                var V = IntervalS.GetNew(4, 0);
                var VI = IntervalS.GetNew(5, 0);

                tChords.Add(new Chord(I, major, 0));
                tChords.Add(new Chord(I, majorR, 0));
                tChords.Add(new Chord(II, minor, 5));
                tChords.Add(new Chord(IV, major, 0));
                tChords.Add(new Chord(V, major, 0));
                tChords.Add(new Chord(VI, minor, 5));

                Chords = tChords.ToArray();

                var tBank = new List<Vert>[128];
                for (int i = 0; i < 128; i++)
                {
                    tBank[i] = new List<Vert>();
                }
                foreach (var chord in Chords)
                {
                    foreach (var inst in chord.Instances(bRange, tRange, aRange, sRange))
                    {
                        var item = new Vert(chord, inst);
                        tBank[12 + Tonic.ResidueSemis + item.Chord.Root.ResidueSemis + item.Voicing.S.Semis].Add(item);
                    }
                }

                Bank = new Vert[128][];
                for (int i = 0; i < 128; i++)
                {
                    Bank[i] = tBank[i].ToArray();
                }

            }

        }

    }
}
