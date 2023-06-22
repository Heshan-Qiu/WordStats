using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace WordStats
{
    public class WordStatsService : BackgroundService
    {
        private readonly ILogger<WordStatsService> _logger;

        private readonly Stream _stream;

        private readonly Encoding _encoding;

        private readonly IWordStats _stats;

        private readonly IWordStatsWriter _writer;

        private readonly WordStatsServiceOptions _options;

        public WordStatsService(Stream stream, Encoding encoding, IWordStats stats, IWordStatsWriter writer,
            IOptions<WordStatsServiceOptions> options, ILogger<WordStatsService> logger)
        {
            _stream = stream;
            _encoding = encoding;
            _stats = stats;
            _writer = writer;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("WordStatsService is running at: {time}", DateTimeOffset.Now);

                var buffer = ReadStream();
                var text = ConvertStreamToString(buffer);

                GetWordStats(text);
                GetCharacterStats(text);

                _writer.WriteStats(_stats);

                await Task.Delay(_options.Delay, stoppingToken);
            }
        }

        private byte[]? ReadStream()
        {
            if (_stream == null || _stream.CanRead == false)
                return null;

            var buffer = new byte[1024];
            _stream.Read(buffer);

            return buffer;
        }

        private string? ConvertStreamToString(byte[]? buffer)
        {
            if (buffer == null)
                return null;

            return _encoding.GetString(buffer);
        }

        private void GetWordStats(string? text)
        {
            if (text == null)
                return;

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim())
                .GroupBy(w => w)
                .ToDictionary(g => g.Key, g => g.Count());
            _stats.AddWords(words);

            var characters = text.Replace(" ", "").ToCharArray()
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());
            _stats.AddCharacters(characters);
        }

        private void GetCharacterStats(string? text)
        {
            if (text == null)
                return;

            var characters = text.Replace(" ", "").ToCharArray()
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());
            _stats.AddCharacters(characters);
        }
    }
}