using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.TextTools
{
    class DefaultNewsComparator : INewsComparator
    {
        private readonly WordsStats stats;

        public DefaultNewsComparator(WordsStats stats)
        {
            this.stats = stats;
        }

        
        // Wektor to będzie słownik (word -> itdf)
        // i wtedy jesli będą mieli wspólne słowa do sumujemy to ...

        // 1. szukamy wspólnych słów w dwóch newach
        // 2. 

        public double Compare(News x, News y)
        {
            double result = 0;

            var xWords = new Dictionary<string, bool>();
            var yWords = new Dictionary<string, bool>();


            foreach (string word in x.words)
            {
                xWords[word] = false;
            }

            foreach (string word in y.words)
            {
                yWords[word] = false;
            }

           
            foreach (string word  in xWords.Keys)
            {
                if (yWords.ContainsKey(word))
                {
                    result += (stats.GetDocumentTermFrequency(word, x.url) +
                               stats.GetDocumentTermFrequency(word, y.url)
                               ) * stats.GetInvertedDocumentTermFreqency(word);

                }
            }



            return result;
        }
    }
}
