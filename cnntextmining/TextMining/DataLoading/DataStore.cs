using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using TextMining.Model;

namespace TextMining.DataLoading
{
    class DataStore
    {
        private readonly string path;
        private readonly Dictionary<string, News> dict = new Dictionary<string, News>();
        private readonly Dictionary<string, List<News>> topics = new Dictionary<string, List<News>>();

        private List<News> newsss;
        private static DataStore dataStore;

        private DataStore(string  path)
        {
            this.path = path;
            Load();
        }

        public static DataStore Instance
        {
            get
            {
                if (dataStore != null)
                {
                    return dataStore;
                }
                return dataStore = new DataStore(ConfigurationManager.AppSettings["database"]);
            }
        }



        private void Load()
        {
            newsss = GetAllNewsFromFile();
            foreach (News n in newsss)
            {
                dict[n.url] = n;
                if (!topics.ContainsKey(n.topicUrl))
                {
                    topics[n.topicUrl] = new List<News>();                
                }
                topics[n.topicUrl].Add(n);
            }
        }


        public List<News> GetAllNews()
        {
            return newsss;
        }


        public List<News> GetAllNewsFromFile()
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



        public List<News> GetAllNewsFromDatabase(string topicURL, SqlConnection conn)
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

        public List<News> GetAllNews(int newsToGet)
        {
            List<News> result = new List<News>();

            for (int i = 0; i < newsToGet; i++ )
            {
                result.Add(newsss[i]);
            }

            return result;
        }

        public List<News> GetAllNewsFromDatabase(int newsToGet, SqlConnection conn)
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

        public List<string> GetTopicsFromDatabase(SqlConnection conn)
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
            if (dict.ContainsKey(newsURL))
            {
                return dict[newsURL];
            }
            return null;
        }



        public News GetNewsFromDatabase(string newsURL, SqlConnection conn)
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


        public List<News> GetAllNews(string topic)
        {

            List<News> result = new List<News>();

            for (int i = 0; i < newsss.Count; i++)
            {
                if (newsss[i].topicUrl == topic)
                {
                    result.Add(newsss[i]);
                }
            }

            return result;
        }


        public double GetTopicSize(string topic)
        {
            if (topics.ContainsKey(topic))
            {
                return topics[topic].Count;
            }
            return 0;
        }
    }
}
    