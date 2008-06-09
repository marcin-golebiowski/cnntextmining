using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Model;
using TextMining.TextTools;
using DataFetcher=TextMining.DataLoading.DataFetcher;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
         @"Data Source=IBM-PC\SQLEXPRESS;Initial Catalog=TextMining;Integrated Security=True";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //var crawler = new Crawler(new SaveAction(conn) , conn);
               //crawler.Run();


                var dataFetcher = new DataFetcher(conn);
                DateTime start = DateTime.Now;
                List<News> news = dataFetcher.GetAllNews();

                WordsStats freq = new WordsStats(news);
                freq.Compute();


                DateTime end = DateTime.Now;
                Console.WriteLine(end - start);

            }
        }
    }
}
