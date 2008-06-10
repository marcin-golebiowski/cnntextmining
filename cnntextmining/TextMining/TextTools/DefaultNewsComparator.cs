using System;
using System.Collections.Generic;

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

        public double Compare(Vector x, Vector y)
        {
            double result = 0;

            var words = new List<string>();

            foreach (string word in x.Items.Keys)
            {
                words.Add(word);
            }

            foreach (string word in y.Items.Keys)
            {
                words.Add(word);
            }


            foreach (string word in words)
            {
                double val =
                    (stats.GetTF(word, x.URL)-stats.GetTF(word, y.URL))*stats.GetIDF(word);
                val = val*val;

                result += val;
            }

            // News1: [ word1 -> 1.3 , word2 -> 0.0004, word3 -> 0 , ......]
            // News2: [ word1' -> 1.2, word2' -> 0, word3' -> .....]

            result = Math.Sqrt(Math.Abs(result));

            return result;
        }
    }
}
