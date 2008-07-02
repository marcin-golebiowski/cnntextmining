using System.Collections.Generic;

namespace TextMining.Evaluation.ClusteringMeasures
{
    public class TopicSpitCount : IGroupEvaluator
    {
        public double Eval(List<Group> groups)
        {
            int count = 0;

            foreach (var set in groups)
            {
                count += Util.getTopicsInGroup(set).Count;
            }

            return count;
        }
    }
}
