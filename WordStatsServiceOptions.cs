using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordStats
{
    public class WordStatsServiceOptions
    {
        public int ReadDelay { get; set; } = 1000;
        public int WriteDelay { get; set; } = 1000;
    }
}