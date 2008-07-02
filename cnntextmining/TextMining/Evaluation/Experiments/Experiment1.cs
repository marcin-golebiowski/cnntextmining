using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clustering;
using TextMining.DataLoading;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    class Experiment1 : IExperiment
    {
      

        public void Run()
        {

            const int newsCount = 500;
            const int K = 100;
            const int VectorLen = 3000;
            const int iterations = 2;


            DateTime clock;

            Console.WriteLine("Assingments: start loading");
            clock = DateTime.Now;

            Console.WriteLine("Assigments: end loading - " + (DateTime.Now - clock).ToString());
            


            Console.WriteLine("News: start loading");
            clock = DateTime.Now;
            List<News> news = DataStore.Instance.GetAllNews(newsCount);
            Console.WriteLine("News: end loading -  " + (DateTime.Now - clock).ToString());



            Console.WriteLine("WordsStats: start");
            clock = DateTime.Now;
            var stats = new WordsStats(news);
            stats.Compute();
            Console.WriteLine("WordsStats: end - " + (DateTime.Now - clock).ToString());
            
            var comparator = new CosinusMetricComparator();
            var algorithm = new Kmeans(comparator, stats,VectorLen);

            Console.WriteLine("Starting KMeans");
            //List<List<News>> sets = algorithm.Compute(news, K, iterations);
            Console.WriteLine("KMeans end");

            //ExperimentStats.GetGroupCountString(sets);

            
           
            
        }
    }
}
