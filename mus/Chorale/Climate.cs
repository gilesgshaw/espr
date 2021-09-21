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

        public Climate(IEnumerable<Context> contexts, Context home)
        {
            Contexts = Array.AsReadOnly(contexts.ToArray());
            Home = home;
        }
    }

}
