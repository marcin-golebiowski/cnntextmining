using System;
using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.TextTools
{
    public class Vector
    {
        private readonly WordsStats stats;
        private readonly News news;
        private readonly int maxlen;
        private readonly Dictionary<string, double> items = new Dictionary<string, double>();

        public Vector(WordsStats stats, News news, int maxlen)
        {
            this.stats = stats;
            this.news = news;
            this.maxlen = maxlen;
        }


        public Dictionary<string, double> Items
        {
            get { return items; }
        }

        public double GetLength()
        {
            double result = 0;
            foreach (string word in items.Keys)
            {
                double val = items[word];
                result += val*val;
            }

            result = Math.Sqrt(result);

            return result;
        }


        public string URL
        {
            get { return VectorNews.url; }
        }

        public News VectorNews
        {
            get { return news; }
        }

        public void BuildVector()
        {
            List<Pair> list = new List<Pair>();

            foreach (string word in VectorNews.words)
            {
                Pair p = new Pair();
                p.word = word;
                p.val = stats.GetTF(word, VectorNews.url) * stats.GetIDF(word);
                list.Add(p);
            }

            list.Sort(new Comp());


            if (list.Count > maxlen)
            {
                list.RemoveRange(maxlen - 1, list.Count - (maxlen - 1));
            }

            
            foreach(Pair p in list)
            {
                Items[p.word] = p.val;
            }
        }


        public void Trim()
        {

            if (items.Count > maxlen)
            {
                List<Pair> list = new List<Pair>();

                foreach (KeyValuePair<string, double> item in items)
                {
                    Pair p = new Pair();
                    p.word = item.Key;
                    p.val = item.Value;
                    list.Add(p);
                }
                list.Sort(new Comp());
                list.RemoveRange(maxlen - 1, list.Count - (maxlen - 1));

                Items.Clear();


                foreach (Pair p in list)
                {
                    Items[p.word] = p.val;
                }
            }
        }
    }

    class Comp : IComparer<Pair>
    {
        public int Compare(Pair x, Pair y)
        {
            if (x.val > y.val)
            {
                return -1;
            }

            if (x.val < y.val)
            {
                return 1;
            }

            return 0;
        }
    }


    class Pair
    {
        public string word;
        public double val;
    }
}