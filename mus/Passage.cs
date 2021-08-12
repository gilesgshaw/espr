using System;
using System.Collections.ObjectModel;
using System.Linq;
using static System.Math;

namespace mus
{

    // immutable, provided 'Chord' and 'Vert' are
    // currently vunerable to invalid inputs
    public class Passage : TreeValued, IEquatable<Passage>
    {
        public IntervalS Tonic { get; }
        public ReadOnlyCollection<Vert> Verts { get; } //length at least 1

        public ReadOnlyCollection<Chord> Chords { get; } // for convenience
        public ReadOnlyCollection<(Pitch S, Pitch A, Pitch T, Pitch B)> Pitches { get; } // for convenience

        //accepts these as trusted redundant information:
        public Passage Left { get; } //will be null rather then 'empty'
        public Passage Right { get; } //will be null rather then 'empty'

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


                if (Chords.Count == 1) // Length 1
                {

                    //Bad chords
                    if (Chords[0].Root.ResidueNumber == 5 && Verts[0].Voicing.B.ResidueNumber == 2)
                    {
                        tr += 35;
                    }
                    if (Chords[0].Root.ResidueNumber == 1 && Verts[0].Voicing.B.ResidueNumber == 0)
                    {
                        tr += 15;
                    }

                    //Doubling 5th or root in chord 7
                    if (Chords[0].Root.ResidueNumber == 6)
                    {
                        int[] profile = new int[7];
                        profile[Verts[0].Voicing.B.ResidueNumber]++;
                        profile[Verts[0].Voicing.T.ResidueNumber]++;
                        profile[Verts[0].Voicing.A.ResidueNumber]++;
                        profile[Verts[0].Voicing.S.ResidueNumber]++;
                        if (profile[4] >= 2) tr += 60;
                        if (profile[0] >= 2) tr += 60;
                    }

                }


                if (Chords.Count == 2) // Length 2
                {

                    //Voice leading
                    tr += Abs(TrsAr[0].A.Semis) + 2.5 * Max(0, Abs(TrsAr[0].A.Semis) - 4);
                    tr += Abs(TrsAr[0].T.Semis) + 2.5 * Max(0, Abs(TrsAr[0].T.Semis) - 4);
                    tr += 0.5 * Abs(TrsAr[0].B.Semis) + (1.5 * Max(0, Abs(TrsAr[0].B.Semis) - 6)
                        * (Verts[1].Voicing.B.ResidueNumber + Verts[0].Voicing.B.ResidueNumber + 1));
                    double numStatic = 0;
                    if (TrsAr[0].S.Semis == 0) numStatic += 1; //This should be left in!
                    if (TrsAr[0].A.Semis == 0) numStatic += 1;
                    if (TrsAr[0].T.Semis == 0) numStatic += 1.5;
                    if (TrsAr[0].B.Semis == 0) numStatic += 2.5;
                    tr += numStatic * numStatic * 3;

                    //Resolving 7th
                    //Can it resolve several chords later?
                    if (Verts[0].Voicing.S.ResidueNumber == 6 && TrsAr[0].S.Number != -1) tr += 70;
                    if (Verts[0].Voicing.A.ResidueNumber == 6 && TrsAr[0].A.Number != -1) tr += 70;
                    if (Verts[0].Voicing.T.ResidueNumber == 6 && TrsAr[0].T.Number != -1) tr += 70;
                    if (Verts[0].Voicing.B.ResidueNumber == 6 && TrsAr[0].B.Number != -1) tr += 70;

                    //Preparing 7th
                    //Should a 7th be prepared in the previous chord? probably
                    if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) >= 2) tr += 70;
                    if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) >= 2) tr += 70;
                    if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) >= 2) tr += 70;
                    if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) >= 2) tr += 70;
                    if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) == 1) tr += 35;
                    if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) == 1) tr += 35;
                    if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) == 1) tr += 35;
                    if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) == 1) tr += 35;
                    if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(TrsAr[0].S.Number) == 0) tr -= 6;
                    if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(TrsAr[0].A.Number) == 0) tr -= 6;
                    if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(TrsAr[0].T.Number) == 0) tr -= 6;
                    if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(TrsAr[0].B.Number) == 0) tr -= 6;

                    //Movement after dim 7
                    if (Verts[0].Chord.Variety.PQ7 == -2 &&
                        (Pitches[0].B) > (Pitches[1].B)) tr += 15;
                    if (Verts[0].Chord.Variety.PQ7 == -2 &&
                        (Pitches[0].S) < (Pitches[1].S)) tr += 15;

                    //Chord transition
                    if (Chords[1].Root == Chords[0].Root && Verts[1].Voicing.B.Residue == Verts[0].Voicing.B.Residue)
                    {
                        tr += 50;
                    }
                    switch ((Chords[1].Root - Chords[0].Root).ResidueNumber)
                    {
                        case 3: tr += -10; break;
                        case 5: tr += -5; break;
                        case 4: tr += 0; break;
                        case 1: tr += 5; break;
                        case 6: tr += 20; break;
                        case 2: tr += 25; break;
                        case 0: tr += 35; break;
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


                if (Chords.Count == 3) // Length 3
                {

                    //Chord transition 1 to 3
                    if (Chords[2].Root == Chords[0].Root && Verts[2].Voicing.B.Residue == Verts[0].Voicing.B.Residue)
                    {
                        tr += 65;
                    }

                }


                return tr;
            }
        }

        public Passage(IntervalS tonic, ReadOnlyCollection<Vert> verts, Passage left, Passage right) : base()
        {

            Tonic = tonic;
            Verts = verts;

            Chords = Array.AsReadOnly(verts.Select((x) =>
                x.Chord
            ).ToArray());

            Pitches = Array.AsReadOnly(verts.Select((x) => (
                S: new Pitch(Tonic + (x.Chord.Root + x.Voicing.S)),
                A: new Pitch(Tonic + (x.Chord.Root + x.Voicing.A)),
                T: new Pitch(Tonic + (x.Chord.Root + x.Voicing.T)),
                B: new Pitch(Tonic + (x.Chord.Root + x.Voicing.B)))
            ).ToArray());

            Left = left;
            Right = right;

            if (verts.Count == 1)
            {
                AddChildren(verts);
            }
            else if (verts.Count == 2)
            {
                AddChildren(new (double, TreeValued)[] { (1, Left), (1, Right) });
            }
            else
            {
                AddChildren(new (double, TreeValued)[] { (1, Left), (1, Right), (-1, Left.Right) });
            }

        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Passage);
        }

        public bool Equals(Passage other)
        {
            if
            (
                other == null ||
                other.Tonic != Tonic ||
                Verts.Count != other.Verts.Count
            )
                return false;

            for (int i = 0; i < Verts.Count; i++)
                if
                (
                    !Verts[i].Equals(other.Verts[i])
                )
                    return false;

            return true;
        }

        //may want improving
        public override int GetHashCode()
        {
            int hashCode = -1495229413;
            hashCode = hashCode * -1521134295 + Tonic.GetHashCode();
            hashCode = hashCode * -1521134295 + Verts.GetHashCode();
            return hashCode;
        }
    }

}
