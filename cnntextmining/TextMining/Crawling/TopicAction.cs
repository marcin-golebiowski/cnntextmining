using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using TextMining.DataProcessing;

namespace TextMining.Crawling
{
    class TopicAction 
    {
        private readonly SqlConnection conn;

        public TopicAction(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Do(string topicURL)
        {
            var topics = new Topics(conn);

            Regex regex = new Regex("<a href=\"http:[^\"#?]+[\"#?]");
            string content = Downloader.FetchPage(topicURL);

            foreach (Match m in regex.Matches(content))
            {
                Uri tmp = new Uri(m.Value.Substring(9, m.Value.Length - 10));

                if (tmp.OriginalString.StartsWith("http://edition.cnn.com/"))
                {
                    topics.AddTopicURL(topicURL, tmp.OriginalString);
                }
            }
        }

    }
}