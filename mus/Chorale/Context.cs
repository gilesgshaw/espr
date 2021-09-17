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
        public Note Tonic { get; }
        public Quad<(int, int)> Ranges; //absolute, from C0
        public relChord[] Chords { get; }

        private readonly Dictionary<Sound, Vert>[] iBank; //by MIDI pitches, 0-127
        private readonly ReadOnlyDictionary<Sound, Vert>[] iBanks;
        public ReadOnlyDictionary<Sound, Vert> Bank(int sop) => iBanks[sop];
        public Vert GetVert(Sound sound) => iBank[sound.S.MIDI][sound];

        public Context(Note tonic, IEnumerable<relChord> chords)
        {

            Tonic = tonic;

            Ranges = new Quad<(int, int)>((48, 65), (45, 59), (42, 53), (29, 45));

            Chords = chords.ToArray();

            iBank = Enumerable.Range(0, 127).Select((x) => new Dictionary<Sound, Vert>()).ToArray();
            iBanks = iBank.Select((x) => new ReadOnlyDictionary<Sound, Vert>(x)).ToArray();

            foreach (relChord chord in Chords)
            {
                var offset = (Tonic.FromC + chord.Root).ResidueSemis;
                var relativeRanges = Quad.Select(Ranges, (x) => (x.Item1 - offset, x.Item2 - offset));
                foreach (VoicingC voicing in Instances(chord.Variety, relativeRanges))
                {
                    var sound = new Sound(new Pitch(Tonic.FromC + chord.Root), voicing.S, voicing.A, voicing.T, voicing.B);
                    iBank[sound.S.MIDI][sound] = new Vert(chord, voicing, this);
                }
            }

        }

        //ranges measures from ROOT OF CHORD
        //returns relative voicings
        private static IEnumerable<VoicingC> Instances(Variety Variety, Quad<(int, int)> ranges)
        {
            return from v in VoicingS.FromVariety(Variety)
                   from V in VoicingC.FromSimple(v, ranges)
                   select V;
        }
    }

}
