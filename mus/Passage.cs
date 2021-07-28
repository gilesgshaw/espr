﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace mus
{
    public static partial class notation
    {

        //could do with some optimisation...
        //for later should maybe store the pitches directly...
        //and in fact they are probably the 'independant' data.

        public class Passage : TreeValued, IEquatable<Passage>
        {
            public IntervalS Tonic { get; }
            public Vert[] Verts { get; } //length at least 1
            public Chord[] Chords { get; } //length at least 1

            //public Cadence Cadence { get; }

            //accepts these as trusted redundant information.

            public Passage Left { get; } //will be null rather then 'empty'
            public Passage Right { get; } //will be null rather then 'empty'

            //public IntervalC[][] Pitches { get; } //satb

            public override double IntrinsicPenalty
            {
                get
                {
                    var tr = base.IntrinsicPenalty;


                    if (Chords.Length == 1) // Length 1
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


                    if (Chords.Length == 2) // Length 2
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
                        if (SChange == -RootChange) numStatic += 1;
                        if (AChange == -RootChange) numStatic += 1;
                        if (TChange == -RootChange) numStatic += 1.5;
                        if (BChange == -RootChange) numStatic += 2.5;
                        tr += numStatic * numStatic * 3;

                        //Resolving 7th
                        if (Verts[0].Voicing.S.ResidueNumber == 6 && ((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.A.ResidueNumber == 6 && ((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.T.ResidueNumber == 6 && ((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number != 1) tr += 70;
                        if (Verts[0].Voicing.B.ResidueNumber == 6 && ((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number != 1) tr += 70;

                        //Preparing 7th
                        if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number) > 1) tr += 70;
                        if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number) > 1) tr += 70;
                        if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number) > 1) tr += 70;
                        if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number) > 1) tr += 70;
                        if (Verts[1].Voicing.S.ResidueNumber == 6 && Abs(((Verts[0].Voicing.S + Verts[0].Chord.Root) - (Verts[1].Voicing.S + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.A.ResidueNumber == 6 && Abs(((Verts[0].Voicing.A + Verts[0].Chord.Root) - (Verts[1].Voicing.A + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.T.ResidueNumber == 6 && Abs(((Verts[0].Voicing.T + Verts[0].Chord.Root) - (Verts[1].Voicing.T + Verts[1].Chord.Root)).Number) == 0) tr -= 6;
                        if (Verts[1].Voicing.B.ResidueNumber == 6 && Abs(((Verts[0].Voicing.B + Verts[0].Chord.Root) - (Verts[1].Voicing.B + Verts[1].Chord.Root)).Number) == 0) tr -= 6;

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


                    if (Chords.Length == 3) // Length 3
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

            //private static Cadence GetCadence(Vert[] verts)
            //{
            //    Cadence Cadence = default;
            //    if (verts.Length >= 3) Cadence = new Cadence(verts[verts.Length - 3], verts[verts.Length - 2], verts[verts.Length - 1]);
            //    if (verts.Length == 2) Cadence = new Cadence(default, verts[verts.Length - 2], verts[verts.Length - 1]);
            //    if (verts.Length == 1) Cadence = new Cadence(default, default, verts[verts.Length - 1]);
            //    if (verts.Length == 0) Cadence = new Cadence(default, default, default);
            //    return Cadence;
            //}

            public Passage(IntervalS tonic, Vert[] verts, Passage left, Passage right) : base() //.Concat(new TreeValued[] { GetCadence(verts) }))
            {
                Tonic = tonic;
                Chords = Array.ConvertAll(verts, (x) => x.Chord);
                Verts = verts;
                Left = left;
                Right = right;
                //Cadence = GetCadence(verts);
                if (verts.Length == 1)
                {
                    AddChildren(verts);
                }
                else if (verts.Length == 2)
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
                if (other == null || other.Tonic != Tonic || Verts.Length != other.Verts.Length) return false;
                for (int i = 0; i < Verts.Length; i++)
                {
                    if (!ReferenceEquals(Verts[i], other.Verts[i])) return false;
                }
                return true;
            }

            //may want improving
            public override int GetHashCode()
            {
                return -1971104453 + EqualityComparer<Vert[]>.Default.GetHashCode(Verts);
            }

            public static bool operator ==(Passage left, Passage right)
            {
                return EqualityComparer<Passage>.Default.Equals(left, right);
            }

            public static bool operator !=(Passage left, Passage right)
            {
                return !(left == right);
            }
        }

    }
}
