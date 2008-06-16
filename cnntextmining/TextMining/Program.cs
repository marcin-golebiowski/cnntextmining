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
         @"Data Source=.\SQLEXPRESS;Initial Catalog=TextMining;Integrated Security=True";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var exp1 = new Experiment1(conn);
                exp1.Run();
            }
        }
    }
}
