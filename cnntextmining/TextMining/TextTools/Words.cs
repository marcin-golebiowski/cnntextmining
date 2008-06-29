using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.TextTools
{
    public class Words
    {
        public static List<News> ComputeWords(List<News> news)
        {
            foreach (News info in news)
            {
                var words = new List<string>();
                Filter(info);


                string[] wordstemp = info.rawData.Split(' ');
                for (int i = 0; i < wordstemp.Length; i++)
                {
                    // normalization 
                    string word = wordstemp[i].ToLower().Trim();
                    if (WordQuilifier.WordIsOK(word))
                    {
                        // steaming
                        word = Stemmer.DoPorterStemming(word);

                        if (word.Length > 3)
                        {
                            words.Add(word);
                        }
                    }
                }

                info.words = words;


            }

            return news;
        }

        private static void Filter(News info)
        {
            string[] toRemove = new string[] { "\n", "\t", ".", ",", "\"", "&quot;",  ";", "?", "�", "'" };

            foreach (string s in toRemove)
            {
                info.rawData = info.rawData.Replace(s, " ");
            }
        }
    }
}
