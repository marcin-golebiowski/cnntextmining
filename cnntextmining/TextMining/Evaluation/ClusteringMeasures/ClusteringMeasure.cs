using System.Collections.Generic;
using TextMining.DataLoading;
using TextMining.Model;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class ClusteringMeasure
    {

        // This method is uneffective.
        public static double compute(List<string> inputTopicsUris, List<Group> clustering, int kDominance)
        {
            double result = 0.0;

            List<News> allNews = DataStore.Instance.GetAllNews();
            Dictionary<string, Group> newsCountInTopic = new Dictionary<string,Group>();


            foreach(string topic in inputTopicsUris)
            {
                int c = 0;
                Group topicNews = new Group("x");
                foreach(News news in allNews)
                {
                    if(news.topicUrl == topic)
                    {
                        topicNews.Add(news);
                    }
                }
                newsCountInTopic.Add(topic, topicNews);
            }

            int newsCount = 0;

            foreach (Group group in clustering)
            {
                newsCount += group.Count;

                List<string> topicsInGroup = Util.getTopicsInGroup(group);
                foreach (string topic in topicsInGroup)
                {
                    result *= (double)Util.topicCountInGroup(topic, group) / (double)newsCountInTopic[topic].Count;
                    result *= (double)Util.topicCountInGroup(topic, group) / (double)group.Count;
                }
                //result *= topicDominance(group, topicsInGroup, kDominance);
                 
            }

            result *= Util.avgDeviation(clustering, newsCount);

            return result;
        }

    }
}
