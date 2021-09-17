﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus.Chorale
{

    // immutable
    // no comparisons / hash code implemented
    public class Context
    {
        public Note Tonic { get; }
        public Tonality Tonality { get; }


        public Context(Note tonic, Tonality tonality)
        {
            Tonic = tonic;
            Tonality = tonality;

            Quad<(int, int)> Ranges = new Quad<(int, int)>((48, 65), (45, 59), (42, 53), (29, 45)); //absolute, from C0

            iBank = Enumerable.Range(0, 127).Select((x) => new Dictionary<Sound, Vert>()).ToArray();
            iBanks = iBank.Select((x) => new ReadOnlyDictionary<Sound, Vert>(x)).ToArray();

            foreach (relChord chord in tonality.Chords.Keys)
            {
                var offset = (Tonic.FromC + chord.Root).ResidueSemis;
                var relativeRanges = Quad.Select(Ranges, (x) => (x.Item1 - offset, x.Item2 - offset));
                foreach (VoicingC voicing in Instances(chord.Variety, relativeRanges))
                {
                    var sound = new Sound(new Pitch(Tonic.FromC + chord.Root), voicing.S, voicing.A, voicing.T, voicing.B);
                    iBank[sound.S.MIDI][sound] = new Vert(chord, voicing, tonality);
                }
            }
        }


        private readonly Dictionary<Sound, Vert>[] iBank; //by MIDI pitches, 0-127
        private readonly ReadOnlyDictionary<Sound, Vert>[] iBanks;
        public ReadOnlyDictionary<Sound, Vert> Bank(int sop) => iBanks[sop];
        public Vert GetVert(Sound sound) => iBank[sound.S.MIDI][sound];


        //ranges measured from ROOT OF CHORD. returns relative voicings.
        private static IEnumerable<VoicingC> Instances(Variety Variety, Quad<(int, int)> ranges)
        {
            return from v in VoicingS.FromVariety(Variety)
                   from V in VoicingC.FromSimple(v, ranges)
                   select V;
        }
    }

}
