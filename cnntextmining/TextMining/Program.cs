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
         @"Data Source=IBM-PC\SQLEXPRESS;Initial Catalog=TextMining;User ID=marek;Password=marek";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();


                //var crawler = new Crawler(new SaveAction(conn) , conn);
               //crawler.Run();


                int n = 5000;
                int m = 100000;

                // 21 sec - 100'000
                // 200 sec - 1'000'000

                // 30'000 * 500 = 15'000'000

                var dataFetcher = new DataFetcher(conn);
                List<News> news = dataFetcher.GetAllNews(true, n);
                

                WordsStats freq = new WordsStats(news);
                freq.Compute();

                Random rand = new Random();

                DateTime start = DateTime.Now;

                DefaultNewsComparator comparer = new DefaultNewsComparator(freq);

                for (int i = 0; i < m; i++)
                {
                    comparer.Compare(news[rand.Next()% n], news[rand.Next()% n]);
                }

                
                DateTime end = DateTime.Now;
                Console.WriteLine(end - start);
               
            }
        }
    }
}
