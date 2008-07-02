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
            WordsStats stats = new WordsStats(Words.ComputeWords(DataStore.Instance.GetAllNews()));
            stats.Compute();

             Console.WriteLine("Words Stats - computed");
             GroupFactory factory = new GroupFactory(conn);


            var topics = new List<string>();
            topics.Add(@"http://topics.edition.cnn.com/topics/computer_technology");
            topics.Add(@"http://topics.edition.cnn.com/topics/formula_one_racing");
            topics.Add(@"http://topics.edition.cnn.com/topics/medicine");

            Group initialGroup = factory.CreateGroupWithNewsFromTopics(topics);
            JaccardMetricCompartator comp = new JaccardMetricCompartator();


            Kmeans algorithm = new Kmeans(comp, stats, 1000);
            List<Group> groups = algorithm.Compute(initialGroup, 3, 10);
            ExperimentStats.PrintDetailsString(groups);
                
           
        }
    }
}
