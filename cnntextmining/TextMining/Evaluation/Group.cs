using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.Evaluation
{
    public class Group
    {
        private readonly string name;

        private readonly List<News> news = new List<News>();

        public Group(string name) : this(name, null)
        {
        }

        public Group(string name, IEnumerable<News> initial)
        {
            this.name = name;
            if (initial != null)
            {
                news.AddRange(initial);
            }
        }

        public News this[int i]
        {
            get
            {
                return news[i];
            }
            set
            {
                news[i] = value;
            }
        }
        

        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get
            {
                return news.Count;
            }
        }

        public void Clear()
        {
            news.Clear();
        }

        public void AddRange(List<News> toAdd)
        {
            news.AddRange(toAdd);
        }

        public void Add(News news1)
        {
            news.Add(news1);
        }
    }
}
