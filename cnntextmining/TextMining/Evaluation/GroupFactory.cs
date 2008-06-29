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
            result.AddRange(WordsSetter.ComputeWords(f.GetAllNews()));
            return result;
        }

        public Group CreateGroupWithNewsFromTopics(string[] topics)
        {
            return null;
        }
    }
}
