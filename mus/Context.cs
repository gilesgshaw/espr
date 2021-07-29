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

            public Context(IntervalS tonic)
            {

                Tonic = tonic;

                bRange = (29 - Tonic.ResidueSemis, 45 - Tonic.ResidueSemis);
                tRange = (42 - Tonic.ResidueSemis, 53 - Tonic.ResidueSemis);
                aRange = (45 - Tonic.ResidueSemis, 59 - Tonic.ResidueSemis);
                sRange = (48 - Tonic.ResidueSemis, 65 - Tonic.ResidueSemis);

                var tChords = new List<Chord>();

                var major = new Variety(0, null, 0, null, 0, null, null, (false, ""));
                var majorR = new Variety(0, null, 0, null, null, null, null, (false, ""));
                var minor = new Variety(0, null, -1, null, 0, null, null, (true, ""));
                var dim = new Variety(0, null, -1, null, -1, null, null, (true, ((char)0x006F).ToString()));
                var dominant7 = new Variety(0, null, 0, null, 0, null, -1, (false, "7"));
                var minor7 = new Variety(0, null, -1, null, 0, null, -1, (true, "7"));
                var hdim7 = new Variety(0, null, -1, null, -1, null, -1, (true, ((char)0x00F8) + "7"));

                var I = IntervalS.GetNew(0, 0);
                var II = IntervalS.GetNew(1, 0);
                var III = IntervalS.GetNew(2, 0);
                var IV = IntervalS.GetNew(3, 0);
                var V = IntervalS.GetNew(4, 0);
                var VI = IntervalS.GetNew(5, 0);
                var VII = IntervalS.GetNew(6, 0);

                tChords.Add(new Chord(I, major, 0));
                tChords.Add(new Chord(I, majorR, 0));
                tChords.Add(new Chord(II, minor, 3));
                tChords.Add(new Chord(II, minor7, -6));
                tChords.Add(new Chord(IV, major, 0));
                tChords.Add(new Chord(V, major, 0));
                tChords.Add(new Chord(V, dominant7, -8));
                tChords.Add(new Chord(VI, minor, 3));
                tChords.Add(new Chord(VII, dim, 4));
                tChords.Add(new Chord(VII, hdim7, -4));

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
