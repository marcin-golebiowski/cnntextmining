using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.Crawling;
using TextMining.Experiments;
using TextMining.Model;
using TextMining.TextTools;
using DataFetcher=TextMining.DataLoading.DataFetcher;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
         @"Data Source=.\SQLEXPRESS;Initial Catalog=TextMining;Integrated Security=True";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();


                //TopicOriginalAssigment oryginalAssigment = new TopicOriginalAssigment(conn);
                //oryginalAssigment.Load();


                var dataFetcher = new DataFetcher(conn);
                List<News> news = dataFetcher.GetAllNews(true, 4000);
                WordsStats stats = new WordsStats(news);
                stats.Compute();

                DefaultNewsComparator comparator = new DefaultNewsComparator(stats);

            
                Kmeans algorithm = new Kmeans(comparator, stats, 50);

                Console.WriteLine("Starting KMeans");
                List<List<News>> sets =  algorithm.Compute(news, 10, 2);
                Console.WriteLine("KMeans end");

                ExperimentStats.PrintStats(sets);
                


                //var crawler = new Crawler(new SaveAction(conn) , conn);
                //crawler.Run();


                /*int numberOfNews = 1000;
                int numberOfComparision = 1000;
                int maxVectorLen = 500;

                // 21 sec - 100'000
                // 200 sec - 1'000'000

                // 30'000 * 500 = 15'000'000

                Console.WriteLine("Loading start");
                var dataFetcher = new DataFetcher(conn);
                List<News> news = dataFetcher.GetAllNews(true, numberOfNews);
                Console.WriteLine("Loading end");

                WordsStats freq = new WordsStats(news);
                freq.Compute();
              
                Random rand = new Random();

                DateTime start = DateTime.Now;

                DefaultNewsComparator comparer = new DefaultNewsComparator(freq);

                for (int i = 0; i < numberOfComparision; i++)
                {
                    Vector v1 = new Vector(freq,
                        news[rand.Next()% numberOfNews], maxVectorLen);
                    v1.BuildVector();


                    Vector v2 = new Vector(freq,
                        news[rand.Next() % numberOfNews], maxVectorLen);
                    v2.BuildVector();

                    DefaultNewsComparator comp = new DefaultNewsComparator(freq);

                    Console.WriteLine(comp.Compare(v1, v2));


                }


                DateTime end = DateTime.Now;
                Console.WriteLine(end - start);
                 */
            }
        }
    }
}
