using System.Data.SqlClient;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
       @"Data Source=IBM-PC\SQLEXPRESS;Initial Catalog=TextMining;Integrated Security=True";

        static void Main()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string uri1 =
                  "http://edition.cnn.com/2008/WORLD/europe/05/29/airbus.trading.ap/index.html";
                const string uri2 =
                  "http://topics.edition.cnn.com/topics/business";
                const string uri3 = 
                  "http://edition.cnn.com/2006/TRAVEL/06/15/A380.update/index.html";

                SaveAction action = new SaveAction(conn);

                Crawler.run(action, 10000, new string[] { uri1, uri2, uri3 }, 0, 20);

            }
        }
    }
}
