using System;
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


            #region Static

            //not yet implemented tolerence
            public static IEnumerable<Passage> GetSingletons(Situation situation, double[] tolerence)
            {
                return Array.ConvertAll(situation.Context.Bank[situation.Sop[0]], (x) => new Passage(situation.Context.Tonic, new Vert[] { x }, null, null));
            }

            //not yet implemented tolerence
            public static IEnumerable<Passage> GetPairs(Situation situation, double[] tolerence)
            {
                if (situation.Displacement == 0)
                {
                    foreach (var final in situation.Context.Bank[situation.Sop[1]].Where((x) => x.Chord.Root.ResidueNumber == 0 && x.Voicing.B.ResidueNumber == 0))
                    {
                        foreach (var pp in situation.Context.Bank[situation.Sop[0]].Where((x) => x.Voicing.B.ResidueNumber == 0))
                        {
                            switch (pp.Chord.Root.ResidueNumber)
                            {
                                case 4:
                                    yield return new Passage(
                                        situation.Context.Tonic,
                                        new Vert[] { pp, final },
                                        new Passage(situation.Context.Tonic, new Vert[] { pp }, null, null),
                                        new Passage(situation.Context.Tonic, new Vert[] { final }, null, null)
                                        );
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var final in situation.Context.Bank[situation.Sop[1]])
                    {
                        foreach (var pp in situation.Context.Bank[situation.Sop[0]])
                        {
                            yield return new Passage(
                                situation.Context.Tonic,
                                new Vert[] { pp, final },
                                new Passage(situation.Context.Tonic, new Vert[] { pp }, null, null),
                                new Passage(situation.Context.Tonic, new Vert[] { final }, null, null)
                                );
                        }
                    }
                }
            }

            //at least 1
            public static Dictionary<Situation, List<Passage>> Cache = new Dictionary<Situation, List<Passage>>();

            //at least 1
            public static IEnumerable<Passage> GetExterenal(Situation situation, double[] tolerence)
            {
                if (situation.Sop.Length == 1)
                {
                    //exactly 1
                    if (!Cache.ContainsKey(situation)) Cache.Add(situation, new List<Passage>(GetSingletons(situation, tolerence)));
                }
                else if (situation.Sop.Length == 2)
                {
                    //exactly 2
                    if (!Cache.ContainsKey(situation)) Cache.Add(situation, new List<Passage>(GetPairs(situation, tolerence)));
                }
                else
                {
                    //at least 3
                    AddExterenal(situation, tolerence);
                }
                foreach (var item in Cache[situation])
                {
                    yield return item;
                }
            }

            //at least 3
            //tolerence dosen't matter if already present.
            public static void AddExterenal(Situation situation, double[] tolerence)
            {
                if (Cache.ContainsKey(situation)) return;

                var list = new List<Passage>();
                Cache.Add(situation, list);
                foreach (var l in GetExterenal(situation.Left, tolerence))
                {
                    foreach (var r in GetExterenal(situation.Right, tolerence).Where((x) => x.Left == l.Right))
                    {
                        var obj = new Passage(l.Tonic, l.Verts.Concat(new Vert[] { r.Verts.Last() }).ToArray(), l, r);
                        if (obj.Penalty <= tolerence[obj.Verts.Length]) list.Add(obj);
                    }
                }
            }

            #endregion


            public IntervalS Tonic { get; }
            public Vert[] Verts { get; } //length at least 1
            public Chord[] Chords { get; } //length at least 1

            //public Cadence Cadence { get; }

            //accepts these as trusted redundant information.

            public Passage Left { get; } //will be null rather then 'empty'
            public Passage Right { get; } //will be null rather then 'empty'

            //public IntervalC[][] Pitches { get; } //satb

            //curently takes account that L/R are children (down to singletons)
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
                if (Left != null) AddChildren(new TreeValued[] { Left, Right }); //if 2+, register children.
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
