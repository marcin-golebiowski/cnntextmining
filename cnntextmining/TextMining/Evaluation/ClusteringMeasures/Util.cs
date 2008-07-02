using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class Util
    {
        public static List<string> getTopicsInGroup(Group g)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < g.Count; i++)
            {
                if (!result.Contains(g[i].topicUrl))
                {
                    result.Add(g[i].topicUrl);
                }
            }
            return result;
        }

        public static int topicCountInGroup(string topic, Group g)
        {
            int c = 0;
            for (int i = 0; i < g.Count; i++)
            {
                if (g[i].topicUrl == topic)
                    c++;
            }
            return c;
        }

        public static double avgDeviation(List<Group> clustering, int newsCount)
        {
            double median = newsCount / clustering.Count;
            double sum = 0.0;

            foreach (Group g in clustering)
            {
                sum += g.Count - median;
            }
            return 1.0 / (sum / (double)clustering.Count);
        }
    }
}
