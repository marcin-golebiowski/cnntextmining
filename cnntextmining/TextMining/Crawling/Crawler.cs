using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TextMining.Crawling;
using TextMining.DataProcessing;
using TextMining.Model;

namespace TextMining.Crawling
{
    public class Crawler
    {
        private readonly SqlConnection conn;
        private readonly VisitedPages history;
        private readonly PersistentQueue queue;
        private readonly IAction newsAction;       
        private readonly Random random = new Random();


        public Crawler(IAction newsAction,  SqlConnection conn)
        {
            this.newsAction = newsAction;
            
            this.conn = conn;

            queue = new PersistentQueue(conn);
            history = new VisitedPages(conn);
        }

        public void Run2()
        {
            SqlTransaction trans = null;

            int count = 0;

            while (true)
            {
                using (var command2
                    = new SqlCommand(
                        "SELECT TOP 1 TopicURL FROM dbo.[TopicsTmp] WHERE Visited IS NULL", conn))
                {
                    object result = command2.ExecuteScalar();

                    if (result == DBNull.Value)
                    {
                        return;
                    }

                    Console.WriteLine("-PROCESSING: " + result);
                    var action = new TopicAction(conn);

                    try
                    {
                        action.Do(result.ToString());
                        count++;
                    }
                    finally
                    {
                        using (var command
                            = new SqlCommand("UPDATE dbo.[TopicsTmp] SET Visited = GETDATE()" 
                                             + " WHERE TopicURL = @url", conn))
                        {
                            command.Parameters.AddWithValue("@url", result.ToString());
                            command.ExecuteNonQuery();
                        }
                    }

                    Console.WriteLine("COUNT = " + count);
                }
            }
        }


        public void Run()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("PAGES VISITED: " + history.Count);
                    Console.WriteLine("NEWS:" + history.NewsCount);
                    Console.WriteLine("QUEUE SIZE: " + queue.Count);


                    if (queue.Count == 0)
                    {
                        Console.WriteLine("END");
                        break;
                    }

                    Uri curr = queue.Dequeue();
                    Console.WriteLine("PROCESSING: " + curr);

                    if (!history.WasVisited(curr.OriginalString))
                    {
                        try
                        {
                            //----
                            if (!CNNPage.IsNewsPage(curr.OriginalString) &&
                                !CNNPage.isTopicPage(curr.OriginalString))
                            {
                                history.SetVisited(curr);
                                Console.WriteLine("--");
                            }

                         


                            //----
                            if (CNNPage.IsNewsPage(curr.OriginalString))
                            {
                                CNNPage page = new CNNPage(curr.OriginalString);
                                newsAction.Do(page);

                                //----
                                Uri[] links = page.allLinks.ToArray();
                                AddLinksToPagesToVisit(links);
                            }

                            //----
                            if (CNNPage.isTopicPage(curr.OriginalString))
                            {
                                var action = new TopicAction(conn);
                                action.Do(curr.OriginalString);

                                //----
                                Uri[] links = GetLinks(curr.OriginalString);
                                AddLinksToPagesToVisit(links);
                            }
                        }
                        finally
                        {
                            history.SetVisited(curr);
                            Console.WriteLine("--");
                        }
                    }
                    else
                    {
                        Console.WriteLine("SKIP: Page have been already visited");
                        Console.WriteLine("--");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }



       
        private void AddLinksToPagesToVisit(Uri[] links)
        {
            if (links.Length < 30)
            {
                Console.WriteLine("Dodaje wszystkie linki: (" + links.Length + ")");
                foreach (var uri in links)
                {
                    try
                    {
                        queue.Enqueue(uri);
                    }
                    catch {}
                }
            }
            else
            {
                Console.WriteLine("Dodaje losowe 20 linków");
                var ids = new List<int>();
                for (int i = 0; i < links.Length; i++)
                {
                    ids.Add(i);
                }

                for (int j = 30; j >= 1; j--)
                {
                    int i = random.Next()%j;
                    try
                    {
                        queue.Enqueue(links[ids[i]]);
                    }
                    catch {}
                    ids.RemoveAt(i);

                }
            }
        }

        private static Uri[] GetLinks(string uri)
        {
            List<Uri> links = new List<Uri>();

            if (CNNPage.isTopicPage(uri))
            {
                Regex regex = new Regex("<a href=\"http:[^\"#?]+[\"#?]");
                string page = Downloader.FetchPage(uri);
                foreach (Match m in regex.Matches(page))
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