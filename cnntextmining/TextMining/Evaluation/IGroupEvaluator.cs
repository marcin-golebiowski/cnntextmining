using System.Collections.Generic;

namespace TextMining.Evaluation
{
    public interface IGroupEvaluator
    {
        /// <summary>
        /// Return computed score 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        double Eval(List<Group> groups);
    }
}