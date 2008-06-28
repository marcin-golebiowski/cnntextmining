﻿using System;
using System.Collections.Generic;
using TextMining.Evaluation;
using TextMining.Experiments;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    public class Kmeans
    {
        private readonly IComparator comparator;
        private readonly DefaultEvaluator eval;
        private readonly WordsStats stats;
        private readonly int maxLen;

        public Kmeans(IComparator comparator,  DefaultEvaluator eval, WordsStats stats,  int maxLen)
        {
            this.comparator = comparator;
            this.eval = eval;
            this.stats = stats;
            this.maxLen = maxLen;
        }


        public List<List<News>> Compute(List<News> news, int K, int maxIterations)
        {
            EuclidesMetricComparator comp = new EuclidesMetricComparator();

            Random rand = new Random();
            Vector[] centroids = new Vector[K];

            //1. Losuj K newsow

            for (int i = 0; i < K; i++)
            {
                News n = news[rand.Next()%news.Count];
                centroids[i] = new Vector(stats, n, maxLen);
                centroids[i].BuildVector();
            }

            int[] assigment = new int[news.Count];
            Vector[] vectors = new Vector[news.Count];


            // /// Petla
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                DateTime start = DateTime.Now;

                Console.WriteLine("Iteration " + iteration + ": started");
                // liczenie przydzialu
                for (int i = 0; i < news.Count; i++)
                {
                    vectors[i] = new Vector(stats, news[i], maxLen);
                    vectors[i].BuildVector();

                    int min = 0;
                    double minVal = comp.Compare(centroids[0], vectors[i]);

                    for (int j = 1; j < centroids.Length; j++)
                    {
                        double val = comp.Compare(centroids[j], vectors[i]);
                        //Console.WriteLine(val);

                        if (val < minVal)
                        {
                            minVal = val;
                            min = j;
                        }
                    }
                    //Console.WriteLine("----");
                    assigment[i] = min;
                }

                // liczymy centroidy
                centroids = ComputeNewCentroids(K, assigment, news, stats, vectors, centroids);
                
                
                
                Console.WriteLine("Time: " + (DateTime.Now - start));
                List<List<News>> currentSet = GetCurrentSet(news, assigment, K);
                Console.WriteLine("Eval: " + eval.GetScore(currentSet));
                ExperimentStats.PrintStats(currentSet);
                Console.WriteLine("---");
               
            }

            return GetCurrentSet(news, assigment, K);
        }

        private List<List<News>> GetCurrentSet(List<News> news, int[] assigment, int K)
        {

            //3. Zwróc wynik
            List<List<News>> result = new List<List<News>>();
            for (int i = 0; i < K; i++)
            {
                result.Add(new List<News>());
            }

            for (int j = 0; j < news.Count; j++)
            {
                result[assigment[j]].Add(news[j]);
            }
            return result;
        }

        private Vector[] ComputeNewCentroids(int K, int[] assigment, List<News> news,
            WordsStats stats, Vector[] vectors, Vector[] oldCentroids)
        {
            Vector[] newCentroids = new Vector[K];

            List<List<int>> sets = new List<List<int>>();
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


                //trim
                newCentroids[i].Trim();
            }

            return newCentroids;
        }
    }
}
