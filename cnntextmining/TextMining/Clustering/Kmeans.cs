using System;
using System.Collections.Generic;
using TextMining.Evaluation;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clustering
{
    public class Kmeans
    {
        private readonly IComparator comparator;
        private readonly WordsStats stats;
        private readonly int maxLen;
       
        public Kmeans(IComparator comparator, WordsStats stats,  int maxLen)
        {
            this.comparator = comparator;
            this.stats = stats;
            this.maxLen = maxLen;
        }


        public List<Group> Compute(Group news, uint K, uint maxIterations)
        
        {
            var rand = new Random();
            var centroids = new Vector[K];

            //1. Losuj K newsow

            for (int i = 0; i < K; i++)
            {
                News n = news[rand.Next()%news.Count];
                centroids[i] = new Vector(stats, n, maxLen);
                centroids[i].BuildVector();
            }


            var assigment = new int[news.Count];
            var vectors = new Vector[news.Count];


            // /// Petla
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // liczenie przydzialu
                for (int i = 0; i < news.Count; i++)
                {
                    vectors[i] = new Vector(stats, news[i], maxLen);
                    vectors[i].BuildVector();

                    int min = 0;
                    double maxVal = comparator.Compare(centroids[0], vectors[i]);

                    for (int j = 1; j < centroids.Length; j++)
                    {
                        double val = comparator.Compare(centroids[j], vectors[i]);

                        if (val > maxVal)
                        {
                            maxVal = val;
                            min = j;
                        }
                    }
                    assigment[i] = min;
                }

                // liczymy centroidy
                centroids = ComputeNewCentroids(K, assigment, vectors, centroids);
            }
            return GetCurrentSet(news, assigment, K);
        }

        private static List<Group> GetCurrentSet(Group news, int[] assigment, uint K)
        {
            //3. Zwróc wynik
            var result = new List<Group>();
            for (int i = 0; i < K; i++)
            {
                result.Add(new Group(""));
            }

            for (int j = 0; j < news.Count; j++)
            {
                result[assigment[j]].Add(news[j]);
            }
            return result;
        }



        private Vector[] ComputeNewCentroids(uint K, int[] assigment, Vector[] vectors, Vector[] oldCentroids)
        {
            var newCentroids = new Vector[K];

            var sets = new List<List<int>>();
            for (int i = 0; i < K; i++)
            {
                sets.Add(new List<int>());
            }


            // adding neighbours
            for (int j = 0; j < assigment.Length; j++)
            {
                sets[assigment[j]].Add(j);
            }

            for (int i = 0; i < K; i++)
            {
                newCentroids[i] = new Vector(stats, null, maxLen);


                // dla kazdego newsa ze aktualnego zbioru centroidow
                foreach (int index in sets[i])
                {
                    // dla kazdeg slowa z wektora z danej grupy
                    foreach (string word in vectors[index].Items.Keys)
                    {

                        if (newCentroids[i].Items.ContainsKey(word))
                        {
                            newCentroids[i].Items[word] +=
                                vectors[index].Items[word]/(sets[i].Count + 1);
                        }
                        else
                        {
                            newCentroids[i].Items[word] =
                                vectors[index].Items[word]/(sets[i].Count + 1);
                        }
                    }
                }


                // dodanwanie do nowego 
                foreach (string word in oldCentroids[i].Items.Keys)
                {
                    if (newCentroids[i].Items.ContainsKey(word))
                    {
                        newCentroids[i].Items[word] +=
                            oldCentroids[i].Items[word] / (sets[i].Count + 1);
                    }
                    else
                    {
                        newCentroids[i].Items[word] =
                            oldCentroids[i].Items[word] / (sets[i].Count + 1);
                    }
                }

                newCentroids[i].Trim();
            }

            return newCentroids;
        }
    }
}
