using System.Data.SqlClient;

namespace TextMining
{
    class Program
    {
        private const string connectionString =
         @"Data Source=IBM-PC\SQLEXPRESS;Initial Catalog=TextMining;Integrated Security=True";

        static void Main()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //var crawler = new Crawler(new SaveAction(conn) , conn);
               //crawler.Run();


                var clastering = new Clastering(conn);


            }
        }
    }
}
