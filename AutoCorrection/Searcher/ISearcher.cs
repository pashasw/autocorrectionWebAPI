using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCorrection.Searcher
{
    interface ISearcher
    {
        HashSet<Int32> Search(String str, out List<string> output);
    }
}
