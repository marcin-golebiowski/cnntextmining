using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TextMining.Clastering;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    public class ExperimentKMeans : IExperiment
    {
        private readonly SqlConnection conn;

        public ExperimentKMeans(SqlConnection conn)
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
            topics.Add(@"http://topics.edition.cnn.com/topics/astronomy");
            topics.Add(@"http://topics.edition.cnn.com/topics/armed_forces");
            topics.Add(@"http://topics.edition.cnn.com/topics/genetics");
            topics.Add(@"http://topics.edition.cnn.com/topics/religion");

            Group initialGroup = factory.CreateGroupWithNewsFromTopics(topics);
            CosinusMetricComparator comp = new CosinusMetricComparator();


            Kmeans algorithm = new Kmeans(comp, stats, 1000);
            List<Group> groups = algorithm.Compute(initialGroup, 4, 10);
            ExperimentStats.PrintDetailsString(groups);
                
           
        }
    }
}
