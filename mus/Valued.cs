using System;
using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        class Valuer<T> : IComparer<T> where T : Valuable
        {
            public int Compare(T x, T y)
            {
                return Comparer<double>.Default.Compare(x.Penalty, y.Penalty);
            }

            private Valuer() { }

            public readonly static Valuer<T> instance = new Valuer<T>();
        }

        public interface Valuable
        {
            double Penalty { get; }
        }

        public abstract class Valued : Valuable
        {

            public virtual double Penalty { get; }

            protected Valued()
            {
                Penalty = 0;
            }

            protected Valued(Valued[] children)
            {
                Penalty = children.Aggregate(0D, (x, y) => x + y.Penalty);
            }

            protected Valued(double penalty)
            {
                Penalty = penalty;
            }

            protected Valued(Valued[] children, double penalty)
            {
                Penalty = children.Aggregate(penalty, (x, y) => x + y.Penalty);
            }
        }


        //Is actually a directed acyclic graph
        public abstract class TreeValued : Valuable, IEquatable<TreeValued>
        {

            public IEnumerable<(double, TreeValued)> Children { get; private set; }

            public IEnumerable<(double, TreeValued)> Decendants { get; private set; }

            public virtual double IntrinsicPenalty { get; }

            public virtual double TemporaryPenalty { get; }

            public double Penalty
            {
                get
                {
                    //HERE: check this is calculated correctly.
                    if (iPenalty == null) iPenalty = Decendants.Aggregate(IntrinsicPenalty + TemporaryPenalty, (x, y) => x + y.Item1 * y.Item2.IntrinsicPenalty);
                    return iPenalty.Value;
                }
            }

            private double? iPenalty { get; set; }

            //HERE: check this is calculated correctly.
            public double ResidualPenalty { get => Decendants.Aggregate(IntrinsicPenalty, (x, y) => x + y.Item1 * y.Item2.IntrinsicPenalty); }



            private static int NextSerial = 0;
            private int Serial { get; }


            protected void AddChildren(IEnumerable<TreeValued> additional)
            {
                Children = Children.Concat(additional.Select((x) => (1D, x)));
                ComputeDecendants();
            }

            protected void AddChildren(IEnumerable<(double, TreeValued)> additional)
            {
                Children = Children.Concat(additional);
                ComputeDecendants();
            }


            //HERE: check this is calculated correctly.
            private void ComputeDecendants()
            {
                Decendants = Children.SelectMany((x) => x.Item2.Decendants.Select((y) => (y.Item1 * x.Item1, y.Item2))).Concat(Children).Distinct();
            }


            protected TreeValued(IEnumerable<TreeValued> children, double intrinticPenalty, double temporaryPenalty)
                : this(children.Select((x) => (1D, x)), intrinticPenalty, temporaryPenalty) { }

            protected TreeValued(IEnumerable<(double, TreeValued)> children, double intrinticPenalty, double temporaryPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = temporaryPenalty;
                Children = children;
                ComputeDecendants();
            }

            protected TreeValued(IEnumerable<TreeValued> children, double intrinticPenalty)
                : this(children.Select((x) => (1D, x)), intrinticPenalty) { }

            protected TreeValued(IEnumerable<(double, TreeValued)> children, double intrinticPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = 0;
                Children = children;
                ComputeDecendants();
            }

            protected TreeValued()
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = 0;
                TemporaryPenalty = 0;
                Children = Enumerable.Empty<(double, TreeValued)>();
                ComputeDecendants();
            }

            protected TreeValued(double intrinticPenalty, double temporaryPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = temporaryPenalty;
                Children = Enumerable.Empty<(double, TreeValued)>();
                ComputeDecendants();
            }

            protected TreeValued(double intrinticPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = 0;
                Children = Enumerable.Empty<(double, TreeValued)>();
                ComputeDecendants();
            }

            protected TreeValued(IEnumerable<TreeValued> children)
                : this(children.Select((x) => (1D, x))) { }

            protected TreeValued(IEnumerable<(double, TreeValued)> children)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = 0;
                TemporaryPenalty = 0;
                Children = children;
                ComputeDecendants();
            }



            public override bool Equals(object obj)
            {
                return Equals(obj as TreeValued);
            }

            public bool Equals(TreeValued other)
            {
                return other != null &&
                       Serial == other.Serial;
            }

            public override int GetHashCode()
            {
                return Serial;
            }
        }

    }
}
