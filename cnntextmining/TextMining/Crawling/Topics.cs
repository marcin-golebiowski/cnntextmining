using System.Data.SqlClient;

namespace TextMining.Crawling
{
    public class Topics
    {
        private readonly SqlConnection conn;

        public Topics(SqlConnection conn)
        {
            this.conn = conn;
        }

        public void AddTopicURL(string topicURL, string linkURL)
        {
            using (var command2
                = new SqlCommand("INSERT INTO dbo.[Topics](TopicURL, LinkURL) VALUES(@t, @l)",
                                 conn))
            {
                command2.Parameters.AddWithValue("@t", topicURL);
                command2.Parameters.AddWithValue("@l", linkURL);
                command2.ExecuteNonQuery();
            }
        }
    }
}