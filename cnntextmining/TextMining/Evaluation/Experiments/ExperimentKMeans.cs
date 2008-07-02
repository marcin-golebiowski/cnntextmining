using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TextMining.Clustering;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    public class ExperimentKMeans : IExperiment
    {

        public void Run()
        {
            WordsStats stats = new WordsStats(Words.ComputeWords(DataStore.Instance.GetAllNews()));
            stats.Compute();

            Console.WriteLine("Words Stats - computed");
            var topics = new List<string>();
            topics.Add(@"http://topics.edition.cnn.com/topics/computer_technology");
            topics.Add(@"http://topics.edition.cnn.com/topics/formula_one_racing");
            topics.Add(@"http://topics.edition.cnn.com/topics/medicine");

            Group initialGroup = GroupFactory.CreateGroupWithNewsFromTopics(topics);
            JaccardMetricCompartator comp = new JaccardMetricCompartator();


            Kmeans algorithm = new Kmeans(comp, stats, 1000);
            List<Group> groups = algorithm.Compute(initialGroup, 3, 10);
            ExperimentStats.PrintDetailsString(groups);
                
           
        }
    }
}
