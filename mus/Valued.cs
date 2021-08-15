using System;
using System.Collections.Generic;
using System.Linq;

namespace mus.Gen
{

    class Valuer<T> : IComparer<T> where T : IValuable
    {
        public int Compare(T x, T y)
        {
            return Comparer<double>.Default.Compare(x.Penalty, y.Penalty);
        }

        private Valuer() { }

        public readonly static Valuer<T> instance = new Valuer<T>();
    }

    public interface IValuable
    {
        double Penalty { get; }
    }

    //In current design, the same child object is counted every time it appears
    //So this whole 'serial' thing is basically redundant,
    //since equality comparison should not be needed
    // However I'm leaving all this stuff here in case it becomes useful later.
    public abstract class TreeValued : IValuable, IEquatable<TreeValued>
    {

        private IEnumerable<(double, TreeValued)> Children { get; set; }

        public virtual double IntrinsicPenalty { get; }

        public virtual double TemporaryPenalty { get; }

        public double Penalty
        {
            get
            {
                //HERE: check this is calculated correctly.
                if (iPenalty == null) iPenalty = Children.Aggregate(IntrinsicPenalty + TemporaryPenalty, (x, y) => x + y.Item1 * y.Item2.ResidualPenalty);
                return iPenalty.Value;
            }
        }

        private double? iPenalty { get; set; }

        public double ResidualPenalty
        {
            get
            {
                //HERE: check this is calculated correctly.
                if (iResidualPenalty == null) iResidualPenalty = Children.Aggregate(IntrinsicPenalty, (x, y) => x + y.Item1 * y.Item2.ResidualPenalty);
                return iResidualPenalty.Value;
            }
        }

        private double? iResidualPenalty { get; set; }



        private static int NextSerial = 0;
        private int Serial { get; }


        protected void AddChildren(IEnumerable<TreeValued> additional)
        {
            Children = Children.Concat(additional.Select((x) => (1D, x)));
        }

        protected void AddChildren(IEnumerable<(double, TreeValued)> additional)
        {
            Children = Children.Concat(additional);
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
        }

        protected TreeValued()
        {
            Serial = NextSerial;
            NextSerial++;
            IntrinsicPenalty = 0;
            TemporaryPenalty = 0;
            Children = Enumerable.Empty<(double, TreeValued)>();
        }

        protected TreeValued(double intrinticPenalty, double temporaryPenalty)
        {
            Serial = NextSerial;
            NextSerial++;
            IntrinsicPenalty = intrinticPenalty;
            TemporaryPenalty = temporaryPenalty;
            Children = Enumerable.Empty<(double, TreeValued)>();
        }

        protected TreeValued(double intrinticPenalty)
        {
            Serial = NextSerial;
            NextSerial++;
            IntrinsicPenalty = intrinticPenalty;
            TemporaryPenalty = 0;
            Children = Enumerable.Empty<(double, TreeValued)>();
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
