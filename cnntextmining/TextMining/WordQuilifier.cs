using System.Collections.Generic;
using System.IO;

namespace TextMining
{
    public class WordQuilifier
    {
        static Dictionary<string, string> stopWords = new Dictionary<string, string>();

        public static bool WordIsOK(string word)
        {
            if (stopWords.Count == 0)
            {
                string[] words = File.ReadAllLines("data/stopWords.txt");

                foreach (string w in words)
                {
                    stopWords[w] = "";
                }
            }

            if (word.Length < 3)
            {
                return false;
            }

            if (word == "the")
            {
                
            }

            if (stopWords.ContainsKey(word))
            {
                return false;
            }

            return true;
        }
    }
}