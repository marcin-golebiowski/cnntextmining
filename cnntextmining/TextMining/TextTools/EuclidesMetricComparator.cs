using System;

namespace TextMining.TextTools
{
    class EuclidesMetricComparator : IComparator
    {
        private readonly WordsStats stats;

        public EuclidesMetricComparator(WordsStats stats)
        {
            this.stats = stats;
        }

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
