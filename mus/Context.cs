using System.Collections.Generic;
using System.Linq;

namespace mus.Chorale
{

    //TODO: make immutable (as promised to rest of code)
    public class Context
    {
        public IntervalS Tonic { get; }
        public (int, int) bRange { get; } //relative to tonic.
        public (int, int) tRange { get; } //relative to tonic.
        public (int, int) aRange { get; } //relative to tonic.
        public (int, int) sRange { get; } //relative to tonic.
        public Chord[] Chords { get; }
        public Vert[][] Bank { get; } //key is MIDI pitches

        public Context(IntervalS tonic, IEnumerable<Chord> chords)
        {

            Tonic = tonic;

            bRange = (29 - Tonic.ResidueSemis, 45 - Tonic.ResidueSemis);
            tRange = (42 - Tonic.ResidueSemis, 53 - Tonic.ResidueSemis);
            aRange = (45 - Tonic.ResidueSemis, 59 - Tonic.ResidueSemis);
            sRange = (48 - Tonic.ResidueSemis, 65 - Tonic.ResidueSemis);

            Chords = chords.ToArray();

            var tBank = new List<Vert>[128];
            for (int i = 0; i < 128; i++)
            {
                tBank[i] = new List<Vert>();
            }
            foreach (var chord in Chords)
            {
                foreach (var inst in Instances(chord, bRange, tRange, aRange, sRange))
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

        //I think ranges count from tonic.
        //returns relative voicings
        private static IEnumerable<VoicingC> Instances(Chord ch, (int, int) bRange, (int, int) tRange, (int, int) aRange, (int, int) sRange)
        {
            foreach (var v in VoicingS.FromVariety(ch.Variety))
            {
                foreach (var V in VoicingC.FromSimple(v,
                    (bRange.Item1 - ch.Root.ResidueSemis, bRange.Item2 - ch.Root.ResidueSemis),
                    (tRange.Item1 - ch.Root.ResidueSemis, tRange.Item2 - ch.Root.ResidueSemis),
                    (aRange.Item1 - ch.Root.ResidueSemis, aRange.Item2 - ch.Root.ResidueSemis),
                    (sRange.Item1 - ch.Root.ResidueSemis, sRange.Item2 - ch.Root.ResidueSemis)))
                {
                    yield return V;
                }
            }
        }
    }

}
