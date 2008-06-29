using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.Evaluation
{
    public class ExperimentStats
    {
        public static string GetGroupCountString(List<Group> groups)
        {
            var b = new StringBuilder();

            foreach (Group set in groups)
            {
                b.Append(set.Count + ";");
            }
            b.AppendLine();

            return b.ToString();
        }


        public static void PrintDetailsString(List<Group> groups)
        {
            Console.WriteLine("==");
            foreach (Group set in groups)
            {
                var count = new Dictionary<string, int>();


                for (int i = 0; i < set.Count; i++)
                {
                    Console.WriteLine(set[i].url);
                    
                    if (count.ContainsKey(set[i].topicUrl))
                    {
                        count[set[i].topicUrl] += 1;
                    }
                    else
                    {
                        count[set[i].topicUrl] = 1;
                    }
                }

                Console.WriteLine("--");
                foreach (KeyValuePair<string, int> pair in count)
                {
                    Console.WriteLine(pair.Key + "-" + pair.Value);
                }
                Console.WriteLine("+++++++++++++");
            }

            Console.WriteLine("=====");
        }
    }
}