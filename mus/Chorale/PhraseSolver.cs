using System;
using System.Collections.Generic;
using System.Linq;
using mus.Gen;

namespace mus.Chorale
{

    public class PhraseSolver : StringValuer<PhraseSt, Phrase>
    {
        protected override int Capacity(PhraseSt problem) => caps[problem.Sop.Count];
        protected override double Tolerence(PhraseSt problem) => tols[problem.Sop.Count];

        private readonly int[] caps;
        private readonly double[] tols;

        //this (and corresponding one) may be a bit off with e/h.
        public PhraseSolver(int[] caps, double[] tols) : base(Equate.When<PhraseSt>(
            (x, y) => ReferenceEquals(x, y),
            (x) => x == null ? 0 : x.GetHashCode()))
        {
            this.caps = caps;
            this.tols = tols;
        }

        protected override IEnumerable<(Phrase, T)> Refine<T>(PhraseSt problem, (Phrase, T)[] solutions)
        {
            // could also short circuit on final chord...
            // [here] what if both opening and cadence...

            //           -----------------------   prepare predicates   ------------------------------

            Func<(Phrase, T), bool> ValidCadence = (x) =>
            {
                return                                                 //  perfect  V-I
                        (x.Item1.LVerts[0].Chord.Root.ResidueNumber == 4 && x.Item1.LVerts[0].Voicing.B.ResidueNumber == 0 &&
                        x.Item1.LVerts[1].Chord.Root.ResidueNumber == 0 && x.Item1.LVerts[1].Voicing.B.ResidueNumber == 0)
                        ||                                             // imperfect ?-V
                        (!x.Item1.LVerts[1].Chord.Variety.PQ7.HasValue &&
                        x.Item1.LVerts[1].Chord.Root.ResidueNumber == 4 && x.Item1.LVerts[1].Voicing.B.ResidueNumber == 0);
            };

            Func<(Phrase, T), bool> ValidOpening = (x) =>
            {                                                          //  opening  I(a)
                return x.Item1.LVerts[0].Chord.Root.ResidueNumber == 0 && x.Item1.LVerts[0].Voicing.B.ResidueNumber == 0;
            };

            Func<(Phrase, T), bool> NoModulation = (x) =>
            {
                return ReferenceEquals(x.Item1.RContext[0], x.Item1.LContext[1]);
            };

            Func<(Phrase, T), bool> NoPivotingMd = (x) =>
            {
                return ReferenceEquals(x.Item1.LContext[0], x.Item1.RContext[0]);
            };


            //           -----------------------       then apply       ------------------------------

            switch (problem.Sop.Count)
            {
                case 1:

                    if (problem.Initial && problem.MarginL == 0) // first chord, opening phrase
                    {
                        return base.Refine(problem, solutions.Where(ValidOpening).ToArray());
                    }
                    else if (problem.MarginR < 2)                // last or penultimate chord
                    {
                        return base.Refine(problem, solutions.Where(NoPivotingMd).ToArray());
                    }

                    break;
                case 2:

                    switch (problem.MarginR)
                    {
                        case 0:                                  // tail of cadence
                            return base.Refine(problem, solutions.Where(ValidCadence).Where(NoModulation).ToArray());

                        case 1:                                  // head of cadence
                            return base.Refine(problem, solutions.Where(NoModulation).ToArray());
                    }

                    break;
            }                                                    // anything else
            return base.Refine(problem, solutions);

        }

        //WIP
        protected override IEnumerable<Phrase> SolveSingleton(PhraseSt problem)
        {
            foreach (var ctL in problem.Climate.Contexts)
            {
                foreach (var ctR in problem.Climate.Contexts)
                {
                    foreach (var moment in ctL.Bank(problem.Sop[0]).Keys.Intersect(ctR.Bank(problem.Sop[0]).Keys))
                        yield return new Phrase(
                            problem,
                            ctL,
                            ctR,
                            moment);
                }
            }
        }

        protected override bool Combine(PhraseSt parent, Phrase left, Phrase right, out Phrase full) => Phrase.Combine(parent, left, right, out full);
        protected override PhraseSt Left(PhraseSt parent) => parent.Left;
        protected override PhraseSt Right(PhraseSt parent) => parent.Right;
    }

}
