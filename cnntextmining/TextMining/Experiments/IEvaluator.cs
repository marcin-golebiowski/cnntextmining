using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.Experiments
{
    public interface IEvaluator
    {
        /// <summary>
        /// Return computed score 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        long GetScore(List<List<CNNPage>> sets);
    }
}
