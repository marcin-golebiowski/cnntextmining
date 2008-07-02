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
            double result = 1.0;
            int newsCount = 0;

            foreach (Group group in clustering)
            {
                newsCount += group.Count;

                List<string> topicsInGroup = Util.getTopicsInGroup(group);
                foreach (string topic in topicsInGroup)
                {
                    result *= (double)Util.topicCountInGroup(topic, group) / (double)DataStore.Instance.GetTopicSize(topic);
                    result *= (double)Util.topicCountInGroup(topic, group) / (double)group.Count;
                }
                //result *= topicDominance(group, topicsInGroup, kDominance);
                 
            }
            //result *= Util.avgDeviation(clustering, newsCount);
            return result;
        }

    }
}
