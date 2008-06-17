using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.DataLoading;
using TextMining.Experiments;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    class Experiment1 : IExperiment
    {
        private readonly SqlConnection conn;

        public SqlConnection Conn
        {
            get { return conn; }
        }

        public SqlConnection _ { get; set; }

        public Experiment1(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Run()
        {

            const int newsCount = 1000;
            const int K = 100;
            const int VectorLen = 400;
            const int iterations = 5;


            DateTime clock;

            Console.WriteLine("Assingments: start loading");
            clock = DateTime.Now;
            TopicOriginalAssigment ass = new TopicOriginalAssigment(conn);
            ass.Load();

            Console.WriteLine("Assigments: end loading - " + (DateTime.Now - clock).ToString());
            

            var dataFetcher = new DataFetcher(conn);

            Console.WriteLine("News: start loading");
            clock = DateTime.Now;
            List<News> news = dataFetcher.GetAllNews(true, newsCount);
            Console.WriteLine("News: end loading -  " + (DateTime.Now - clock).ToString());



            Console.WriteLine("WordsStats: start");
            clock = DateTime.Now;
            var stats = new WordsStats(news);
            stats.Compute();
            Console.WriteLine("WordsStats: end - " + (DateTime.Now - clock).ToString());
            
            DefaultEvaluator eval = new DefaultEvaluator(ass); 

            var comparator = new DefaultNewsComparator(stats);
            var algorithm = new Kmeans(comparator, eval, stats,VectorLen);

            Console.WriteLine("Starting KMeans");
            List<List<News>> sets = algorithm.Compute(news, K, iterations);
            Console.WriteLine("KMeans end");

            ExperimentStats.PrintStats(sets);

            
           
            
        }
    }
}
