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
        private readonly uint relatedToWrite;
        private readonly uint topicCount;
        private readonly string[] topics;
        private readonly uint kMeansIterations;
        private const int maxLen = 2000;

        private ushort comparatorId;
        private IComparator[] comparators = new IComparator[]
                                                {
                                                    new CosinusMetricComparator(), new EuclidesMetricComparator(),
                                                    new JaccardMetricCompartator()
                                                };


        public FinalExperiment(string[] topics, uint kMeansIterations, uint relatedToWrite, ushort comparatorId)
        {
            this.topics = topics;
            this.kMeansIterations = kMeansIterations;
            this.relatedToWrite = relatedToWrite;
            this.comparatorId = comparatorId;
        }

        public FinalExperiment(uint topicCount, uint kMeansIterations, uint relatedToWrite, ushort comparatorId)
        {
            this.relatedToWrite = relatedToWrite;
            this.comparatorId = comparatorId;
            this.topicCount = topicCount;
            this.kMeansIterations = kMeansIterations;
        }


        public void Run()
        {
            DateTime expSt = DateTime.Now;

            Console.WriteLine("========================================================================");
            Console.WriteLine(" Metoda porównania: " + comparators[comparatorId].GetType());
            Console.WriteLine("========================================================================");
            WordsStats stats = new WordsStats(Words.ComputeWords(DataStore.Instance.GetAllNews()));
            stats.Compute();

            List<string> experimentTopics;

            if (topics != null)
            {
                experimentTopics = new List<string>();
                experimentTopics.AddRange(topics);
            }
            else
            {
                experimentTopics = GetRandomTopics(topicCount);
            }


            Group initialGroup = GroupFactory.CreateGroupWithNewsFromTopics(experimentTopics);


            Console.WriteLine("========================================================================");
            Console.WriteLine("Topiki w grupie początkowej:");
            foreach (string topic in experimentTopics)
            {
                Console.WriteLine(topic + " [" + Util.topicCountInGroup(topic, initialGroup) + "]");
            }

            Console.WriteLine("Rozmiar grupy: " + initialGroup.Count);
        ;

            DateTime start;
            TimeSpan t1, t2;
            

            Hierarchical hr = new Hierarchical(comparators[comparatorId], stats, maxLen);
            Kmeans km = new Kmeans(comparators[comparatorId], stats, maxLen);

            Console.WriteLine("========================================================================");

            start = DateTime.Now;
            List<Group> hierarchicalResult = hr.Compute(initialGroup, topicCount != 0 ? topicCount : (uint)topics.Length, 
                Hierarchical.Distance.AVG);
            t1 = (DateTime.Now - start);
            
            start = DateTime.Now;
            List<Group> kMeansResult = km.Compute(initialGroup, topicCount != 0 ? topicCount : (uint)topics.Length, kMeansIterations);
            t2 = (DateTime.Now - start);

            PrintStats("KMeans", t2, kMeansResult, kMeansIterations);
            PrintStats("Hierachical", t1, hierarchicalResult, 0);

            Console.WriteLine("========================================================================");
            Console.WriteLine("Czas działania: " + (DateTime.Now - expSt));

        }

        private  void PrintStats(string name, TimeSpan t1, List<Group> result, uint iter)
        {
            IGroupEvaluator groupEvaluator1 = new MedianCoverageForDominanceTopic();
            IGroupEvaluator groupEvaluator2 = new TopicSpitCount();
            IGroupEvaluator groupEvaluator3 = new MedianTopicSplit();

            Console.WriteLine("========================================================================");
            Console.WriteLine(" --------==== " + name + " ====--------");

            if (iter != 0)
            {
                Console.WriteLine("  -- liczba iteracji => " + iter);
            }

            Console.WriteLine("  -- czas działania => " + t1);
            Console.WriteLine("  -- średnia dominacja => " + groupEvaluator1.Eval(result));
            Console.WriteLine("  -- suma rozbicia topików => " + groupEvaluator2.Eval(result));
            Console.WriteLine("  -- średnia rozbicia topików => " + groupEvaluator3.Eval(result));
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine(" --- Zawartość grup ---");
            ExperimentStats.PrintDetailsString(result);


            Console.WriteLine(" --- Powiązane topiki ---");
            HighRelatedTopics htopics = new HighRelatedTopics(result);
            List<string[]> related = htopics.getHighRelatedTopics(relatedToWrite);
        }

        private static List<string> GetRandomTopics(uint size)
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
