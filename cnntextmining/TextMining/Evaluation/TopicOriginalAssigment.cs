using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TextMining.Evaluation
{
    public class TopicOriginalAssigment
    {
        private readonly SqlConnection connection;

        // topic url -> id of topic
        private readonly Dictionary<string, int> topics;

        private readonly Dictionary<int, string> topicsRev;

        // news -> a list of id's of topics
        private readonly Dictionary<string, List<int>> assigment;

        public TopicOriginalAssigment(SqlConnection connection)
        {
            assigment = new Dictionary<string, List<int>>();
            topics = new Dictionary<string, int>();
            topicsRev = new Dictionary<int, string>();
            this.connection = connection;
        }

        public List<int> GetTopicsForNews(string newsURL)
        {
            if (assigment.ContainsKey(newsURL))
            {
                return assigment[newsURL];
            }
            return new List<int>();
        }

        public string GetTopicURL(int topicURL)
        {
            if (topicsRev.ContainsKey(topicURL))
            {
                return topicsRev[topicURL];
            }
            return "";
        }

        public void Load()
        {
            Console.WriteLine("Topic links loading ...");
            assigment.Clear();
            topics.Clear();

            int topicNumbers = 0;

            using (var command
                = new SqlCommand("SELECT * FROM dbo.[Topics] WHERE LinkURL Like '%.html'", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            string topicURL = reader["TopicURL"].ToString();
                            string linkURL = reader["LinkURL"].ToString();

                            int topicID;

                            if (!topics.ContainsKey(topicURL))
                            {
                                topics[topicURL] = topicNumbers;
                                topicsRev[topicNumbers] = topicURL;
                                topicID = topicNumbers;
                                topicNumbers++;
                            }
                            else
                            {
                                topicID = topics[topicURL];
                            }



                            if (assigment.ContainsKey(linkURL))
                            {
                                if (!assigment[linkURL].Contains(topicID))
                                {
                                    assigment[linkURL].Add(topicID);
                                }
                            }
                            else
                            {
                                assigment[linkURL] = new List<int> {topicID};
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Topics links loaded.");
        }


        public bool AreInTheSameTopic(string link1, string link2)
        {
            if (!assigment.ContainsKey(link1)) return false;
            if (!assigment.ContainsKey(link2)) return false;


            foreach (int id1 in assigment[link1])
            {
                foreach (int id2 in assigment[link2])
                {
                    if (id1 == id2) return true;
                }
            }
            return false;
        }
    }
}