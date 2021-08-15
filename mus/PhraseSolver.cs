﻿using System.Collections.Generic;
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
            if (problem.Sop.Count == 1 && problem.Initial)                 // opening chord
            {
                return base.Refine(problem, solutions.Where((x) =>
                {
                    return x.Item1.LChords[0].Root.ResidueNumber == 0 && x.Item1.LVerts[0].Voicing.B.ResidueNumber == 0;
                }
                ).ToArray());                                              // should be I(a)
            }
            else if (problem.Sop.Count == 1 && problem.Displacement < 2)   // last or penultimate chord
            {
                return base.Refine(problem, solutions.Where((x) =>
                {
                    return x.Item1.LTonic[0] == x.Item1.RTonic[0];         // no modulations
                }
                ).ToArray());
            }
            else if (problem.Sop.Count == 2 && problem.Displacement == 0)  // cadence
            {
                return base.Refine(problem, solutions.Where((x) =>
                {
                    return                                                 // perfect V-I
                    (x.Item1.LChords[0].Root.ResidueNumber == 4 && x.Item1.LVerts[0].Voicing.B.ResidueNumber == 0 &&
                    x.Item1.LChords[1].Root.ResidueNumber == 0 && x.Item1.LVerts[1].Voicing.B.ResidueNumber == 0)
                    ||                                                     // imperfect ?-V
                    (!x.Item1.LVerts[1].Chord.Variety.PQ7.HasValue &&
                    x.Item1.LChords[1].Root.ResidueNumber == 4 && x.Item1.LVerts[1].Voicing.B.ResidueNumber == 0);
                }
                ).ToArray());
            }
            else                                                           // anything else
            {
                return base.Refine(problem, solutions);                    // is fine.
            }
            //could also check if this is a final chord (i.e. should be I or V)
        }

        //WIP
        protected override IEnumerable<Phrase> SolveSingleton(PhraseSt problem)
        {
            foreach (var ctL in problem.Contexts)
            {
                foreach (var ctR in problem.Contexts)
                {
                    foreach (var l in ctL.Bank[problem.Sop[0]])
                    {
                        foreach (var r in ctR.Bank[problem.Sop[0]])
                        {
                            if (l.Chord.Root + l.Voicing.S + ctL.Tonic != r.Chord.Root + r.Voicing.S + ctR.Tonic) continue;
                            if (l.Chord.Root + l.Voicing.A + ctL.Tonic != r.Chord.Root + r.Voicing.A + ctR.Tonic) continue;
                            if (l.Chord.Root + l.Voicing.T + ctL.Tonic != r.Chord.Root + r.Voicing.T + ctR.Tonic) continue;
                            if (l.Chord.Root + l.Voicing.B + ctL.Tonic != r.Chord.Root + r.Voicing.B + ctR.Tonic) continue;
                            yield return new Phrase(
                                ctL.Tonic,
                                ctR.Tonic,
                                l,
                                r,
                                (new Pitch(l.Chord.Root + l.Voicing.S + ctL.Tonic),
                                new Pitch(l.Chord.Root + l.Voicing.A + ctL.Tonic),
                                new Pitch(l.Chord.Root + l.Voicing.T + ctL.Tonic),
                                new Pitch(l.Chord.Root + l.Voicing.B + ctL.Tonic)));
                        }
                    }
                }
            }
        }

        protected override bool Combine(Phrase left, Phrase right, out Phrase full) => Phrase.Combine(left, right, out full);
        protected override PhraseSt Left(PhraseSt parent) => parent.Left;
        protected override PhraseSt Right(PhraseSt parent) => parent.Right;
    }

}