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

    // immutable, provided 'Chord', 'Variety' and 'Vert' are
    // currently vunerable to invalid inputs (in various ways)
    public class Phrase : TreeValued, IEquatable<Phrase>
    {
        public int Length { get; }                                                            // at least 1

        public ReadOnlyCollection<IntervalS> LTonic { get; }
        public ReadOnlyCollection<IntervalS> RTonic { get; }              // l/r redundancy for convenience

        public ReadOnlyCollection<Vert> LVerts { get; }
        public ReadOnlyCollection<Vert> RVerts { get; }

        public ReadOnlyCollection<(IntervalS Root, Variety Variety)> Chords { get; }     // for convenience
        public ReadOnlyCollection<Chord> LChords { get; }                                // for convenience
        public ReadOnlyCollection<Chord> RChords { get; }                                // for convenience
        public ReadOnlyCollection<(Pitch S, Pitch A, Pitch T, Pitch B)> Pitches { get; } // for convenience

        public Phrase Left { get; }                                // references to children if applicable,
        public Phrase Right { get; }                                                      // otherwise null

        private Phrase(
            int length,
            ReadOnlyCollection<IntervalS> lTonic,
            ReadOnlyCollection<IntervalS> rTonic,
            ReadOnlyCollection<Vert> lVerts,
            ReadOnlyCollection<Vert> rVerts,
            ReadOnlyCollection<(IntervalS Root, Variety Variety)> chords,
            ReadOnlyCollection<Chord> lChords,
            ReadOnlyCollection<Chord> rChords,
            ReadOnlyCollection<(Pitch S, Pitch A, Pitch T, Pitch B)> pitches,
            Phrase left,
            Phrase right)
            : base()
        {
            Length = length;
            LTonic = lTonic;
            RTonic = rTonic;
            LVerts = lVerts;
            RVerts = rVerts;
            Chords = chords;
            LChords = lChords;
            RChords = rChords;
            Pitches = pitches;
            Left = left;
            Right = right;
        }

        public Phrase(IntervalS tonicL, IntervalS tonicR, Vert vertL, Vert vertR, (Pitch S, Pitch A, Pitch T, Pitch B) pitches)
            : base(new (double, TreeValued)[] { (0.5, vertL), (0.5, vertR) })
        {

            Length = 1;

            Pitches = Array.AsReadOnly(new[] { pitches });
            LTonic = Array.AsReadOnly(new[] { tonicL });
            RTonic = Array.AsReadOnly(new[] { tonicR });
            LVerts = Array.AsReadOnly(new[] { vertL });
            RVerts = Array.AsReadOnly(new[] { vertR });

            Chords = Array.AsReadOnly(Enumerable.Zip(LVerts, LTonic, (x, y) =>
                          (x.Chord.Root + y, x.Chord.Variety)
                      ).ToArray());
            LChords = Array.AsReadOnly(LVerts.Select((x) =>
                          x.Chord
                      ).ToArray());
            RChords = Array.AsReadOnly(RVerts.Select((x) =>
                          x.Chord
                      ).ToArray());

        }

        public static bool Combine(Phrase l, Phrase r, out Phrase full)
        {
            full = null;

            if (l.Length == 1 && l.RTonic[0] != r.LTonic[0]) return false;

            full = new Phrase(

                lTonic: Comb(l.LTonic, r.LTonic),
                rTonic: Comb(l.RTonic, r.RTonic),
                lVerts: Comb(l.LVerts, r.LVerts),
                rVerts: Comb(l.RVerts, r.RVerts),
                chords: Comb(l.Chords, r.Chords),
                lChords: Comb(l.LChords, r.LChords),
                rChords: Comb(l.RChords, r.RChords),
                pitches: Comb(l.Pitches, r.Pitches),

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
                var PitchMx = Array.ConvertAll(Pitches.ToArray(),
                    (x) => new Pitch[] {
                            x.S,
                            x.A,
                            x.T,
                            x.B
                    });
                var PitchMxR = Array.ConvertAll(Pitches.ToArray(),
                    (x) => new IntervalS[] {
                            x.S.FromC0.Residue,
                            x.A.FromC0.Residue,
                            x.T.FromC0.Residue,
                            x.B.FromC0.Residue
                    });
                var TrsMx =
                    Enumerable.Zip(PitchMx, PitchMx.Skip(1),
                    (x, y) =>
                    {
                        return Enumerable.Zip(x, y,
                            (a, b) => b - a)
                        .ToArray();
                    }
                    ).ToArray();
                var TrsMxR =
                    Enumerable.Zip(PitchMx, PitchMx.Skip(1),
                    (x, y) =>
                    {
                        return Enumerable.Zip(x, y,
                            (a, b) => (b - a).Residue)
                        .ToArray();
                    }
                    ).ToArray();
                var TrsAr =
                    Enumerable.Zip(Pitches, Pitches.Skip(1),
                    (x, y) => (
                        S: y.S - x.S,
                        A: y.A - x.A,
                        T: y.T - x.T,
                        B: y.B - x.B
                    )).ToArray();


                if (Length == 1) // Length 1
                {

                    //Bad chords
                    if (LTonic[0] == RTonic[0])
                    {
                        if (LChords[0].Root.ResidueNumber == 5 && LVerts[0].Voicing.B.ResidueNumber == 2) tr += 35;
                        if (LChords[0].Root.ResidueNumber == 1 && LVerts[0].Voicing.B.ResidueNumber == 0) tr += 15;
                    }
                    else
                    {
                        if (LChords[0].Root.ResidueNumber == 5 && LVerts[0].Voicing.B.ResidueNumber == 2) tr += 45;
                        if (LChords[0].Root.ResidueNumber == 1 && LVerts[0].Voicing.B.ResidueNumber == 0) tr += 25;
                        if (RChords[0].Root.ResidueNumber == 5 && RVerts[0].Voicing.B.ResidueNumber == 2) tr += 45;
                        if (RChords[0].Root.ResidueNumber == 1 && RVerts[0].Voicing.B.ResidueNumber == 0) tr += 25;
                    }

                    //Doubling 5th or root in chord 7
                    if (LChords[0].Root.ResidueNumber == 6 || RChords[0].Root.ResidueNumber == 6)
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
                    if (Chords[0].Variety.PQ7 == -2 &&
                        (Pitches[0].B) > (Pitches[1].B)) tr += 13;
                    if (Chords[0].Variety.PQ7 == -2 &&
                        (Pitches[0].S) < (Pitches[1].S)) tr += 13;

                    //Chord transition
                    if (Chords[1].Root == Chords[0].Root && Pitches[0].B == Pitches[1].B) tr += 65;
                    switch ((Chords[1].Root - Chords[0].Root).ResidueNumber)
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

                }


                if (Length == 3) // Length 3
                {

                    //Chord transition 1 to 3
                    if (Chords[2].Root == Chords[0].Root && Pitches[0].B == Pitches[2].B) tr += 40;

                    //Bass jumping
                    tr += 2.5 * Max(0, TrsAr[0].B.Semis - 2) * Max(0, TrsAr[1].B.Semis - 2);

                }


                return tr;
            }
        }


        public override bool Equals(object obj) => Equals(obj as Phrase);

        public bool Equals(Phrase other)
        {
            if
            (
                other == null ||
                other.Length != Length
            )
                return false;

            for (int i = 0; i < Length; i++)
                if
                (
                    !EqualityComparer<Vert>.Default.Equals(LVerts[i], other.LVerts[i]) ||
                    !EqualityComparer<Vert>.Default.Equals(RVerts[i], other.RVerts[i]) ||
                    !EqualityComparer<IntervalS>.Default.Equals(LTonic[i], other.LTonic[i]) ||
                    !EqualityComparer<IntervalS>.Default.Equals(RTonic[i], other.RTonic[i])
                )
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = -87926583;
            for (int i = 0; i < Length; i++)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<Vert>.Default.GetHashCode(LVerts[i]);
                hashCode = hashCode * -1521134295 + EqualityComparer<Vert>.Default.GetHashCode(RVerts[i]);
                hashCode = hashCode * -1521134295 + EqualityComparer<IntervalS>.Default.GetHashCode(LTonic[i]);
                hashCode = hashCode * -1521134295 + EqualityComparer<IntervalS>.Default.GetHashCode(RTonic[i]);
            }
            return hashCode;
        }
    }

}
