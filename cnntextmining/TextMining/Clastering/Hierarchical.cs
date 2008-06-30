using System;
using System.Collections.Generic;
using TextMining.Evaluation;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    class Hierarchical
    {
        private class Pair
        {
            public int first, second;
        }

        public enum Distance { MIN, MAX, AVG };

        private readonly IComparator comparator;
        private readonly WordsStats stats;
        private readonly int maxLen;

        public Hierarchical(IComparator comparator, WordsStats stats, int maxLen)
        {
            this.comparator = comparator;
            this.stats = stats;
            this.maxLen = maxLen;
        }

        public List<Group> Compute(Group news, int k, Distance d)
        {
            Console.WriteLine("Obliczanie odlegosci");
            double[,] distances = computeDistances(news);
            Console.WriteLine("Koniec");

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
                //Console.WriteLine("Iteratation: " + (news.Count - groups.Count));
                // search 2 nearest groups, and put them together
                Pair nearest = getTwoClosestClusters(groups, distances, d);

                groups[nearest.first].AddRange(groups[nearest.second]);
                groups.RemoveAt(nearest.second);
            }

            List<Group> result = new List<Group>();

            foreach (List<int> gr in groups)
            {
                Group toAdd = new Group("");
                foreach (int n in gr)
                {
                    toAdd.Add(news[n]);
                }
                result.Add(toAdd);
            }

            return result;
        }


        private double[,] computeDistances(Group news)
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
                    distances[j, i] = distances[i, j];
                    //Console.Write(distances[i, j] + "  ");
                }
                Console.WriteLine(i);
            }
            return distances;
        }

        private Pair getTwoClosestClusters(List<List<int>> groups, double[,] dist, Distance d)
        {
            double max = double.MinValue;
            int f = 0, s = 0;

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double curr = 0.0;
                    switch (d)
                    {
                        case Distance.AVG:
                            curr = avgDist(groups[i], groups[j], dist);
                            break;
                        case Distance.MAX:
                            curr = maxDist(groups[i], groups[j], dist);
                            break;
                        case Distance.MIN:
                            curr = minDist(groups[i], groups[j], dist);
                            break;
                    }
  

                    if (curr > max)
                    {
                        max = curr;
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

        private double avgDist(List<int> g1, List<int> g2, double[,] dist)
        {
            double sum = 0.0;

            foreach (int i in g1)
            {
                foreach (int j in g2)
                {
                    sum += dist[i, j];
                }
            }

            return sum / (g1.Count*g2.Count);
        }

        private double maxDist(List<int> g1, List<int> g2, double[,] dist)
        {
            double min = double.MaxValue;

            foreach (int i in g1)
            {
                foreach (int j in g2)
                {
                    if (dist[i, j] < min)
                    {
                        min = dist[i, j];
                    }
                }
            }

            return min;
        }

        private double minDist(List<int> g1, List<int> g2, double[,] dist)
        {
            double max = double.MinValue;

            foreach (int i in g1)
            {
                foreach (int j in g2)
                {
                    if (dist[i, j] > max)
                    {
                        max = dist[i, j];

                    }
                }
            }

            return max;
        }




    }
}
