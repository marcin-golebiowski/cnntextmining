using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TextMining.DataLoading;
using TextMining.TextTools;

namespace TextMining.Model
{
    public class CNNPage
    {
        public enum Topic { TRAVEL, SPORT, SHOWBIZ, TECH, BUSINESS, ASIA, EUROPE, US, WORLD };

        // If site type is topic, it will be null!
        // public NewsPageMetaData metaData;

        public string uri;         // site uri
        public string pureText;    // site text, pure no shitty tags

        public List<Uri> allLinks; // all important links (some can appear twice! - check)
        public List<string> words; // all words in news (not implemented)

        public CNNPage(string uri)
        {
            this.uri = uri;
            words = new List<string>();
            allLinks = new List<Uri>();

            string page = Downloader.FetchPage(uri);

            process(uri, page);
        }

        public static bool isTopicPage(string uri)
        {
            if (uri.Split(new char[] { '/' })[2].Contains("topics"))
            {
                return true;
            }
            return false;
        }

        public static bool IsNewsPage(string uri)
        {
            return uri.Contains("index.html")
                   && uri.Split(new char[] { '/' })[2].Contains("edition");
        }

        private void process(string uri_, string page)
        {
            if (IsNewsPage(uri_))
            {
                ProcessIncludedText(includedText(page));
                //WordList wl = new WordList();
                //words = wl.getWordList(pureText);
                DontMissLinks(page);
            }
            else
            {
                throw new Exception("Need news page");
            }
        }

        private static string includedText(string page)
        {
            bool include = false;

            StringBuilder sb = new StringBuilder();
            int i = 0;

            while (i < page.Length - 3)
            {
                // Beginning of a comment
                if (page[i] == '<' && page[i + 1] == '!' && page[i + 2] == '-' && page[i + 3] == '-')
                {
                    string toCheck = page.Substring(i + 4, 22);

                    if (toCheck == "startclickprintinclude")
                    {
                        include = true;
                        i += 29;
                        continue;
                    }
                    if (toCheck == "startclickprintexclude")
                    {
                        include = false;
                        i += 29;
                        continue;
                    }

                    toCheck = page.Substring(i + 4, 20);

                    if (toCheck == "endclickprintinclude")
                    {
                        include = false;
                        i += 27;
                        continue;
                    }

                    if (toCheck == "endclickprintexclude")
                    {
                        include = true;
                        i += 27;
                        continue;
                    }

                    // dont include <!--date>
                    // <!-- date --><!-- /date -->

                    toCheck = page.Substring(i + 5, 4);
                    if (toCheck == "date")
                    {
                        include = false;
                        i += 13;
                        continue;
                    }

                    toCheck = page.Substring(i + 5, 5);
                    if (toCheck == "/date")
                    {
                        include = true;
                        i += 14;
                        continue;
                    }

                }
                if (include)
                {
                    sb.Append(page[i]);
                }
                i++;

            }
            if (include)
            {
                sb.Append(page[i++]);
                sb.Append(page[i++]);
                sb.Append(page[i++]);
            }
            return sb.ToString();
        }

        private void ProcessIncludedText(string page)
        {
            bool include = true;
            bool specialDontInclude = false;
            int i = 0;

            StringBuilder text = new StringBuilder();

            while (i < page.Length)
            {
                if (page[i] == '<')
                {
                    include = false;
                    string toCheck;
                    i++;

                    if (i + 25 < page.Length)
                    {
                        toCheck = page.Substring(i, 24);
                        if (toCheck == "div id=\"cnnGlobalFooter\"")
                            break;
                    }
                    if (i + 34 < page.Length)
                    {
                        toCheck = page.Substring(i, 34);
                        if (toCheck == "div class=\"cnnSCRightColBorderBox\"")
                            break;
                    }
                    if (i + 24 < page.Length)
                    {
                        toCheck = page.Substring(i, 24);
                        if (toCheck == "p class=\"cnnAttribution\"")
                            break;
                    }
                    if (i + 19 < page.Length)
                    {
                        toCheck = page.Substring(i, 19);
                        if (toCheck == "p class=\"cnnTopics\"")
                            break;
                    }
                    if (i + 6 < page.Length)
                    {
                        toCheck = page.Substring(i, 6);
                        if (toCheck == "script")
                            specialDontInclude = true;
                    }

                    continue;
                }
                if (page[i] == '>')
                {
                    if (specialDontInclude)
                    {
                        specialDontInclude = false;
                        i++;
                    }
                    else
                    {
                        include = true;
                        i++;
                    }
                    continue;
                }
                if (include)
                    text.Append(page[i]);
                i++;
            }


            Regex linkReg = new Regex("<a href=\"http:[^\"]+\"");

            foreach (Match m in linkReg.Matches(page))
            {
                Uri tmp = new Uri(m.Value.Substring(9, m.Value.Length - 10));
                if (!allLinks.Contains(tmp))
                {
                    allLinks.Add(tmp);
                }
            }

            pureText = text.ToString().Trim();
        }

        /*
        private void GetWords()
        {
            string[] wordstemp = pureText.Split(' ');
            for (int i = 0; i < wordstemp.Length; i++)
            {
                // normalization 
                string word = wordstemp[i].ToLower().Trim();

                // steaming
                word = Stemmer.DoPorterStemming(word);

                if (WordQuilifier.WordIsOK(word))
                {
                    words.Add(word);
                }
            }
        }
        */
 
        private void DontMissLinks(string page)
        {
            Regex dontMissReg =
                new Regex("<div class=\"cnnStoryElementBox\".*", RegexOptions.Singleline);
            Match md = dontMissReg.Match(page);
            string dontmiss = md.Value;

            string toProcess = dontmiss;

            if (dontmiss.IndexOf("<!--endclickprintexclude-->") > 0)
                toProcess = dontmiss.Substring(0, dontmiss.IndexOf("<!--endclickprintexclude-->"));

            Regex linkReg = new Regex("href=\"http:[^\"]+\"");

            foreach (Match m in linkReg.Matches(toProcess))
            {
                Uri tmp = new Uri(m.Value.Substring(6, m.Value.Length - 7));
                if (!allLinks.Contains(tmp))
                    allLinks.Add(tmp);
            }

            Regex linkReg2 = new Regex("<a href=\"/[^\"]+\"");

            foreach (Match m in linkReg2.Matches(toProcess))
            {
                Uri tmp = new Uri("http://edition.cnn.com" + m.Value.Substring(9, m.Value.Length - 10));
                if (!allLinks.Contains(tmp))
                {
                    allLinks.Add(tmp);
                }
            }
        }
    }
}