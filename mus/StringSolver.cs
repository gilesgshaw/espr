using System.Collections.Generic;
using System.Linq;

namespace mus
{

    //thread very-unsafe at the moment.
    public abstract class StringSolver<TProblem, TSolution>
    {
        protected abstract IEnumerable<TSolution> SolveSingleton(TProblem problem);
        protected abstract TProblem Left(TProblem parent); //should return null if singleton
        protected abstract TProblem Right(TProblem parent); //should return null if singleton
        protected abstract IEnumerable<(TSolution, T)> Refine<T>(TProblem problem, (TSolution, T)[] solutions);
        protected abstract bool Combine(TSolution left, TSolution right, out TSolution full); //is only called with 'compatable' arguments

        #region Registry

        //solutions must be registerd by 1) problem 2) problem & lChild 3) problem & rChild
        private readonly List<TSolution> allSolutions;
        private readonly Dictionary<TProblem, List<int>> solutionsTo; // keys present iff 'marked as attempted'
        private readonly Dictionary<(int, TProblem), List<int>> lParentsOf; // don't assume certain keys are present
        private readonly Dictionary<(int, TProblem), List<int>> rParentsOf; // don't assume certain keys are present

        //for singletons
        //'mark as attempted' before calling
        private void Register(TProblem problem, TSolution solution)
        {
            var index = allSolutions.Count;
            solutionsTo[problem].Add(index);
            allSolutions.Add(solution);

            //HERE to help with debugging, keep track of solutions with no parents (1 of 3)
            //lParentsOf.Add((index, default), new List<int>());
            //rParentsOf.Add((index, default), new List<int>());
        }

        //for others
        //'mark as attempted' before calling
        private void Register(TProblem problem, TSolution solution, int lChild, int rChild)
        {
            var index = allSolutions.Count;
            solutionsTo[problem].Add(index);
            if (!lParentsOf.ContainsKey((rChild, problem))) lParentsOf.Add((rChild, problem), new List<int>());
            lParentsOf[(rChild, problem)].Add(index);
            if (!rParentsOf.ContainsKey((lChild, problem))) rParentsOf.Add((lChild, problem), new List<int>());
            rParentsOf[(lChild, problem)].Add(index);
            allSolutions.Add(solution);

            //HERE to help with debugging, keep track of solutions with no parents (2 of 3)
            //lParentsOf.Add((index, default), new List<int>());
            //rParentsOf.Add((index, default), new List<int>());
        }

        #endregion

        public void Ensure(TProblem problem)
        {
            if (solutionsTo.ContainsKey(problem)) return;
            if (Left(problem) != null)
            {
                Ensure(Left(problem));
                Ensure(Right(problem));
            }
            SolveInternal(problem);
        }

        public IEnumerable<TSolution> Solve(TProblem problem)
        {
            Ensure(problem);
            return solutionsTo[problem].Select((x) => allSolutions[x]);
        }

        #region Internal

        //children must be solved already
        private void SolveInternal(TProblem problem)
        {
            solutionsTo.Add(problem, new List<int>());
            if (Left(problem) == null)
            {
                var results = GetInternalS(problem);
                foreach (var solution in Refine(problem, results.Select((x) => (x, (object)null)).ToArray()))
                {
                    Register(problem, solution.Item1);
                }
            }
            else
            {
                var results = GetInternalM(problem);
                foreach (var solution in Refine(problem, results.Select((x) => (x.Item1, x)).ToArray()))
                {
                    //HERE to help with debugging, keep track of solutions with no parents (3 of 3)
                    //if (lParentsOf.ContainsKey((solution.Item2.Item3, default))) lParentsOf.Remove((solution.Item2.Item3, default));
                    //if (rParentsOf.ContainsKey((solution.Item2.Item2, default))) rParentsOf.Remove((solution.Item2.Item2, default));

                    Register(problem, solution.Item1, solution.Item2.Item2, solution.Item2.Item3);
                }
            }
        }

        //children must be solved already
        private IEnumerable<(TSolution, int, int)> GetInternalM(TProblem problem)
        {
            TSolution tempSol = default;
            if (Right(Left(problem)) == null)
            {
                foreach (var left in solutionsTo[Left(problem)])
                {
                    foreach (var right in solutionsTo[Right(problem)])
                    {
                        if (Combine(allSolutions[left], allSolutions[right], out tempSol))
                            yield return (tempSol, left, right);
                    }
                }
            }
            else
            {
                foreach (var child in solutionsTo[Right(Left(problem))]
                    .Where((x) => lParentsOf.ContainsKey((x, Left(problem))) && rParentsOf.ContainsKey((x, Right(problem)))))
                {
                    var l = lParentsOf[(child, Left(problem))];
                    var r = rParentsOf[(child, Right(problem))];
                    foreach (var left in l)
                    {
                        foreach (var right in r)
                        {
                            if (Combine(allSolutions[left], allSolutions[right], out tempSol))
                                yield return (tempSol, left, right);
                        }
                    }
                }
            }
        }

        private IEnumerable<TSolution> GetInternalS(TProblem problem)
        {
            foreach (var solution in SolveSingleton(problem))
            {
                yield return solution;
            }
        }

        #endregion

        protected StringSolver(IEqualityComparer<TProblem> comparer)
        {
            allSolutions = new List<TSolution>();
            solutionsTo = new Dictionary<TProblem, List<int>>(comparer);
            var wcomparer = Equate.When<(int, TProblem)>(
                (x, y) => x.Item1 == y.Item1 && comparer.Equals(x.Item2, y.Item2),
                (x) => 0x6B89D32A + x.Item1 * 0x45555529 + comparer.GetHashCode(x.Item2));
            lParentsOf = new Dictionary<(int, TProblem), List<int>>(wcomparer);
            rParentsOf = new Dictionary<(int, TProblem), List<int>>(wcomparer);
        }

        protected StringSolver() : this(EqualityComparer<TProblem>.Default) { }
    }

}
