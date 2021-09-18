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

        public Climate(IEnumerable<Context> contexts)
        {
            Contexts = Array.AsReadOnly(contexts.ToArray());
        }
    }

}
