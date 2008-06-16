using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TextMining.Clastering;
using TextMining.Crawling;
using TextMining.Evaluation.Experiments;
using TextMining.Experiments;
using TextMining.Model;
using TextMining.TextTools;
using DataFetcher=TextMining.DataLoading.DataFetcher;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
         @"Data Source=NEVERLAND\SQLEXPRESS;Initial Catalog=TextMining;user=marek;password=marek";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //var exp1 = new Experiment1(conn);
                //exp1.Run();

                var exp2 = new Experiment_DBSCAN(conn);
                exp2.Run();
            }
        }
    }
}
