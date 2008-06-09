using System;
using System.Data.SqlClient;

namespace TextMining.Crawling
{
    public class VisitedPages
    {
        private readonly SqlConnection conn;
      

        public VisitedPages(SqlConnection conn)
        {
            this.conn = conn;

        }

        public int Count
        {
            get
            {
                using (var command2
                    = new SqlCommand("SELECT COUNT(*) FROM dbo.[Pages] WHERE Visited IS NOT NULL", conn))
                {
                    return Convert.ToInt32(command2.ExecuteScalar().ToString());
                }
            }
        }

        public int NewsCount
        {
            get
            {
                using (var command2
                    = new SqlCommand("SELECT COUNT(URL) FROM dbo.[News]", conn))
                {
                    return Convert.ToInt32(command2.ExecuteScalar().ToString());
                }
            }
        }
      

        public bool WasVisited(string url)
        {
            using (var command2
                = new SqlCommand("SELECT Visited FROM dbo.[Pages] WHERE URL = @url", conn))
            {
                command2.Parameters.AddWithValue("@url", url);

                return command2.ExecuteScalar() != DBNull.Value;
                
            }
        }

        public void SetVisited(Uri url)
        {
            using (var command2
                = new SqlCommand("UPDATE dbo.[Pages] SET Visited = GETDATE() WHERE URL = @url", conn))
            {
                command2.Parameters.AddWithValue("@url", url.OriginalString);

                command2.ExecuteNonQuery();
            }
        }
    }
}