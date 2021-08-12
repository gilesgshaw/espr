using System.Collections.Generic;
using System.Linq;

namespace mus.Gen
{

    public abstract class StringValuer<TProblem, TSolution> : StringSolver<TProblem, TSolution> where TSolution : Valuable
    {
        protected override IEnumerable<(TSolution, T)> Refine<T>(TProblem problem, (TSolution, T)[] solutions)
        {
            var permissableSolutions = new List<(TSolution, T)>();
            var tolerence = Tolerence(problem);
            foreach (var solution in solutions)
            {
                if (solution.Item1.Penalty <= tolerence) permissableSolutions.Add(solution);
            }
            permissableSolutions.Sort(Compare.With<(TSolution, T)>((x, y) => x.Item1.Penalty.CompareTo(y.Item1.Penalty)));
            return permissableSolutions.Take(Capacity(problem));
        }

        protected abstract int Capacity(TProblem problem);
        protected abstract double Tolerence(TProblem problem);

        protected StringValuer(IEqualityComparer<TProblem> comparer) : base(comparer) { }
        protected StringValuer() : base() { }
    }

}
