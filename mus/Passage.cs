using System;

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
                    return tr;
                }
            }

            public Passage(IntervalS tonic, Vert[] verts) : base()
            {
                Tonic = tonic;
                Chords = Array.ConvertAll(verts, (x) => x.Chord);
                Verts = verts;
            }
        }

    }
}
