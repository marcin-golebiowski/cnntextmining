using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.Crawling;
using TextMining.Evaluation;
using TextMining.Evaluation.Experiments;
using TextMining.Model;
using TextMining.TextTools;
using DataFetcher=TextMining.DataLoading.DataFetcher;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
         @"Data Source=NEVERLAND\SQLEXPRESS;Initial Catalog=TextMiningNew;user=marek;password=marek";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // preprocessing
                var fetcher = new DataFetcher(conn);
                WordsStats stats = new WordsStats(WordsSetter.ComputeWords(fetcher.GetAllNews()));
                stats.Compute();

                GroupFactory factory = new GroupFactory(conn);
                Group start = factory.CreateGroupWithAllNews();

                EuclidesMetricComparator comp = new EuclidesMetricComparator();
                Kmeans kmeans = new Kmeans(comp, stats, 10);



                List<Group> groups =  kmeans.Compute(start, 10, 10);


                ExperimentStats.PrintStats(groups);


                //var assigment = new TopicOriginalAssigment(conn);
                //assigment.Load();

                //DataFetcher fetcher = new DataFetcher(conn);
                //List<News> news =  fetcher.GetAllNews();

                //foreach (var info in news)
                //{
                    //Console.WriteLine(info.url);
                    //Console.WriteLine(assigment.GetTopicsForNews(info.url).Count);
                //}
                //Console.WriteLine("Dostêpne: " + news.Count + " newsów");

                //List<string> topics = fetcher.GetTopics();

                //Console.WriteLine("Liczba topików: " + topics.Count);
                


                //var exp1 = new Experiment1(conn);
                //exp1.Run();

                //var exp2 = new Experiment_DBSCAN(conn);
                //exp2.Run();

                /*CNNPage page = new CNNPage("http://edition.cnn.com/2003/TECH/space/07/30/sprj.colu.columbia.probe/index.html");

                Console.WriteLine(page.pureText);
                Console.WriteLine();
                foreach(Uri link in page.allLinks)
                    Console.WriteLine(link);

                */

            }
        }
    }
}
