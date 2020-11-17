using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMemoryClass.Configuration
{
    public class ApplicationConfiguration
    {
        public bool MinimiseOnStart { get; set; } = false;

        public bool StopSearchIfOutlookNotRunning { get; set; } = true;

        public ApplicationConfiguration()
        {

        }
    }
}
