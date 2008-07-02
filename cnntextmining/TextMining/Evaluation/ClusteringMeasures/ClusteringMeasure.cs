using System;
using System.Collections.Generic;
using System.Text;
using TextMining.DataLoading;
using TextMining.Model;
using System.Data.SqlClient;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class ClusteringMeasure
    {

        // This method is uneffective.
        public static double compute(SqlConnection conn, List<string> inputTopicsUris, List<Group> clustering, int kDominance)
        {
            double result = 0.0;

            DataFetcher fetcher = new DataFetcher(conn);

            List<News> allNews = fetcher.GetAllNews();
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

            int c2 = 0;
            int newsCount = 0;

            foreach (Group group in clustering)
            {
                newsCount += group.Count;

                List<string> topicsInGroup = Util.getTopicsInGroup(group);
                foreach (string topic in topicsInGroup)
                {
                    result += (double)Util.topicCountInGroup(topic, group) / (double)newsCountInTopic[topic].Count;
                    result += (double)Util.topicCountInGroup(topic, group) / (double)group.Count;
                    c2 += 2;
                }
                //result *= topicDominance(group, topicsInGroup, kDominance);
                 
            }

            result *= Util.avgDeviation(clustering, newsCount);

            return result / (double)c2;
        }

    }
}
