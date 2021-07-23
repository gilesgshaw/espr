//this class currently dosen't do anything
//and was in fact used spuriously, in every passage object

//namespace mus
//{
//    public static partial class notation
//    {

//        //as currently using, fields may be null.
//        public class Cadence : TreeValued
//        {
//            public Vert Antepenultimate { get; }
//            public Vert Penultimate { get; }
//            public Vert Ultimate { get; }

//            public override double IntrinsicPenalty
//            {
//                get
//                {
//                    var tr = base.IntrinsicPenalty;
//                    if (Antepenultimate != null &&
//                        Antepenultimate.Chord.Root.ResidueNumber == 1 && Antepenultimate.Voicing.B.ResidueNumber == 4 &&
//                        Penultimate.Chord.Root.ResidueNumber == 4 && Penultimate.Voicing.B.ResidueNumber == 1 &&
//                        Ultimate.Chord.Root.ResidueNumber == 1 && Ultimate.Voicing.B.ResidueNumber == 1)
//                    {
//                        tr -= 0;
//                    }
//                    return tr;
//                }
//            }

//            public Cadence(Vert antepenultimate, Vert penultimate, Vert ultimate) : base()
//            {
//                Antepenultimate = antepenultimate;
//                Penultimate = penultimate;
//                Ultimate = ultimate;
//            }
//        }

//    }
//}
