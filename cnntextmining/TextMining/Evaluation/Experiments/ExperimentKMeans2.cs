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

            Group initialGroup = factory.CreateGroupWithAllNews(1000);
            CosinusMetricComparator comp = new CosinusMetricComparator(10);


            Kmeans algorithm = new Kmeans(comp, stats, 2000);
            List<Group> groups = algorithm.Compute(initialGroup, 100, 20);
            ExperimentStats.PrintDetailsString(groups);
        }
    }
}
