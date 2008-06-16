using System;
using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.Experiments
{
    public class ExperimentStats
    {
        public static void PrintStats(List<List<News>> sets)
        {
            foreach (List<News> set in sets)
            {
                Console.Write(set.Count + ";");
            }
        }
    }
}
