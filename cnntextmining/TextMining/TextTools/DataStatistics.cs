using System;
using System.Collections.Generic;
using System.Text;
using TextMining.Model;

namespace TextMining.TextTools
{
    class DataStatistics
    {
        public string getData(List<News> news, WordsStats stats, int maxLen, IComparator comp)
        {
            StringBuilder sb = new StringBuilder();

            double standardDeviation;
            double average;
            double aritm;
            double min = double.MaxValue;
            double max = double.MinValue;

            double sum = 0.0;
            int c = 0;
            int zeroCount = 0;

            int cLinks = 0;

            double[,] dist = new double[news.Count, news.Count];

            for (int i = 0; i < news.Count; i++)
            {
                Vector x = new Vector(stats, news[i], maxLen);
                x.BuildVector();
                for (int j = 0; j < i; j++)
                {
                    Vector y = new Vector(stats, news[j], maxLen);
                    y.BuildVector();

                    double res = comp.Compare(x, y);
                    dist[i, j] = res;

                    if(res == 0)
                        zeroCount++;

                    min = Math.Min(min, res);
                    max = Math.Max(max, res);

                    sum += res;
                    c++;

                    //cLinks += commonLinks(news[i], news[j]);

                    if (c % 10000 == 0)
                        Console.WriteLine(c);

                    //Console.WriteLine(cLinks); 
                }
            }

            average = sum / c;

            double devSum = 0.0;
            double artSum = 0.0;
            c = 0;

            for (int i = 0; i < news.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    devSum += dist[i, j] * dist[i, j];
                    artSum += Math.Abs(dist[i, j] - average);
                    c++;
                }
            }

            standardDeviation = Math.Sqrt(   (devSum / c) - (average*average)   );
            aritm = artSum / c;

            sb.Append("Data information:\n");
            sb.Append("Minimum distance = " + min.ToString() + "\n");
            sb.Append("Maximum distance = " + max.ToString() + "\n");
            sb.Append("Average distance = " + average.ToString() + "\n");
            sb.Append("Zero count = " + zeroCount + "\n");
            sb.Append("Standard deviation = " + standardDeviation.ToString() + "\n");
            sb.Append("Arithmetic median = " + aritm.ToString() + "\n");
            //sb.Append("Common links average = " + (cLinks / c) + "\n");
            sb.Append("End data information\n");

            return sb.ToString();
        }

        private int commonLinks(News x, News y)
        {
            int c = 0;
            foreach (string u1 in x.links)
            {
                foreach (string u2 in y.links)
                {
                    if (u1 == u2)
                        c++;
                }
            }
            Console.WriteLine(x.links.Count + " " + y.links.Count);
            return c;
        }




    }
}
