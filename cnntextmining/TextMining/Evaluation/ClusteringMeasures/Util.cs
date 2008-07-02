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

        // Chcemy aby w hrupi byl topic dominujacy
        // Srednia + max odbiega mocno
        public static double topicDominance(Group g, List<string> topics, int k)
        {
            double result = 0.0;

            double median = (double)g.Count / (double)topics.Count;

            int max = int.MinValue;

            for (int i = 0; i < topics.Count; i++)
            {
                int tmp = topicCountInGroup(topics[i], g);
                if (tmp > max)
                {
                    max = tmp;
                }
            }
            result = max / (median * k);
            if (result >= 1.0)
            {
                result = 1.0;
            }
            return result;
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
