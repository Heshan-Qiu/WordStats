namespace WordStats
{
    public interface IWordStats
    {
        void AddWord(string word, int count);

        void AddWords(IDictionary<string, int> words);

        void AddCharacter(char character, int count);

        void AddCharacters(IDictionary<char, int> characters);

        IEnumerable<String> GetLargestFiveWords();

        IEnumerable<String> GetSmallestFiveWords();

        IEnumerable<KeyValuePair<string, int>> GetMostFrequentTenWords();

        IEnumerable<KeyValuePair<string, int>> GetWords();

        IEnumerable<KeyValuePair<char, int>> GetCharacters();

        string ToJsonString();
    }
}