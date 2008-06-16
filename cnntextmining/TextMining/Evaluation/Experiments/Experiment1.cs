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
            var dataFetcher = new DataFetcher(conn);
            List<News> news = dataFetcher.GetAllNews(true, 1000);
            var stats = new WordsStats(news);
            stats.Compute();

            var comparator = new DefaultNewsComparator(stats);


            var algorithm = new Kmeans(comparator, stats, 400);

            Console.WriteLine("Starting KMeans");
            List<List<News>> sets = algorithm.Compute(news, 10, 2);
            Console.WriteLine("KMeans end");

            ExperimentStats.PrintStats(sets);

            Console.WriteLine("Loading assingments..");
            TopicOriginalAssigment ass = new TopicOriginalAssigment(conn);
            ass.Load();


            DefaultEvaluator eval = new DefaultEvaluator(ass);

            Console.WriteLine("Starting eval");
            Console.WriteLine(eval.GetScore(sets));
            Console.WriteLine("End eval");
            
        }
    }
}
