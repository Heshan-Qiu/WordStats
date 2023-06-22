using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordStats
{
    public interface IWordStatsWriter
    {
        void WriteStats(IWordStats stats);
    }
}