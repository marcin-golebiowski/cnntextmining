using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace TextMining.TextTools
{
    /// <summary>
    /// 
    /// </summary>
    public class WordQuilifier
    {
        static readonly Dictionary<string, string> stopWords = new Dictionary<string, string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool WordIsOK(string word)
        {
            if (stopWords.Count == 0)
            {
                string[] words = File.ReadAllLines(Path.GetFullPath(ConfigurationManager.AppSettings["stopWords"]));

                foreach (string w in words)
                {
                    stopWords[w] = "";
                }
            }

            if (word.Length < 3)
            {
                return false;
            }

            if (stopWords.ContainsKey(word))
            {
                return false;
            }

            return true;
        }
    }
}