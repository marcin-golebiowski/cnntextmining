using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class MedianCoverageForDominanceTopic
    {
        public static double compute(List<Group> clustering)
        {
            double coverage = 0.0;


            foreach (Group g in clustering)
            {
                int dominanceCount = int.MinValue;

                List<string> topics = Util.getTopicsInGroup(g);

                foreach (string topic in topics)
                {
                    int tmp = Util.topicCountInGroup(topic, g);
                    if (tmp > dominanceCount)
                    {
                        dominanceCount = tmp;
                    }
                }
                coverage += (double)dominanceCount / (double)g.Count;
            }

            return coverage / clustering.Count;
        }


    }
}
