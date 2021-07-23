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
            public IntervalS Tonic { get; }
            public Vert[] Verts { get; }
            public Chord[] Chords { get; }

            //public Cadence Cadence { get; }

            //accepts these as trusted redundant information.

            public Passage Left { get; }
            public Passage Right { get; }

            //public IntervalC[][] Pitches { get; } //satb

            public override double IntrinsicPenalty
            {
                get
                {
                    var tr = base.IntrinsicPenalty;
                    for (int i = 1; i < Chords.Length; i++)
                    {

                        var RootChange = Verts[i].Chord.Root.ResidueSemis - Verts[i - 1].Chord.Root.ResidueSemis;
                        var SChange = Verts[i].Voicing.S.Semis - Verts[i - 1].Voicing.S.Semis;
                        var AChange = Verts[i].Voicing.A.Semis - Verts[i - 1].Voicing.A.Semis;
                        var TChange = Verts[i].Voicing.T.Semis - Verts[i - 1].Voicing.T.Semis;
                        var BChange = Verts[i].Voicing.B.Semis - Verts[i - 1].Voicing.B.Semis;
                        tr += Abs(RootChange + SChange);
                        tr += Abs(RootChange + AChange);
                        tr += Abs(RootChange + TChange);
                        tr += Abs(RootChange + BChange);

                        if (Chords[i].Root == Chords[i - 1].Root && Verts[i].Voicing.B.Residue == Verts[i - 1].Voicing.B.Residue)
                        {
                            tr += 50;
                        }

                        if (Chords[i].Root == Chords[i - 1].Root)
                        {
                            tr += 35;
                        }

                        var Notes = new (IntervalS, IntervalS)[] {
                            (Verts[i - 1].Voicing.S.Residue + Verts[i - 1].Chord.Root, Verts[i].Voicing.S.Residue + Verts[i].Chord.Root),
                            (Verts[i - 1].Voicing.A.Residue + Verts[i - 1].Chord.Root, Verts[i].Voicing.A.Residue + Verts[i].Chord.Root),
                            (Verts[i - 1].Voicing.T.Residue + Verts[i - 1].Chord.Root, Verts[i].Voicing.T.Residue + Verts[i].Chord.Root),
                            (Verts[i - 1].Voicing.B.Residue + Verts[i - 1].Chord.Root, Verts[i].Voicing.B.Residue + Verts[i].Chord.Root)
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
