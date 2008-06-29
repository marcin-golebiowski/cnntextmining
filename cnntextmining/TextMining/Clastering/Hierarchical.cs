using System;
using System.Collections.Generic;
using System.Text;

using TextMining.TextTools;
using TextMining.Model;

namespace TextMining.Clastering
{
    class Hierarchical
    {
        private class Pair
        {
            public int first, second;
        }

        private readonly IComparator comparator;
        private readonly WordsStats stats;
        private readonly int maxLen;

        public Hierarchical(IComparator comparator, WordsStats stats, int maxLen)
        {
            this.comparator = comparator;
            this.stats = stats;
            this.maxLen = maxLen;
        }

        public List<List<News>> Compute(List<News> news, int k)
        {
            double[,] distances = computeDistances(news);

            //Make initial N groups

            List<List<int>> groups = new List<List<int>>();
            for (int i = 0; i < news.Count; i++)
            {
                List<int> tmp = new List<int>();
                tmp.Add(i);
                groups.Add(tmp);
            }


            while (groups.Count > k)
            {
                // search 2 nearest groups, and put them together
                Pair nearest = getTwoClosestClusters(groups, distances);

                groups[nearest.first].AddRange(groups[nearest.second]);
                groups.RemoveAt(nearest.second);
            }

            List<List<News>> result = new List<List<News>>();

            foreach (List<int> gr in groups)
            {
                List<News> toAdd = new List<News>();
                foreach (int n in gr)
                {
                    toAdd.Add(news[n]);
                }
                result.Add(toAdd);
            }

            return result;
        }


        private double[,] computeDistances(List<News> news)
        {
            double[,] distances = new double[news.Count, news.Count];

            for (int i = news.Count - 1; i >= 0; i--)
            {
                Vector row = new Vector(stats, news[i], maxLen);
                row.BuildVector();

                for (int j = 0; j < i; j++)
                {
                    Vector col = new Vector(stats, news[j], maxLen);
                    col.BuildVector();

                    distances[i, j] = comparator.Compare(row, col);
                    Console.Write(distances[i, j] + "  ");
                }
                Console.WriteLine();
            }
            return distances;
        }

        private Pair getTwoClosestClusters(List<List<int>> groups, double[,] dist)
        {
            double min = double.MaxValue;
            int f = 0, s = 0;

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double curr = distanceBetweenGroups(groups[i], groups[j], dist);
                    if (curr < min)
                    {
                        min = curr;
                        f = i;
                        s = j;
                    }
                }
            }
            Pair result = new Pair();
            result.first = f;
            result.second = s;
          
            return result;
        }

        private double distanceBetweenGroups(List<int> g1, List<int> g2, double[,] dist)
        {
            double result = 0.0;

            double min = double.MaxValue;

            foreach (int i in g1)
            {
                foreach (int j in g2)
                {
                    double curr;
                    if (i > j)
                        curr = dist[i, j];
                    else
                        curr = dist[j, i];

                    if (curr < min)
                        min = curr;
                }
            }


            return result;
        }




    }
}
