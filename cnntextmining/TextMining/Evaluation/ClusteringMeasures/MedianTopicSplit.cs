using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class MedianTopicSplit : IGroupEvaluator
    {
        public  double Eval(List<Group> clustering)
        {
            double sum = 0.0;

            Dictionary<string, int> topicInGroupCount = new Dictionary<string,int>();

            foreach(Group g in clustering)
            {
                List<string> topics = Util.getTopicsInGroup(g);
                foreach(string topic in topics)
                {
                    if (topicInGroupCount.ContainsKey(topic))
                    {
                        topicInGroupCount[topic]++;
                    }
                    else
                    {
                        topicInGroupCount.Add(topic, 1);
                    }
                }
            }

            foreach (string topic in topicInGroupCount.Keys)
            {
                sum += topicInGroupCount[topic];
            }

            return sum / (double)topicInGroupCount.Values.Count;
        }
    }
}
