using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.Evaluation;
using TextMining.TextTools;
using DataFetcher=TextMining.DataLoading.DataFetcher;
using TextMining.Evaluation.Experiments;

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

                Console.WriteLine("polaczenie ok");

                // preprocessing
               // var fetcher = new DataFetcher(conn);
               // WordsStats stats = new WordsStats(Words.ComputeWords(fetcher.GetAllNews()));
               // stats.Compute();

               // Console.WriteLine("Words Stats - computed");
               // GroupFactory factory = new GroupFactory(conn);


                var expr = new ExperimentKMeans(conn);
                expr.Run();





                /*
                var exp = new MetricsStatistics(conn);
                exp.Run();
                */
                /*
                var topics = new List<string>();
                //topics.Add(@"http://topics.edition.cnn.com/topics/astronomy");
                //topics.Add(@"http://topics.edition.cnn.com/topics/armed_forces");
                //topics.Add(@"http://topics.edition.cnn.com/topics/genetics");
                topics.Add(@"http://topics.edition.cnn.com/topics/religion");

                Group initialGroup = factory.CreateGroupWithNewsFromTopics(topics);
                CosinusMetricComparator comp = new CosinusMetricComparator();


                var metis = new Metis(stats, comp, conn);

                List<Group> groups = metis.Compute(initialGroup, 4);
                ExperimentStats.PrintDetailsString(groups);
                

                
                
                //Hierarchical algorithm = new Hierarchical(comp, stats, 4000);

                //Dbscan scan = new Dbscan(comp, stats, 1000)p;

                //List<Group> groups = algorithm.Compute(start, 10);

                //ExperimentStats.PrintDetailsString(groups);


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
