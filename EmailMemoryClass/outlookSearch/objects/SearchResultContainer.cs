using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMemoryClass.outlookSearch
{
    public class SearchResultContainer
    {
        public List<SearchResult> Results;

        public SearchResultContainer(List<SearchResult> result)
        {
            Results = result;
        }
    }
}
