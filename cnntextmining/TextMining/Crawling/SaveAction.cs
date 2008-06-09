using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TextMining.Model;

namespace TextMining.Crawling
{
    class SaveAction : IAction
    {
        private const string sql = "INSERT INTO News(URL, Words,Links,RawData, Created) VALUES(@url, @words, @links, @raw, @date)";
        readonly SqlConnection conn;

        public SaveAction(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Do(CNNPage page)
        {
            try
            {
                using (SqlCommand command
                    = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@url", page.uri);
                    command.Parameters.AddWithValue("@words", ToOneString(page.words, 4000));
                    command.Parameters.AddWithValue("@links", ToOneString(page.allLinks, 1000));
                    command.Parameters.AddWithValue("@raw", page.pureText);
                    command.Parameters.AddWithValue("@date", DateTime.Now.ToString());

                    command.ExecuteNonQuery();

                    Console.WriteLine("Saved");
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string ToOneString(IEnumerable<string> words, int maxlen)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string s in words)
            {
                builder.Append(s);
                builder.Append(";");
            }

            string str = builder.ToString();

            if (str.Length > maxlen)
            {
                return str.Substring(0, maxlen);
            }

            return str;
        }

        private static string ToOneString(IEnumerable<Uri> list, int maxlen)
        {
            StringBuilder builder = new StringBuilder();

            foreach (Uri s in list)
            {
                builder.Append(s.ToString());
                builder.Append(";");
            }
            string str = builder.ToString();

            if (str.Length > maxlen)
            {
                return str.Substring(0, maxlen);
            }
            return str;
        }
    }
}