using System.Net;
using System.IO;

namespace TextMining
{
    class Util
    {
        public static string FetchPage(string uri)
        {
            WebRequest request = WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();
            return responseFromServer;
        }


        public static string DoPorterStemming(string word)
        {
            Stemmer s = new Stemmer();
            s.add(word.ToCharArray(), word.Length);
            s.stem();
            return s.ToString();
        }

    }
}
