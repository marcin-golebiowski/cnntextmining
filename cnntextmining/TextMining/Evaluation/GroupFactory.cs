using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Evaluation
{
    public class GroupFactory
    {
        private readonly SqlConnection conn;

        public GroupFactory(SqlConnection conn)
        {
            this.conn = conn;
        }

        public Group CreateGroupWithAllNews()
        {
            var result = new Group("INITIAL");
            var f = new DataFetcher(conn);
            result.AddRange(Words.ComputeWords(f.GetAllNews()));
            return result;
        }

        public Group CreateGroupWithAllNews(int count)
        {
            var result = new Group("INITIAL");
            var f = new DataFetcher(conn);
            result.AddRange(Words.ComputeWords(f.GetAllNews(false, count)));
            return result;
        }

        public Group CreateGroupWithNewsFromTopics(List<string> topics)
        {
            var result = new Group("INITIAL");
            var f = new DataFetcher(conn);

            foreach (string topic in topics)
            {
                result.AddRange(Words.ComputeWords(f.GetAllNews(topic)));    
            }

            return result;
        }
    }
}
