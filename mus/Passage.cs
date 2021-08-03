using System;
using System.Collections.ObjectModel;
using System.Linq;
using static System.Math;

namespace mus
{
    public static partial class notation
    {

        // immutable, provided 'Chord' and 'Vert' are
        // currently vunerable to invalid inputs
        public class Passage : TreeValued, IEquatable<Passage>
        {
            public IntervalS Tonic { get; }
            public ReadOnlyCollection<Vert> Verts { get; } //length at least 1
            public ReadOnlyCollection<Chord> Chords { get; } //length at least 1

            //accepts these as trusted redundant information:
            public Passage Left { get; } //will be null rather then 'empty'
            public Passage Right { get; } //will be null rather then 'empty'

            public override double IntrinsicPenalty
            {
                get
                {
                    var tr = base.IntrinsicPenalty;


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
                        var RootChange = Verts[1].Chord.Root.ResidueSemis - Verts[0].Chord.Root.ResidueSemis;
                        var SChange = Verts[1].Voicing.S.Semis - Verts[0].Voicing.S.Semis;
                        var AChange = Verts[1].Voicing.A.Semis - Verts[0].Voicing.A.Semis;
                        var TChange = Verts[1].Voicing.T.Semis - Verts[0].Voicing.T.Semis;
                        var BChange = Verts[1].Voicing.B.Semis - Verts[0].Voicing.B.Semis;
                        tr += Abs(RootChange + AChange) + 2.5 * Max(0, Abs(RootChange + AChange) - 4);
                        tr += Abs(RootChange + TChange) + 2.5 * Max(0, Abs(RootChange + TChange) - 4);
                        tr += 0.5 * Abs(RootChange + BChange) + 1.5 * Max(0, Abs(RootChange + BChange) - 6) * (Verts[1].Voicing.B.ResidueNumber + Verts[0].Voicing.B.ResidueNumber + 1);
                        double numStatic = 0;
                        if (SChange == -RootChange) numStatic += 1; //This should be left in!
                        if (AChange == -RootChange) numStatic += 1;
                        if (TChange == -RootChange) numStatic += 1.5;
                        if (BChange == -RootChange) numStatic += 2.5;
                        tr += numStatic * numStatic * 3;

                        //Resolving 7th
                        //Can it resolve several chords later?
                        if (Verts[0].Voicing.S.ResidueNumber == 6 && ((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.A.ResidueNumber == 6 && ((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.T.ResidueNumber == 6 && ((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.B.ResidueNumber == 6 && ((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number != 1) tr += 70;

                        //Preparing 7th
                        //Should a 7th be prepared in the previous chord? probably
                        if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number) >= 2) tr += 70;
                        if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number) >= 2) tr += 70;
                        if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number) >= 2) tr += 70;
                        if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number) >= 2) tr += 70;
                        if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number) == 1) tr += 35;
                        if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number) == 1) tr += 35;
                        if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number) == 1) tr += 35;
                        if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number) == 1) tr += 35;
                        if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number) == 0) tr -= 6;

                        //Movement after dim 7
                        if (Verts[0].Chord.Variety.PQ7 == -2 &&
                            (Verts[0].Voicing.B + Verts[0].Chord.Root) > (Verts[1].Voicing.B + Verts[1].Chord.Root)) tr += 15;
                        if (Verts[0].Chord.Variety.PQ7 == -2 &&
                            (Verts[0].Voicing.S + Verts[0].Chord.Root) < (Verts[1].Voicing.S + Verts[1].Chord.Root)) tr += 15;

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
                        var Notes = new (IntervalS, IntervalS)[] {
                            (Verts[0].Voicing.S.Residue + Verts[0].Chord.Root, Verts[1].Voicing.S.Residue + Verts[1].Chord.Root),
                            (Verts[0].Voicing.A.Residue + Verts[0].Chord.Root, Verts[1].Voicing.A.Residue + Verts[1].Chord.Root),
                            (Verts[0].Voicing.T.Residue + Verts[0].Chord.Root, Verts[1].Voicing.T.Residue + Verts[1].Chord.Root),
                            (Verts[0].Voicing.B.Residue + Verts[0].Chord.Root, Verts[1].Voicing.B.Residue + Verts[1].Chord.Root)
                        };
                        var Ints = Array.ConvertAll(Notes, (x) => x.Item2 - x.Item1);
                        for (int i1 = 0; i1 < 4; i1++)
                        {
                            for (int i2 = i1 + 1; i2 < 4; i2++)
                            {
                                if (Ints[i1] == Ints[i2] && Ints[i1] != new IntervalS())
                                {
                                    if (Notes[i1].Item1 - Notes[i2].Item1 == new IntervalS(4, 7) || Notes[i1].Item1 - Notes[i2].Item1 == new IntervalS())
                                    {
                                        tr += 50;
                                    }
                                }
                            }
                        }

                        //Overlapping
                        var AbsNotes = new (IntervalC, IntervalC)[] {
                            (Verts[0].Voicing.S + Verts[0].Chord.Root, Verts[1].Voicing.S + Verts[1].Chord.Root),
                            (Verts[0].Voicing.A + Verts[0].Chord.Root, Verts[1].Voicing.A + Verts[1].Chord.Root),
                            (Verts[0].Voicing.T + Verts[0].Chord.Root, Verts[1].Voicing.T + Verts[1].Chord.Root),
                            (Verts[0].Voicing.B + Verts[0].Chord.Root, Verts[1].Voicing.B + Verts[1].Chord.Root)
                        };
                        for (int i1 = 0; i1 < 3; i1++)
                        {
                            int i2 = i1 + 1;

                            if (AbsNotes[i1].Item1 < AbsNotes[i2].Item2)
                            {
                                tr += 45;
                            }
                            if (AbsNotes[i1].Item2 < AbsNotes[i2].Item1)
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
                Chords = Array.AsReadOnly(verts.Select((x) => x.Chord).ToArray());
                Verts = verts;
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
                if (
                    other == null ||
                    other.Tonic != Tonic ||
                    Verts.Count != other.Verts.Count
                )
                    return false;

                for (int i = 0; i < Verts.Count; i++)
                {
                    if (
                        !Verts[i].Equals(other.Verts[i])
                    )
                        return false;
                }

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
}
