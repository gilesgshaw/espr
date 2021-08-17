using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus.Chorale
{

    //TODO: make immutable (as promised to rest of code)
    // no comparisons / hash code implemented
    // should create 'once and for all' and then simply compare by reference
    public class Context
    {
        public IntervalS Tonic { get; }
        public Quad<(int, int)> Ranges; //relative to tonic.
        public Chord[] Chords { get; }

        private readonly Dictionary<Sound, Vert>[] iBank; //by MIDI pitches, 0-127
        private readonly ReadOnlyDictionary<Sound, Vert>[] iBanks;
        public ReadOnlyDictionary<Sound, Vert> Bank(int sop) => iBanks[sop];

        public Context(IntervalS tonic, IEnumerable<Chord> chords)
        {

            Tonic = tonic;

            Ranges = new Quad<(int, int)>(
                (48 - Tonic.ResidueSemis, 65 - Tonic.ResidueSemis),
                (45 - Tonic.ResidueSemis, 59 - Tonic.ResidueSemis),
                (42 - Tonic.ResidueSemis, 53 - Tonic.ResidueSemis),
                (29 - Tonic.ResidueSemis, 45 - Tonic.ResidueSemis));

            Chords = chords.ToArray();

            iBank = Enumerable.Range(0, 127).Select((x) => new Dictionary<Sound, Vert>()).ToArray();
            iBanks = iBank.Select((x) => new ReadOnlyDictionary<Sound, Vert>(x)).ToArray();

            foreach (Chord chord in Chords)
            {
                foreach (VoicingC voicing in Instances(chord, Ranges))
                {
                    var sound = new Sound(new Pitch(((IntervalC)Tonic) + chord.Root), voicing.S, voicing.A, voicing.T, voicing.B);
                    iBank[sound.S.MIDI][sound] = new Vert(chord, voicing);
                }
            }

        }

        //I think ranges count from tonic.
        //returns relative voicings
        private static IEnumerable<VoicingC> Instances(Chord ch, Quad<(int, int)> Ranges)
        {
            return from v in VoicingS.FromVariety(ch.Variety)
                   from V in VoicingC.FromSimple(v,
                       (Ranges.B.Item1 - ch.Root.ResidueSemis, Ranges.B.Item2 - ch.Root.ResidueSemis),
                       (Ranges.T.Item1 - ch.Root.ResidueSemis, Ranges.T.Item2 - ch.Root.ResidueSemis),
                       (Ranges.A.Item1 - ch.Root.ResidueSemis, Ranges.A.Item2 - ch.Root.ResidueSemis),
                       (Ranges.S.Item1 - ch.Root.ResidueSemis, Ranges.S.Item2 - ch.Root.ResidueSemis))
                   select V;
        }
    }

}
