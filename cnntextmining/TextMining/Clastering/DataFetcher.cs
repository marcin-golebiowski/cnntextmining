using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Crawling;

namespace TextMining.Clastering
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
            var result = new List<News>();

            int count = 0;
            using (var command
                    = new SqlCommand("SELECT * FROM dbo.[News]", conn))
            {
                using (SqlDataReader reader =  command.ExecuteReader())
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
                            result.Add(element);

                            count++;

                            if (count % 1000 ==0)
                            {
                                    Console.WriteLine(count);
                            }
                        }
                    }
                }
            }

            return result;
        }



    }
}
