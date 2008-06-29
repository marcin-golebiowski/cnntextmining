using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using TextMining.DataLoading;
using TextMining.Evaluation;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    public class Metis
    {
        private readonly WordsStats stats;
        private readonly IComparator comp;
        private readonly SqlConnection conn;

        public Metis(WordsStats stats, IComparator comp, SqlConnection conn)
        {
            this.stats = stats;
            this.comp = comp;
            this.conn = conn;
            
        }

        public List<Group> Compute(Group news, int K)
        {
            CreateDataFile(news, @"C:\DEV\PROJECTS\Hurtownie\TextMining\exp.graph" );

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\DEV\PROJECTS\Hurtownie\TextMining\metis\kmetis.exe";
            p.StartInfo.Arguments = @"C:\DEV\PROJECTS\Hurtownie\TextMining\exp.graph " + K;
            p.StartInfo.WorkingDirectory = @"C:\DEV\PROJECTS\Hurtownie\TextMining\";
           



            p.Start();
            string res = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return ReadMetisResult(@"C:\DEV\PROJECTS\Hurtownie\TextMining\exp.graph", K);
        }

        

        public void CreateDataFile(Group news, string path)
        {
            var writer = new StreamWriter(path);
            writer.Write(news.Count + " " + news.Count * (news.Count - 1)/2 + " 1");
            writer.Write("\n");
            

            double[,] val = computeDistances(news);

            for (int i = 0; i < news.Count; i++)
            {
                for (int j = 0; j < news.Count; j++)
                {
                    int tmp = (int)(val[i, j] * 100000);

                    if (val[i, j] != val[j, i])
                    {
                        Console.WriteLine("ERROR");
                    }

                    if (i != j)
                    {
                        writer.Write(" " + (j + 1) + " " + tmp);
                    }
                }
                
                writer.Write(" \n");
            }

            writer.Close();

            writer = new StreamWriter(path + ".news");
            for (int i = 0; i < news.Count; i++)
            {
                writer.WriteLine(news[i].url);
            }
            writer.Close();
        }


        private double[,] computeDistances(Group news)
        {
            double[,] distances = new double[news.Count, news.Count];

            for (int i = news.Count - 1; i >= 0; i--)
            {
                Vector row = new Vector(stats, news[i], 1000);
                row.BuildVector();

                for (int j = 0; j < i; j++)
                {
                    Vector col = new Vector(stats, news[j], 1000);
                    col.BuildVector();

                    distances[i, j] = comp.Compare(row, col);
                    distances[j, i] = distances[i, j];
                }
            }

            return distances;
        }



        public List<Group> ReadMetisResult(string path, int K)
        {
            List<News> newses = new List<News>();


            var reader = new StreamReader(path + ".news");
            string line;

            DataFetcher f = new DataFetcher(conn);

            while ((line = reader.ReadLine()) != null)
            {
                News m = f.GetNews(line);

                newses.Add(m);
            }
            reader.Close();

            reader = new StreamReader(path + ".part." + K);
            var result = new List<Group>();

            for (int i = 0; i < K; i++)
            {
                result.Add(new Group(""));
            }

            int r = 0;
            while ((line = reader.ReadLine()) != null)
            {
                result[Convert.ToInt32(line)].Add(newses[r]);
                r++;
            }

            return result;
        }

    }
}
