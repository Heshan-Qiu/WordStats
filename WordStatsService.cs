using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordStats
{
    public class WordStatsService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var wordStats = await GetWordStatsAsync();
                Console.WriteLine($"Word stats: {wordStats}");
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<string> GetWordStatsAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://www.gutenberg.org/files/1342/1342-0.txt");
            var content = await response.Content.ReadAsStringAsync();
            var words = content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var wordStats = words.GroupBy(w => w)
                .Select(g => new { Word = g.Key, Count = g.Count() })
                .OrderByDescending(w => w.Count)
                .Take(10)
                .Select(w => $"{w.Word} ({w.Count})");
            return string.Join(", ", wordStats);
        }
    }
}