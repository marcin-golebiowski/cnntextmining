using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextMining
{
    class Crawler
    {
        public static Dictionary<string, bool> visititedPages =
            new Dictionary<string, bool>();

        public static void run(IAction action, int num, string[] beginUri, int delay, int max)
        {
            Queue<Uri> toProcess = new Queue<Uri>();

            foreach (string url in beginUri)
            {
                toProcess.Enqueue(new Uri(url));
            }

            for (int i = 0; i < num; i++)
            {
                try
                {
                    if (toProcess.Count == 0)
                    {
                        break;
                    }

                    Uri curr = toProcess.Dequeue();

                    if (!visititedPages.ContainsKey(curr.OriginalString))
                    {
                        if (CNNPage.IsNewsPage(curr.OriginalString))
                        {
                            action.Do(new CNNPage(curr.OriginalString));
                            visititedPages[curr.OriginalString] = true;
                        }
                        else
                        {
                            Console.WriteLine("Not NEWS");
                        }

                        Uri[] links = getLinks(curr.OriginalString);

                        for (int j = 0; (j < max) && (j < links.Length); j++)
                        {
                            toProcess.Enqueue(links[j]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Skip: have been already visited");
                    }

                  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static Uri[] getLinks(string uri)
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

            List<Uri> nl = new List<Uri>();

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
