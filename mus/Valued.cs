using System.Collections.Generic;
using System.Linq;

namespace mus
{
    public static partial class notation
    {

        class Valuer<T> : IComparer<T> where T : Valued
        {
            public int Compare(T x, T y)
            {
                return Comparer<double>.Default.Compare(x.Penalty, y.Penalty);
            }

            private Valuer() { }

            public readonly static Valuer<T> instance = new Valuer<T>();
        }

        public abstract class Valued
        {

            public virtual double Penalty { get; }

            protected Valued(Valued[] children)
            {
                Penalty = children.Aggregate(0D, (x, y) => x + y.Penalty);
            }
        }

    }
}
