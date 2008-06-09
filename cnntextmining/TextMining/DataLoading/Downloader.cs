using System.IO;
using System.Net;

namespace TextMining.DataLoading
{
    class Downloader
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
    }
}