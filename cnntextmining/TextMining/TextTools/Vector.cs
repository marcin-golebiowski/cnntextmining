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


        public string URL
        {
            get { return news.url; }
        }

        public void BuildVector()
        {
            List<Pair> list = new List<Pair>();

            foreach (string word in news.words)
            {
                Pair p = new Pair();
                p.word = word;
                p.val = stats.GetTF(word, news.url) * stats.GetIDF(word);
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