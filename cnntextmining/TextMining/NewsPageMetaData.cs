using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining
{
    class NewsPageMetaData
    {
        private CNNPage.Topic topic;
        private DateTime date;
        private string newsTitle;

        public CNNPage.Topic Topic
        {
            get
            {
                return topic;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string NewsTitle
        {
            get
            {
                return newsTitle;
            }
        }

        // Only give uri to news page! You have to care about it!
        public NewsPageMetaData(string uri)
        {
            string[] parts = uri.Split(new char[] { '/' });

            topic = (CNNPage.Topic)Enum.Parse(typeof(CNNPage.Topic), parts[4]);
            date = new DateTime(Int32.Parse(parts[3]), Int32.Parse(parts[parts.Length - 4]), Int32.Parse(parts[parts.Length - 3]));
            newsTitle = parts[parts.Length - 2];
        }
    }
}
