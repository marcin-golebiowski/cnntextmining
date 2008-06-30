using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    public class ExperimentKMeans2 : IExperiment
    {
        private readonly SqlConnection conn;

        public ExperimentKMeans2(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Run()
        {
            var fetcher = new DataFetcher(conn);
            WordsStats stats = new WordsStats(Words.ComputeWords(fetcher.GetAllNews()));
            stats.Compute();

             Console.WriteLine("Words Stats - computed");
             GroupFactory factory = new GroupFactory(conn);


            var topics = new List<string>();
            
            topics.Add(@"http://topics.edition.cnn.com/topics/adolf_hitler");
            topics.Add(@"http://topics.edition.cnn.com/topics/movies");
            topics.Add(@"http://topics.edition.cnn.com/topics/business");
            topics.Add(@"http://topics.edition.cnn.com/topics/education");
            topics.Add(@"http://topics.edition.cnn.com/topics/terrorism");

            Group initialGroup = factory.CreateGroupWithNewsFromTopics(topics);
            CosinusMetricComparator comp = new CosinusMetricComparator();


            Kmeans algorithm = new Kmeans(comp, stats, 1000);
            List<Group> groups = algorithm.Compute(initialGroup, 5, 10);
            ExperimentStats.PrintDetailsString(groups);
                
           
        }
    }
}
