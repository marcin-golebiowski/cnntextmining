using System;
using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.Evaluation
{
    public class ExperimentStats
    {
        public static void PrintStats(List<Group> groups)
        {
            foreach (Group set in groups)
            {
                Console.Write(set.Count + ";");
            }
            Console.WriteLine();
        }
    }
}