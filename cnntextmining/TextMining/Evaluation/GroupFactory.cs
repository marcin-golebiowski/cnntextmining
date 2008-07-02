using System.Collections.Generic;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Evaluation
{
    public class GroupFactory
    {
        private GroupFactory()
        {
        }

        public static Group CreateGroupWithAllNews()
        {
            var result = new Group("INITIAL");
            result.AddRange(Words.ComputeWords(DataStore.Instance.GetAllNews()));
            return result;
        }

        public static Group CreateGroupWithAllNews(int count)
        {
            var result = new Group("INITIAL");
           
            result.AddRange(Words.ComputeWords(DataStore.Instance.GetAllNews(count)));
            return result;
        }

        public static Group CreateGroupWithNewsFromTopics(List<string> topics)
        {
            var result = new Group("INITIAL");
            foreach (string topic in topics)
            {
                result.AddRange(Words.ComputeWords(DataStore.Instance.GetAllNews(topic)));    
            }

            return result;
        }
    }
}
