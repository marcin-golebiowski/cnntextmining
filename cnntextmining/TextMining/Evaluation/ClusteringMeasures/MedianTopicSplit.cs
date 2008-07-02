using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class MedianTopicSplit
    {
        public static double compute(List<Group> clustering)
        {
            double sum = 0.0;

            List<string>[] topicsInGroups = new List<string>[clustering.Count];



            for(int i = 0; i < clustering.Count; i++)
            {
                topicsInGroups[i] = new List<string>();
                

            }


            return 0.0;
        }
    }
}
