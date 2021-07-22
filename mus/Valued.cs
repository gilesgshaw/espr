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

            public IEnumerable<TreeValued> Children { get; private set; }

            public IEnumerable<TreeValued> Decendants { get; private set; }

            public virtual double IntrinsicPenalty { get; }

            public virtual double TemporaryPenalty { get; }

            public double Penalty { get => Decendants.Aggregate(IntrinsicPenalty + TemporaryPenalty, (x, y) => x + y.IntrinsicPenalty); }

            public double ResidualPenalty { get => Decendants.Aggregate(IntrinsicPenalty, (x, y) => x + y.IntrinsicPenalty); }



            private static int NextSerial = 0;
            private int Serial { get; }


            protected void AddChildren(IEnumerable<TreeValued> additional)
            {
                Children = Children.Concat(additional);
                ComputeDecendants();
            }


            private void ComputeDecendants()
            {
                Decendants = Children.SelectMany((x) => x.Decendants).Distinct().Concat(Children);
            }


            protected TreeValued(IEnumerable<TreeValued> children, double intrinticPenalty, double temporaryPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = temporaryPenalty;
                Children = children;
                ComputeDecendants();
            }

            protected TreeValued(IEnumerable<TreeValued> children, double intrinticPenalty)
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
                Children = Enumerable.Empty<TreeValued>();
                ComputeDecendants();
            }

            protected TreeValued(double intrinticPenalty, double temporaryPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = temporaryPenalty;
                Children = Enumerable.Empty<TreeValued>();
                ComputeDecendants();
            }

            protected TreeValued(double intrinticPenalty)
            {
                Serial = NextSerial;
                NextSerial++;
                IntrinsicPenalty = intrinticPenalty;
                TemporaryPenalty = 0;
                Children = Enumerable.Empty<TreeValued>();
                ComputeDecendants();
            }

            protected TreeValued(IEnumerable<TreeValued> children)
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
