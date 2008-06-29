using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.TextTools
{
    public class WordsSetter
    {
        public static List<News> ComputeWords(List<News> news)
        {
            foreach (News info in news)
            {
                List<string> words = new List<string>();
                string[] wordstemp = info.rawData.Split(' ');
                for (int i = 0; i < wordstemp.Length; i++)
                {
                    // normalization 
                    string word = wordstemp[i].ToLower().Trim();

                    // steaming
                    word = Stemmer.DoPorterStemming(word);

                    if (WordQuilifier.WordIsOK(word))
                    {
                        words.Add(word);
                    }
                }

                info.words = words;


            }

            return news;
        }
    }
}
