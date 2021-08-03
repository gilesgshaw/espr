using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static mus.Ut;

namespace mus
{
    public static partial class notation
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
}
