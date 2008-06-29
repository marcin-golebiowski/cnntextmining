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
    }
}