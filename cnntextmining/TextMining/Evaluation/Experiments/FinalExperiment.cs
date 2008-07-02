using System;
using System.Collections.Generic;
using TextMining.DataLoading;
using TextMining.TextTools;
using TextMining.Model;
using TextMining.Clustering;
using TextMining.Evaluation.ClusteringMeasures;

namespace TextMining.Evaluation.Experiments
{
    class FinalExperiment : IExperiment
    {
        public int RelatedToWrite { get; set; }
        private readonly int topicCount;
        private readonly int kMeansIterations;
        const int maxLen = 2000;


        public FinalExperiment(int topicCount, int kMeansIterations, int relatedToWrite)
        {
            RelatedToWrite = relatedToWrite;
            this.topicCount = topicCount;
            this.kMeansIterations = kMeansIterations;
        }


        public void Run()
        {

            WordsStats stats = new WordsStats(Words.ComputeWords(DataStore.Instance.GetAllNews()));
            stats.Compute();

            List<string> randomTopics = getRandomTopics(topicCount);
            Group initialGroup = GroupFactory.CreateGroupWithNewsFromTopics(randomTopics);


            Console.WriteLine("==========================");
            Console.WriteLine("Topiki w grupie:");
            foreach (string topic in randomTopics)
            {
                Console.WriteLine(topic + " [" + Util.topicCountInGroup(topic, initialGroup) + "]");
            }

            Console.WriteLine("Rozmiar grupy: " + initialGroup.Count);
            CosinusMetricComparator cos = new CosinusMetricComparator();
            EuclidesMetricComparator eu = new EuclidesMetricComparator();
            JaccardMetricCompartator ja = new JaccardMetricCompartator();

            DateTime start;
            TimeSpan t1, t2;
            

            Dbscan db = new Dbscan(cos, stats, maxLen);
            Hierarchical hr = new Hierarchical(cos, stats, maxLen);
            Kmeans km = new Kmeans(cos, stats, maxLen);

            Console.WriteLine("===============================================================");

            start = DateTime.Now;
            List<Group> hierarchicalResult = hr.Compute(initialGroup, topicCount, Hierarchical.Distance.AVG);
            t1 = (DateTime.Now - start);
            
            start = DateTime.Now;
            List<Group> kMeansResult = km.Compute(initialGroup, topicCount, kMeansIterations);
            t2 = (DateTime.Now - start);

            PrintStats("KMeans", t1, kMeansResult);
            PrintStats("Hierachical", t2, hierarchicalResult);


            Console.WriteLine("===================== Powiązane topiki ==============================");
            HighRelatedTopics htopics = new HighRelatedTopics(hierarchicalResult);
            List<string[]> related = htopics.getHighRelatedTopics(RelatedToWrite);

            foreach(string[] pair in related)
            {
                Console.WriteLine(pair[0] + "\n" + pair[1] + "\n");
            }

            //ExperimentStats.PrintDetailsString(dbscanResult);
            //ExperimentStats.PrintDetailsString(hierarchicalResult);
            //ExperimentStats.PrintDetailsString(kMeansResult);
        }

        private static void PrintStats(string name, TimeSpan t1, List<Group> result)
        {
            IGroupEvaluator groupEvaluator1 = new MedianCoverageForDominanceTopic();
            IGroupEvaluator groupEvaluator2 = new TopicSpitCount();
            IGroupEvaluator groupEvaluator3 = new MedianTopicSplit();

            Console.WriteLine("========================================================================");
            Console.WriteLine(" ---== "  + name +  " ==---");
            Console.WriteLine("  -- czas działania = " + t1);
            Console.WriteLine("  -- średnia dominacja = " + groupEvaluator1.Eval(result));
            Console.WriteLine("  -- suma rozbicia topików = " + groupEvaluator2.Eval(result));
            Console.WriteLine("  -- średnia rozbicia topików = " + groupEvaluator3.Eval(result));
            Console.WriteLine("---------------------");
            Console.WriteLine("Zawartość grup");
            ExperimentStats.PrintDetailsString(result);
        }

        private List<string> getRandomTopics(int size)
        {
            List<News> news = DataStore.Instance.GetAllNews();

            var result = new List<string>();
            var rand = new Random();


            while (result.Count < size)
            {
                int x = rand.Next(news.Count);
                string topic = news[x].topicUrl;
                if (!result.Contains(topic))
                {
                    result.Add(topic);
                }
            }
            return result;
        }
    }
}
