using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace mus.Chorale
{

    // immutable
    // no comparisons / hash code implemented
    public class Climate
    {
        public ReadOnlyCollection<Context> Contexts { get; }
        public Context Home { get; } // this should also be included in the above.
        public ReadOnlyDictionary<(Context, Context), double> Metric { get; }
        // should contain all ordered (distinct) pairs referenced here
        // but may contain others, which should be ignored

        // wraps relevent parameters
        public Climate(IEnumerable<Context> contexts, Context home, IDictionary<(Context, Context), double> metric)
        {
            Contexts = Array.AsReadOnly(contexts.ToArray());
            Home = home;
            Metric = new ReadOnlyDictionary<(Context, Context), double>(metric);
        }
    }

}
