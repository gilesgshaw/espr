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

            //temporary routine to help refactoring
            private Passage TruncatedBy(int left_, int right)
            {
                if (left_ > 0) return  Left.TruncatedBy(left_ - 1, right);
                if (right > 0) return Right.TruncatedBy(left_, right - 1);
                return this;
            }

            //public IntervalC[][] Pitches { get; } //satb


            #region temporary measure, while 'IntrinsicPenalty' actually returns penaulty intrinsic to all subpassages

            private double VeryIntrinsicPenalty
            {
                get
                {
                    var tr = 0;


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

                    }


                    if (Chords.Length == 2) // Length 2
                    {

                        //Voice leading
                        var RootChange = Verts[1].Chord.Root.ResidueSemis - Verts[0].Chord.Root.ResidueSemis;
                        var SChange = Verts[1].Voicing.S.Semis - Verts[0].Voicing.S.Semis;
                        var AChange = Verts[1].Voicing.A.Semis - Verts[0].Voicing.A.Semis;
                        var TChange = Verts[1].Voicing.T.Semis - Verts[0].Voicing.T.Semis;
                        var BChange = Verts[1].Voicing.B.Semis - Verts[0].Voicing.B.Semis;
                        tr += Abs(RootChange + SChange);
                        tr += Abs(RootChange + AChange);
                        tr += Abs(RootChange + TChange);
                        tr += Abs(RootChange + BChange);
                        int numStatic = 0;
                        if (SChange == -RootChange) numStatic += 1;
                        if (AChange == -RootChange) numStatic += 1;
                        if (TChange == -RootChange) numStatic += 1;
                        if (BChange == -RootChange) numStatic += 3;
                        tr += numStatic * 4;

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
                            case 1: tr += 3; break;
                            case 6: tr += 15; break;
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

            public override double IntrinsicPenalty
            {
                get
                {
                    var tr = base.IntrinsicPenalty;
                    for (int startpos = 0; startpos < Verts.Length; startpos++)
                    {
                        for (int endpos = startpos; endpos < Verts.Length; endpos++)
                        {
                            tr += TruncatedBy(startpos, Verts.Length - endpos - 1).VeryIntrinsicPenalty;
                        }
                    }
                    return tr;
                }
            }

            #endregion


            //private static Cadence GetCadence(Vert[] verts)
            //{
            //    Cadence Cadence = default;
            //    if (verts.Length >= 3) Cadence = new Cadence(verts[verts.Length - 3], verts[verts.Length - 2], verts[verts.Length - 1]);
            //    if (verts.Length == 2) Cadence = new Cadence(default, verts[verts.Length - 2], verts[verts.Length - 1]);
            //    if (verts.Length == 1) Cadence = new Cadence(default, default, verts[verts.Length - 1]);
            //    if (verts.Length == 0) Cadence = new Cadence(default, default, default);
            //    return Cadence;
            //}

            public Passage(IntervalS tonic, Vert[] verts, Passage left, Passage right) : base(verts) //.Concat(new TreeValued[] { GetCadence(verts) }))
            {
                Tonic = tonic;
                Chords = Array.ConvertAll(verts, (x) => x.Chord);
                Verts = verts;
                Left = left;
                Right = right;
                //Cadence = GetCadence(verts);
                //as a temporary measure, subpassages are not officially registered as children.
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
