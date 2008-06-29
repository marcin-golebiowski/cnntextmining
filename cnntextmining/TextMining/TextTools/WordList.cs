using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.TextTools
{
    class WordList
    {
        public List<string> getWordList(string pureText)
        {
            List<string> words = new List<string>();
            string[] wordstemp = pureText.Split(' ');
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
            return words;
        }

    }
}
