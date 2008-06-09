using System.Collections.Generic;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    public class Kmeans
    {
        private readonly INewsComparator comparator;

        public Kmeans(INewsComparator comparator)
        {
            this.comparator = comparator;
        }

        public List<List<News>> Compute(List<News> news,
            int numerOfClusters, int maxIterations) 
        {
            return null;
        }
    }
}
