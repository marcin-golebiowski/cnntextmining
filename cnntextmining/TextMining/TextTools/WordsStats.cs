using System;
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

        public void Compute()
        {
            Console.WriteLine("Start Computing word freq");
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

            Console.WriteLine(wordCount.Count);

            Console.WriteLine("End Computing word freq");
        }    


        public double GetDocumentTermFrequency(string word, string url)
        {
            // liczba wystapien w dokumencie / liczbe wszystkich slow w dokumencie

            double wordFreq = wordDocumentCount[word][url];
            double words = dict[url].words.Count;

            if (words != 0)
            {
                return wordFreq / words;
                
            }
            else
            {
                return 0;
            }

        }

        public double GetInvertedDocumentTermFreqency(string word)
        {
            double D = dict.Count;
            double M = wordDocumentCount[word].Count;

            if (M != 0)
            {
                return D/M;
            }
            else
            {
                return 0;
            }
        }
    }
}
