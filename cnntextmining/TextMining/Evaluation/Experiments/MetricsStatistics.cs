using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TextMining.TextTools;
using TextMining.DataLoading;
using TextMining.Model;

namespace TextMining.Evaluation.Experiments
{
    class MetricsStatistics : IExperiment
    {
        private readonly SqlConnection conn;

        public SqlConnection Conn
        {
            get { return conn; }
        }

        public SqlConnection _ { get; set; }

        public MetricsStatistics(SqlConnection conn)
        {
            this.conn = conn;
        }


        public void Run()
        {
            var dataFetcher = new DataFetcher(conn);
            Console.WriteLine("Downloading news for database...");
            List<News> news = dataFetcher.GetAllNews(true, 700);
            Console.WriteLine("News downloaded " + news.Count);

            news = Words.ComputeWords(news);

            /*
            CosinusMetricComparator cos = new CosinusMetricComparator();
            EuclidesMetricComparator eu = new EuclidesMetricComparator();
            JaccardMetricCompartator ja = new JaccardMetricCompartator();
            */
 
            var stats = new WordsStats(news);
            stats.Compute();

            DataStatistics ds = new DataStatistics();

            Console.WriteLine("Jaccard metric information: ");
            Console.WriteLine(ds.getData(news, stats, 5000,  new JaccardMetricCompartator()));

            Console.WriteLine("Cosinus metric information: ");
            Console.WriteLine(ds.getData(news, stats, 5000, new CosinusMetricComparator()));

            Console.WriteLine("Euclides metric information: ");
            Console.WriteLine(ds.getData(news, stats, 5000, new EuclidesMetricComparator()));

            
        }

    }
}
