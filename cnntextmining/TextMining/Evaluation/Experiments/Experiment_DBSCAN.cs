using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.DataLoading;
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
            Console.WriteLine("newsy przed pobraniem");
            List<News> tmp = dataFetcher.GetAllNews();

            Console.WriteLine("newsy pobrane");

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

            var comparator = new JaccardMetricCompartator();


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

            Group gr = new Group("chujowa grupka", news);

            Console.WriteLine("Starting Dbscan");
            List<Group> sets = algorithm.Compute(gr, 0.23, 2);
            Console.WriteLine("Dbscan end");

            ExperimentStats.GetGroupCountString(sets);

            Console.WriteLine("Loading assingments..");
            TopicOriginalAssigment ass = new TopicOriginalAssigment(conn);
            ass.Load();


            DefaultEvaluator eval = new DefaultEvaluator(ass);

            Console.WriteLine("Starting eval");
            Console.WriteLine(eval.GetScore(sets));
            Console.WriteLine("End eval");

            int sum = 0;

            foreach (Group g in sets)
            {
                Console.Write(g.Count + ";");
                sum += g.Count;
            }
            Console.WriteLine("sum = " + sum);
            
            foreach (Group ggg in sets)
            {
                Console.WriteLine("SET " + gr.Count);
                for (int i = 0; i < ggg.Count; i++ )
                    Console.WriteLine(ggg[i].url);
                Console.WriteLine("------------------");
            }
            
        }
    }
}
