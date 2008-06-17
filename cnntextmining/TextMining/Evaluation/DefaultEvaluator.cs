using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.Experiments
{
    public class DefaultEvaluator : IEvaluator
    {
        private readonly TopicOriginalAssigment assigment;

        public DefaultEvaluator(TopicOriginalAssigment assigment)
        {
            this.assigment = assigment;
        }

        public long GetScore(List<List<News>> sets)
        {
            long score = 0;

            foreach (var set in sets)
            {
                for (int i = 0; i < set.Count; i++)
                {
                    for (int j = i + 1; j < set.Count; j++)
                    {
                        if (assigment.AreInTheSameTopic(set[i].url, set[j].url))
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
