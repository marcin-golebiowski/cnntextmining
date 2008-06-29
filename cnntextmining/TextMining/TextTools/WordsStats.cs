using System.Collections.Generic;
using TextMining.Model;

namespace TextMining.TextTools
{
    public class WordsStats
    {
        private readonly List<News> news;

        // word -> count
        private readonly Dictionary<string, int> wordCount =
            new Dictionary<string, int>();

        // url -> news ...
        private readonly Dictionary<string, News> dict =
            new Dictionary<string, News>();


        // word -> dict of news -> count
        private readonly Dictionary<string, Dictionary<string, int>> wordDocumentCount = new Dictionary<string, Dictionary<string, int>>();


        public WordsStats(List<News> news)
        {
            this.news = news;
        }


        public List<string> GetMostPopularWords(int count)
        {
            var res = new List<string>();

            List<Pair> tmp = new List<Pair>();

            foreach (KeyValuePair<string, int> pair in wordCount)
            {
                var p = new Pair();
                p.val = pair.Value;
                p.word = pair.Key;

                tmp.Add(p);
            }

            tmp.Sort(new Comp());
            

            for (int i = 0; i < count; i++)
            {
                res.Add(tmp[i].word);
            }

            return res;
        }


        public List<string> GetMostWordsD(int count)
        {
            var res = new List<string>();

            List<Pair> tmp = new List<Pair>();

            foreach (KeyValuePair<string, Dictionary<string, int>> pair in wordDocumentCount)
            {
                var p = new Pair();
                p.val = pair.Value.Count;
                p.word = pair.Key;

                tmp.Add(p);
            }

            tmp.Sort(new Comp());


            for (int i = 0; i < count; i++)
            {
                res.Add(tmp[i].word);
            }

            return res;
        }

        public void Compute()
        {
          
            foreach (var item in news)
            {
                dict[item.url] = item;

                foreach (var word in item.words)
                {
                    if (wordCount.ContainsKey(word))
                    {
                        wordCount[word] += 1;
                    }
                    else
                    {
                        wordCount[word] = 1;
                    }

                    if (wordDocumentCount.ContainsKey(word))
                    {
                        if (!wordDocumentCount[word].ContainsKey(item.url))
                        {
                            wordDocumentCount[word][item.url] = 1; 
                        }
                        else
                        {
                            wordDocumentCount[word][item.url] += 1;
                        }
                    }
                    else
                    {
                        wordDocumentCount[word] = new Dictionary<string, int>();
                        wordDocumentCount[word][item.url] = 1;
                    }
                }
            }

        
        }


        public double GetTF(string word, string url)
        {
            // liczba wystapien w dokumencie / liczbe wszystkich slow w dokumencie

            if (!wordDocumentCount.ContainsKey(word))
            {
                return 0;
            }

            double wordFreq = 0;

            if (wordDocumentCount[word].ContainsKey(url))
            {
                wordFreq = wordDocumentCount[word][url];
            }

            double words = dict[url].words.Count;

            if (words != 0)
            {
                return wordFreq/words;
            }

            return 0;
        }

        public double GetIDF(string word)
        {

            if (!wordDocumentCount.ContainsKey(word))
            {
                return 0;
            }


            double D = dict.Count;
            double M = wordDocumentCount[word].Count;

            if (M != 0)
            {
                return D/M;
            }

            return 0;
        }
    }
}
