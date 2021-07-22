﻿using System.Collections.Generic;

namespace mus
{
    public static partial class notation
    {

        //Root is relative to tonic
        public class Chord : TreeValued
        {
            public IntervalS Root { get; }
            public Variety Variety { get; }

            public Chord(IntervalS root, Variety variety, double intrinsicPenalty) : base(intrinsicPenalty)
            {
                Root = root;
                Variety = variety;
            }

            public override double IntrinticPenalty
            {
                get
                {
                    var tr = base.IntrinticPenalty;
                    return tr;
                }
            }

            //I think ranges count from tonic.
            //returns relative voicings
            public IEnumerable<VoicingC> Instances((int, int) bRange, (int, int) tRange, (int, int) aRange, (int, int) sRange)
            {
                foreach (var v in VoicingS.FromVariety(Variety))
                {
                    foreach (var V in VoicingC.FromSimple(v,
                        (bRange.Item1 - Root.ResidueSemis, bRange.Item2 - Root.ResidueSemis),
                        (tRange.Item1 - Root.ResidueSemis, tRange.Item2 - Root.ResidueSemis),
                        (aRange.Item1 - Root.ResidueSemis, aRange.Item2 - Root.ResidueSemis),
                        (sRange.Item1 - Root.ResidueSemis, sRange.Item2 - Root.ResidueSemis)))
                    {
                        yield return V;
                    }
                }
            }
        }

    }
}
