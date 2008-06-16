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

            foreach (string word in x.Items.Keys)
            {
               if (y.Items.ContainsKey(word))
               {
                   double val = x.Items[word] - y.Items[word];
                   val = val * val;
                   result += val;
               }
            }

            result = Math.Sqrt(Math.Abs(result));

            return result;
        }
    }
}
