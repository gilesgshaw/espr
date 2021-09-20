using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static mus.Notation;
using static mus.Gen.Ut;
using mus.Gen;
using static System.Math;

namespace mus.Chorale
{

    // immutable
    // currently vunerable to invalid inputs (in various ways)
    // currently no comparisons / hash code implemented,
    // since we intend to 'know' the identity of each instance generated
    // keeps reference to 'Owner' i.e. PhraseSt instance.
    public class Phrase : TreeValued
    {
        public int Length { get; }                                                            // at least 1
        public PhraseSt Owner { get; }

        public ReadOnlyCollection<Moment> Moments { get; }
        public ReadOnlyCollection<Context> LContext { get; }
        public ReadOnlyCollection<Context> RContext { get; }

        public ReadOnlyCollection<Vert> LVerts { get; }                                  // for convenience
        public ReadOnlyCollection<Vert> RVerts { get; }                                  // for convenience

        public Phrase Left { get; }                                // references to children if applicable,
        public Phrase Right { get; }                                                      // otherwise null


        private Phrase(
            int length,
            PhraseSt owner,
            ReadOnlyCollection<Context> lContext,
            ReadOnlyCollection<Context> rContext,
            ReadOnlyCollection<Vert> lVerts,
            ReadOnlyCollection<Vert> rVerts,
            ReadOnlyCollection<Moment> moments,
            Phrase left,
            Phrase right)
            : base()
        {
            Length = length;
            Owner = owner;
            LContext = lContext;
            RContext = rContext;
            LVerts = lVerts;
            RVerts = rVerts;
            Moments = moments;
            Left = left;
            Right = right;
        }

        public Phrase(PhraseSt owner, Context lContext, Context rContext, Moment pitches)
            : base(new (double, TreeValued)[] { (0.5, lContext.GetVert(pitches)), (0.5, rContext.GetVert(pitches)) })
        {

            Length = 1;
            Owner = owner;

            Vert vertL = lContext.GetVert(pitches);
            Vert vertR = rContext.GetVert(pitches);

            Moments = Array.AsReadOnly(new[] { pitches });
            LContext = Array.AsReadOnly(new[] { lContext });
            RContext = Array.AsReadOnly(new[] { rContext });
            LVerts = Array.AsReadOnly(new[] { vertL });
            RVerts = Array.AsReadOnly(new[] { vertR });

        }

        public static bool Combine(PhraseSt parent, Phrase l, Phrase r, out Phrase full)
        {

            // this will disallow abrupt modulations:
            // full = null;
            // if (l.Length == 1 && !(ReferenceEquals(l.RContext[0], r.LContext[0]))) return false;

            full = new Phrase(

                owner: parent,

                lContext: Comb(l.LContext, r.LContext),
                rContext: Comb(l.RContext, r.RContext),
                lVerts: Comb(l.LVerts, r.LVerts),
                rVerts: Comb(l.RVerts, r.RVerts),
                moments: Comb(l.Moments, r.Moments),

                length: l.Length + 1,
                left: l,
                right: r);

            full.AddChildren(new[] { l, r });
            if (full.Length > 2) full.AddChildren(new (double, TreeValued)[] { (-1, l.Right) });

            return true;
        }


        public override double IntrinsicPenalty
        {
            get
            {
                var tr = base.IntrinsicPenalty;

                // setup
                Pitch[][] PitchMx = Array.ConvertAll(Moments.ToArray(),
                    (x) => new Pitch[] {
                            x.S,
                            x.A,
                            x.T,
                            x.B
                    });
                IntervalS[][] PitchMxR = Array.ConvertAll(Moments.ToArray(),
                    (x) => new IntervalS[] {
                            x.S.FromC0.Residue,
                            x.A.FromC0.Residue,
                            x.T.FromC0.Residue,
                            x.B.FromC0.Residue
                    });
                IntervalC[][] TrsMx =
                    Enumerable.Zip(PitchMx, PitchMx.Skip(1),
                    (x, y) =>
                    {
                        return Enumerable.Zip(x, y,
                            (a, b) => b - a)
                        .ToArray();
                    }
                    ).ToArray();
                IntervalS[][] TrsMxR =
                    Enumerable.Zip(PitchMx, PitchMx.Skip(1),
                    (x, y) =>
                    {
                        return Enumerable.Zip(x, y,
                            (a, b) => (b - a).Residue)
                        .ToArray();
                    }
                    ).ToArray();
                (IntervalC S, IntervalC A, IntervalC T, IntervalC B)[] TrsAr =
                    Enumerable.Zip(Moments, Moments.Skip(1),
                    (x, y) => (
                        S: y.S - x.S,
                        A: y.A - x.A,
                        T: y.T - x.T,
                        B: y.B - x.B
                    )).ToArray();


                if (Length == 1) // Length 1
                {

                    //Bad chords
                    if (ReferenceEquals(LContext[0], RContext[0]))
                    {
                        if (LVerts[0].Chord.Root.ResidueNumber == 5 && LVerts[0].Voicing.B.ResidueNumber == 2) tr += 45;
                        if (LVerts[0].Chord.Root.ResidueNumber == 1 && LVerts[0].Voicing.B.ResidueNumber == 0) tr += 25;
                    }
                    else
                    {
                        if (LVerts[0].Chord.Root.ResidueNumber == 5 && LVerts[0].Voicing.B.ResidueNumber == 2) tr += 35;
                        if (LVerts[0].Chord.Root.ResidueNumber == 1 && LVerts[0].Voicing.B.ResidueNumber == 0) tr += 15;
                        if (LVerts[0].Chord.Root.ResidueNumber == 5 && RVerts[0].Voicing.B.ResidueNumber == 2) tr += 35;
                        if (LVerts[0].Chord.Root.ResidueNumber == 1 && RVerts[0].Voicing.B.ResidueNumber == 0) tr += 15;
                    }

                    //Doubling 5th or root in chord 7
                    if (LVerts[0].Chord.Root.ResidueNumber == 6 || RVerts[0].Chord.Root.ResidueNumber == 6)
                    {
                        int[] profile = new int[7];
                        profile[LVerts[0].Voicing.B.ResidueNumber]++;
                        profile[LVerts[0].Voicing.T.ResidueNumber]++;
                        profile[LVerts[0].Voicing.A.ResidueNumber]++;
                        profile[LVerts[0].Voicing.S.ResidueNumber]++;
                        if (profile[4] >= 2) tr += 60;
                        if (profile[0] >= 2) tr += 60;
                    }

                }


                if (Length == 2) // Length 2
                {

                    //Voice leading
                    tr += Abs(TrsAr[0].A.Semis) + 2.5 * Max(0, Abs(TrsAr[0].A.Semis) - 3);
                    tr += Abs(TrsAr[0].T.Semis) + 2.5 * Max(0, Abs(TrsAr[0].T.Semis) - 3);
                    tr += 0.5 * Abs(TrsAr[0].B.Semis) + (1.8 * Max(0, Abs(TrsAr[0].B.Semis) - 4.5)
                        * (1.5 * LVerts[1].Voicing.B.ResidueNumber + 1.5 * LVerts[0].Voicing.B.ResidueNumber + 1));
                    double numStatic = 0;
                    if (TrsAr[0].S.Semis == 0) numStatic += 1; //This should be left in!
                    if (TrsAr[0].A.Semis == 0) numStatic += 1;
                    if (TrsAr[0].T.Semis == 0) numStatic += 1.5;
                    if (TrsAr[0].B.Semis == 0) numStatic += 2.5;
                    tr += numStatic * numStatic * 3;

                    //Arkward intervals
                    if (TrsAr[0].S.Quality > 0 || TrsAr[0].S.Quality < -1 || (TrsAr[0].S.Quality < 0 && Degree.IsConsonant(TrsAr[0].S.ResidueNumber))) tr += 18;
                    if (TrsAr[0].A.Quality > 0 || TrsAr[0].A.Quality < -1 || (TrsAr[0].A.Quality < 0 && Degree.IsConsonant(TrsAr[0].A.ResidueNumber))) tr += 18;
                    if (TrsAr[0].T.Quality > 0 || TrsAr[0].T.Quality < -1 || (TrsAr[0].T.Quality < 0 && Degree.IsConsonant(TrsAr[0].T.ResidueNumber))) tr += 18;
                    if (TrsAr[0].B.Quality > 0 || TrsAr[0].B.Quality < -1 || (TrsAr[0].B.Quality < 0 && Degree.IsConsonant(TrsAr[0].B.ResidueNumber))) tr += 18;

                    //Resolving 7th
                    //Can it resolve several chords later?
                    if (LVerts[0].Voicing.S.ResidueNumber == 6 && TrsAr[0].S.Number != -1) tr += 70;
                    if (LVerts[0].Voicing.A.ResidueNumber == 6 && TrsAr[0].A.Number != -1) tr += 70;
                    if (LVerts[0].Voicing.T.ResidueNumber == 6 && TrsAr[0].T.Number != -1) tr += 70;
                    if (LVerts[0].Voicing.B.ResidueNumber == 6 && TrsAr[0].B.Number != -1) tr += 70;

                    //Preparing 7th
                    //Should a 7th be prepared *exactly* in the previous chord? probably
                    if (LVerts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) >= 2) tr += 70;
                    if (LVerts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) >= 2) tr += 70;
                    if (LVerts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) >= 2) tr += 70;
                    if (LVerts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) >= 2) tr += 70;
                    if (LVerts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) == 1) tr += 35;
                    if (LVerts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) == 1) tr += 35;
                    if (LVerts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) == 1) tr += 35;
                    if (LVerts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) == 1) tr += 35;
                    if (LVerts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) == 0) tr -= 6;
                    if (LVerts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) == 0) tr -= 6;
                    if (LVerts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) == 0) tr -= 6;
                    if (LVerts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) == 0) tr -= 6;

                    //Movement after dim 7
                    if (Moments[0].Chord.Variety.PQ7 == -2 &&
                        (Moments[0].B) > (Moments[1].B)) tr += 13;
                    if (Moments[0].Chord.Variety.PQ7 == -2 &&
                        (Moments[0].S) < (Moments[1].S)) tr += 13;

                    //Chord transition
                    if (Moments[1].Chord.Root == Moments[0].Chord.Root && Moments[0].B == Moments[1].B) tr += 65;
                    switch ((Moments[1].Chord.Root - Moments[0].Chord.Root).ResidueNumber)
                    {
                        case 3: tr += -10; break;
                        case 5: tr += -5; break;
                        case 4: tr += 0; break;
                        case 1: tr += 5; break;
                        case 6: tr += 20; break;
                        case 2: tr += 25; break;
                        case 0: tr += 32; break;
                    }

                    //Paralells
                    for (int i1 = 0; i1 < 4; i1++)
                    {
                        for (int i2 = i1 + 1; i2 < 4; i2++)
                        {
                            if (TrsMxR[0][i1] == TrsMxR[0][i2] && TrsMxR[0][i1] != new IntervalS())
                            {
                                if (
                                    PitchMxR[0][i1] - PitchMxR[0][i2] == new IntervalS(4, 7) ||
                                    PitchMxR[0][i1] - PitchMxR[0][i2] == new IntervalS())
                                {
                                    tr += 50;
                                }
                            }
                        }
                    }

                    //Overlapping
                    for (int i1 = 0; i1 < 3; i1++)
                    {
                        int i2 = i1 + 1;

                        if (PitchMx[0][i1] < PitchMx[1][i2])
                        {
                            tr += 45;
                        }
                        if (PitchMx[1][i1] < PitchMx[0][i2])
                        {
                            tr += 45;
                        }
                    }

                    // Abrupt modulation
                    if (!(ReferenceEquals(RContext[0], LContext[1])))
                    {
                        tr += 45;
                    }

                }


                if (Length == 3) // Length 3
                {

                    //Chord transition 1 to 3
                    if (Moments[2].Chord.Root == Moments[0].Chord.Root && Moments[0].B == Moments[2].B) tr += 40;

                    //Bass jumping
                    tr += 2.5 * Max(0, TrsAr[0].B.Semis - 2) * Max(0, TrsAr[1].B.Semis - 2);

                }


                return tr;
            }
        }
    }

}
