using System;
using static System.Math;

namespace mus
{
    public static partial class notation
    {

        //could do with some optimisation...
        //for later should maybe store the pitches directly...
        //and in fact they are probably the 'independant' data.
        public class Passage : Valued
        {
            public IntervalS Tonic { get; }
            public Vert[] Verts { get; }
            public Chord[] Chords { get; }

            //public IntervalC[][] Pitches { get; } //satb

            public override double Penalty
            {
                get
                {
                    var tr = base.Penalty;
                    for (int i = 1; i < Chords.Length; i++)
                    {
                        var RootChange = Verts[i].Chord.Root.ResidueSemis - Verts[i - 1].Chord.Root.ResidueSemis;
                        var SChange = Verts[i].Voicing.S.Semis - Verts[i - 1].Voicing.S.Semis;
                        var AChange = Verts[i].Voicing.A.Semis - Verts[i - 1].Voicing.A.Semis;
                        var TChange = Verts[i].Voicing.T.Semis - Verts[i - 1].Voicing.T.Semis;
                        var BChange = Verts[i].Voicing.B.Semis - Verts[i - 1].Voicing.B.Semis;
                        tr += Abs(RootChange+SChange);
                        tr += Abs(RootChange+AChange);
                        tr += Abs(RootChange+TChange);
                        tr += Abs(RootChange+BChange);
                        if (ReferenceEquals(Chords[i], Chords[i - 1]) && Verts[i].Voicing.B.Residue == Verts[i - 1].Voicing.B.Residue)
                        {
                            tr += 60;
                        }
                    }
                    return tr;
                }
            }

            public Passage(IntervalS tonic, Vert[] verts) : base(verts)
            {
                Tonic = tonic;
                Chords = Array.ConvertAll(verts, (x) => x.Chord);
                Verts = verts;
            }
        }

    }
}
