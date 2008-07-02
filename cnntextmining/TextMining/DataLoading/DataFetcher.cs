using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
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


        public List<News> GetAllNewsFromFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] content = new byte[fs.Length];
            fs.Read(content, 0, content.Length);
            fs.Close();
            return (List<News>) Serialization.Deserialize(content, StreamingContextStates.File);
        }


        public void SaveNewsFromFile(List<News> news, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            byte[] serialized = Serialization.Serialize(news, StreamingContextStates.File);
            fs.Write(serialized, 0, serialized.Length);
            fs.Close();
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
                = new SqlCommand("SELECT TOP " + newsToGet + " [URL], [Words], [RawData], [Links], [TopicURL] FROM dbo.[News] JOIN dbo.[Topics] ON LinkURL = URL WHERE URL Like 'http://edition.cnn.com/%'", conn))
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
                            element.topicUrl = reader["TopicURL"].ToString();
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

        public News GetNews(string newsURL)
        {
            News element = null;
            using (var command
                = new SqlCommand("SELECT [URL], [Words], [RawData], [Links], [TopicURL] FROM dbo.[News] JOIN dbo.[Topics] ON LinkURL = URL WHERE URL = @url", conn))
            {
                command.Parameters.AddWithValue("@url", newsURL);
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

                            element = new News();
                            element.url = url;
                            element.topicUrl =  reader["TopicURL"].ToString();
                            element.words.AddRange(words.Split(';'));
                            element.links.AddRange(links.Split(';'));
                            element.rawData = rawData;
                           
                        }

                        reader.Close();
                    }

                }
            }

            return element;
        }
    }
}
    