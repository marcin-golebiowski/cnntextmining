using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.DataLoading;
using TextMining.Model;
using TextMining.TextTools;
using TextMining.Clastering;



namespace TextMining.Evaluation.Experiments
{
    class ExperimentHierarchical : IExperiment
    {

        private readonly SqlConnection conn;

        public SqlConnection Conn
        {
            get { return conn; }
        }

        public SqlConnection _ { get; set; }

        public ExperimentHierarchical(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Run()
        {
            int newsCount = 500;
            int maxLen = 8000;
       
            List<News> news =  DataStore.Instance.GetAllNews(newsCount);

            

            var comparator = new CosinusMetricComparator();


            

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
            

            Console.WriteLine("Starting hierarchical");
            List<News> toCompute = Words.ComputeWords(news);

            var stats = new WordsStats(toCompute);
            stats.Compute();

            var algorithm = new Hierarchical(comparator, stats, maxLen);

            Console.WriteLine("start");
            List<Group> sets = algorithm.Compute(new Group("aa",toCompute), 20, Hierarchical.Distance.AVG);
            Console.WriteLine("Dbscan end");

            ExperimentStats.GetGroupCountString(sets);

            Console.WriteLine("Loading assingments..");
            TopicOriginalAssigment ass = new TopicOriginalAssigment(conn);
            ass.Load();


            foreach (Group gr in sets)
            {
                Console.WriteLine("SET ");
                for (int i = 0; i < gr.Count; i++ )
                    Console.WriteLine(gr[i].url);
                Console.WriteLine("------------------");
            }
            
        }
    }
}
