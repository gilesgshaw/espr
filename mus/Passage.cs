using System;

namespace mus
{
    public static partial class notation
    {

        //could do with some optimisation...
        public class Passage : Valued
        {
            IntervalS Tonic { get; }
            IntervalS[] Roots { get; }
            IntervalC[][] Pitches { get; } //satb
            Vert[] Verts { get; }
            Chord[] Chords { get; }

            private static Vert[] GetVerts(IntervalS tonic, Chord[] chords, IntervalC[][] pitches)
            {
                var Roots = Array.ConvertAll(chords, (x) => x.Root);
                var Verts = new Vert[chords.Length];
                for (int i = 0; i < chords.Length; i++)
                {
                    Verts[i] = new Vert(chords[i], new VoicingC(
                        pitches[i][0] - tonic - Roots[i],
                        pitches[i][1] - tonic - Roots[i],
                        pitches[i][2] - tonic - Roots[i],
                        pitches[i][3] - tonic - Roots[i]
                        ));
                }
                return Verts;
            }

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    return tr;
                }
            }

            public Passage(IntervalS tonic, Chord[] chords, IntervalC[][] pitches) : base()
            {
                Tonic = tonic;
                Chords = chords;
                Roots = Array.ConvertAll(chords, (x) => x.Root);
                Pitches = pitches;
                Verts = GetVerts(tonic, chords, pitches);
            }
        }

    }
}
