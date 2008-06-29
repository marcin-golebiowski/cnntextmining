using System.Collections.Generic;
using TextMining.Evaluation;
using TextMining.Model;

namespace TextMining.Evaluation
{
    public class DefaultEvaluator : IEvaluator
    {
        private readonly TopicOriginalAssigment assigment;

        public DefaultEvaluator(TopicOriginalAssigment assigment)
        {
            this.assigment = assigment;
        }

        public long GetScore(List<Group> groups)
        {
            long score = 0;

            foreach (var group in groups)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    for (int j = i + 1; j < group.Count; j++)
                    {
                        if (assigment.AreInTheSameTopic(group[i].url, group[j].url))
                        {
                            score++;
                        }
                        else
                        {
                            score--;
                        }
                    }
                }
            }
            return score;
        }
    }
}
