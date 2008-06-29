using System.Collections.Generic;

namespace TextMining.Evaluation
{
    public interface IEvaluator
    {
        /// <summary>
        /// Return computed score 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        long GetScore(List<Group> sets);
    }
}