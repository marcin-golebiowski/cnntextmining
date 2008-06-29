using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TextMining.Clastering;
using TextMining.DataLoading;
using TextMining.Experiments;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Evaluation.Experiments
{
    class Experiment_DBSCAN : IExperiment
    {
        private readonly SqlConnection conn;

        public SqlConnection Conn
        {
            get { return conn; }
        }

        public SqlConnection _ { get; set; }

        public Experiment_DBSCAN(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Run()
        {
            int newsCount = 500;
            int maxLen = 8000;

            var dataFetcher = new DataFetcher(conn);
            List<News> tmp = dataFetcher.GetAllNews();

            Random r = new Random();
            List<News> news = new List<News>();

            WordList wl = new WordList();

            for (int i = 0; i < newsCount; i++)
            {
                int x = r.Next(tmp.Count);
                news.Add(tmp[x]);

                tmp[x].words = wl.getWordList(tmp[x].rawData);
                tmp.RemoveAt(x);
            }



            var stats = new WordsStats(news);
            stats.Compute();

            var comparator = new CosinusMetricComparator();


            var algorithm = new Dbscan(comparator, stats, maxLen);

            /*
            for (int i = 0; i < newsCount; i++)
            {
                double sum = 0.0;
                Vector curr = new Vector(stats, news[i], maxLen);
                curr.BuildVector();
                for (int j = 0; j < newsCount; j++)
                {
                    Vector toComp = new Vector(stats, news[j], maxLen);
                    toComp.BuildVector();
                    sum += comparator.Compare(curr, toComp);
                }
                Console.WriteLine(sum / newsCount);
            }
            */
            

            Console.WriteLine("Starting Dbscan");
            List<List<News>> sets = algorithm.Compute(news, 0.0230, 3);
            Console.WriteLine("Dbscan end");

            ExperimentStats.PrintStats(sets);

            Console.WriteLine("Loading assingments..");
            TopicOriginalAssigment ass = new TopicOriginalAssigment(conn);
            ass.Load();


            DefaultEvaluator eval = new DefaultEvaluator(ass);

            Console.WriteLine("Starting eval");
            Console.WriteLine(eval.GetScore(sets));
            Console.WriteLine("End eval");

            
            foreach (List<News> gr in sets)
            {
                Console.WriteLine("SET ");
                foreach (News n in gr)
                    Console.WriteLine(n.url);
                Console.WriteLine("------------------");
            }
            
        }
    }
}
