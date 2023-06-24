namespace WordStats
{
    public interface IWordStatsWriter
    {
        void WriteStats(IWordStats stats);
    }
}