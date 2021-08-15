using System.Collections.Generic;
using mus.Gen;

namespace mus.Chorale
{

    //Root is relative to tonic
    // immutable, provided 'Variety' is
    // currently vunerable to invalid inputs
    public class Chord : TreeValued
    {
        public IntervalS Root { get; }
        public Variety Variety { get; }

        public Chord(IntervalS root, Variety variety, double intrinsicPenalty) : base(intrinsicPenalty)
        {
            Root = root;
            Variety = variety;
        }

        public override double IntrinsicPenalty
        {
            get
            {
                var tr = base.IntrinsicPenalty;
                return tr;
            }
        }

    }

}
