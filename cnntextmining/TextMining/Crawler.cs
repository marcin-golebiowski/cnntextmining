using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace TextMining
{
    public class Crawler
    {
        private readonly SqlConnection conn;

        private readonly VisitedPages history;
        private readonly PersistentQueue queue;
        private readonly IAction action;
        private DateTime saveTime = DateTime.MinValue;
        

        public Crawler(IAction action, SqlConnection conn)
        {
            this.action = action;
            this.conn = conn;
            queue = new PersistentQueue(conn);
            history = new VisitedPages(conn);
        }
        
        public void Run()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("QUEUE: " + queue.Count);
                    if (queue.Count == 0)
                    {
                        Console.WriteLine("END");
                        break;
                    }
                    

                    Uri curr = queue.Dequeue();

                    Console.WriteLine("PROCESSING: " + curr );

                    if (!history.WasVisited(curr.OriginalString))
                    {
                        if (CNNPage.IsNewsPage(curr.OriginalString))
                        {
                            action.Do(new CNNPage(curr.OriginalString));
                            history.Add(curr.OriginalString);
                        }

                        Uri[] links = GetLinks(curr.OriginalString);

                        foreach (var uri in links)
                        {
                            queue.Enqueue(uri);
                        }
                    }
                    else
                    {
                        Console.WriteLine("SKIP: Page have been already visited");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("--");

                if ((saveTime - DateTime.Now) < TimeSpan.FromSeconds(10))
                {
                    saveTime = DateTime.Now;
                    queue.Save();
                    Console.WriteLine("QUEUE SAVED");
                }
            }
        }



        private static Uri[] GetLinks(string uri)
        {
            List<Uri> links = new List<Uri>();

            if (CNNPage.isTopicPage(uri))
            {
                Regex regex = new Regex("<a href=\"http:[^\"#?]+[\"#?]");
                foreach (Match m in regex.Matches(Util.FetchPage(uri)))
                {
                    Uri tmp = new Uri(m.Value.Substring(9, m.Value.Length - 10));
                    if (!links.Contains(tmp))
                        links.Add(tmp);
                }
            }
            else
            {
                CNNPage curr;
                try
                {
                    curr = new CNNPage(uri);
                    links.AddRange(curr.allLinks);
                }
                catch (Exception) { }
            }

            var nl = new List<Uri>();

            foreach (Uri u in links)
            {
                if (CNNPage.IsNewsPage(u.OriginalString)
                    || CNNPage.isTopicPage(u.OriginalString))
                {
                    nl.Add(u);
                }
            }
            return nl.ToArray();
        }
    }

   
}
