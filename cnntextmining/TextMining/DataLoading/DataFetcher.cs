using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Model;

namespace TextMining.DataLoading
{
    class DataFetcher
    {
        private readonly SqlConnection conn;

        public DataFetcher(SqlConnection conn)
        {
            this.conn = conn;
        }

        public List<News> GetAllNews()
        {
            return GetAllNews(false, int.MaxValue);
        }

        public List<News> GetAllNews(string topicURL)
        {
            var result = new List<News>();

            using (var command
                = new SqlCommand("SELECT N.[URL], [Words], [RawData], [Links], [TopicURL] FROM dbo.[News] N JOIN dbo.[Topics] T ON  T.LinkURL = N.URL  WHERE TopicURL = @topic AND URL Like 'http://edition.cnn.com/%'", conn))
            {
                command.Parameters.AddWithValue("@topic", topicURL);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //reader.
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string url = reader["URL"].ToString();
                            string words = reader["Words"].ToString();
                            string links = reader["Links"].ToString();
                            string rawData = reader["RawData"].ToString();


                            var element = new News();
                            element.url = url;
                            element.words.AddRange(words.Split(';'));
                            element.links.AddRange(links.Split(';'));
                            element.rawData = rawData;
                            element.topicUrl = reader["TopicURL"].ToString();
                            result.Add(element);
                        }

                        reader.Close();
                    }

                }
            }
            return result;
        }

        public List<News> GetAllNews(bool trim, int newsToGet)
        {
            var result = new List<News>();

            using (var command
                = new SqlCommand("SELECT TOP " + newsToGet + " [URL], [Words], [RawData], [Links] FROM dbo.[News] WHERE URL Like 'http://edition.cnn.com/%'", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string url = reader["URL"].ToString();
                            string words = reader["Words"].ToString();
                            string links = reader["Links"].ToString();
                            string rawData = reader["RawData"].ToString();


                            var element = new News();
                            element.url = url;
                            element.words.AddRange(words.Split(';'));
                            element.links.AddRange(links.Split(';'));
                            element.rawData = rawData;
                            result.Add(element);
                        }

                        reader.Close();
                    }

                }
            }
            return result;
        }

        public List<string> GetTopics()
        {
            var result = new List<string>();

            using (var command
                = new SqlCommand("SELECT Distinct TopicURL FROM dbo.[Topics]", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string url = reader["TopicURL"].ToString();
                          
                            result.Add(url);
                        }

                        reader.Close();
                    }

                }
            }
            return result;
        }
    }
}
    