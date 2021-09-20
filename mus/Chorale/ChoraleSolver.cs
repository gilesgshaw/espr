using System.Collections.Generic;
using System.Linq;
using mus;
using System;
using mus.Gen;

namespace mus.Chorale
{

    public class ChoraleSolver : StringValuer<PassageSt, Passage>
    {
        protected override int Capacity(PassageSt problem) => caps[problem.Sop.Count];
        protected override double Tolerence(PassageSt problem) => tols[problem.Sop.Count];

        private readonly int[] caps;
        private readonly double[] tols;

        public ChoraleSolver(int[] caps, double[] tols) : base(Equate.When<PassageSt>(
            (x, y) => ReferenceEquals(x, y),
            (x) => x == null ? 0 : x.GetHashCode()))
        {
            this.caps = caps;
            this.tols = tols;
        }


        protected override IEnumerable<(Passage, T)> Refine<T>(PassageSt situation, (Passage, T)[] solutions)
        {
            if (situation.Sop.Count == 1 && situation.Initial)                // opening chord (i.e. I(a))
            {
                return base.Refine(situation, solutions.Where((x) =>
                {
                    return x.Item1.Chords[0].Root.ResidueNumber == 0 && x.Item1.Verts[0].Voicing.B.ResidueNumber == 0;
                }
                ).ToArray());
            }
            else if (situation.Sop.Count == 2 && situation.Displacement == 0) // cadence
            {
                return base.Refine(situation, solutions.Where((x) =>
                {
                    return  //   perfect V-I
                    (x.Item1.Chords[0].Root.ResidueNumber == 4 && x.Item1.Verts[0].Voicing.B.ResidueNumber == 0 &&
                    x.Item1.Chords[1].Root.ResidueNumber == 0 && x.Item1.Verts[1].Voicing.B.ResidueNumber == 0)
                    ||      // imperfect ?-V
                    (!x.Item1.Verts[1].Chord.Variety.PQ7.HasValue &&
                    x.Item1.Chords[1].Root.ResidueNumber == 4 && x.Item1.Verts[1].Voicing.B.ResidueNumber == 0);
                }
                ).ToArray());
            }
            else                                                               // anything else
            {
                return base.Refine(situation, solutions);
            }
            //could also check if this is a final chord (i.e. should be I or V)
        }


        protected override bool Combine(PassageSt parent, Passage l, Passage r, out Passage full)
        {
            full = new Passage(l.Tonic, Array.AsReadOnly(l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray()), l, r);
            return true;
        }

        protected override PassageSt Left(PassageSt parent) => parent.Left;
        protected override PassageSt Right(PassageSt parent) => parent.Right;
        protected override IEnumerable<Passage> SolveSingleton(PassageSt problem) =>
            problem.Context.Bank(problem.Sop[0]).Select((x) => new Passage(problem.Context.Tonic, Array.AsReadOnly(new[] { x.Value }), null, null));
    }

}
