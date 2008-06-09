using System;
using System.Data;
using System.Data.SqlClient;

namespace TextMining
{
    public class PersistentQueue
    {
        private readonly SqlConnection conn;
        private int max = 4000;
        private int shrink = 2000;

        public PersistentQueue(SqlConnection conn)
        {
            this.conn = conn;

        }


        public int Count
        {
            get
            {
                using (var command2 
                    = new SqlCommand("SELECT COUNT(*) FROM dbo.[Pages] WHERE Visited IS NULL", conn))
                {
                    return Convert.ToInt32(command2.ExecuteScalar().ToString());
                }
            }
        }

        public void Enqueue(Uri url)
        {
            using (var command2
                     = new SqlCommand("INSERT INTO dbo.[Pages](URL, Created) VALUES(@url, GETDATE())", conn))
            {
                command2.Parameters.AddWithValue("@url", url.OriginalString);

                command2.ExecuteNonQuery();
            }
        }

        public Uri Dequeue()
        {
            using (var command2
                     = new SqlCommand("SELECT TOP 1 URL FROM dbo.[Pages] WHERE Visited IS NULL ORDER BY Created ASC", conn))
            {
                return new Uri(command2.ExecuteScalar().ToString());
            }
        }

      
    }
}
