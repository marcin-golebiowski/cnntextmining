using System.Collections.Generic;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class MedianCoverageForDominanceTopic : IGroupEvaluator
    {
        public double Eval(List<Group> groups)
        {
            double coverage = 0.0;

            foreach (Group g in groups)
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
                coverage += dominanceCount / (double)g.Count;
            }
            return coverage / groups.Count;
        }
    }
}
