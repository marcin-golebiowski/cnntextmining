using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TextMining
{
    public class VisitedPages
    {
        private readonly SqlConnection conn;
        private readonly Dictionary<string, bool> pages = new Dictionary<string, bool>();

        public VisitedPages(SqlConnection conn)
        {
            this.conn = conn;

            Load();
        }

        private void Load()
        {
            using (var command
                = new SqlCommand("SELECT URL FROM dbo.[News]", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pages[reader["URL"].ToString()] = true;
                        }
                    }
                }
            }
        }


        public bool WasVisited(string url)
        {
            return pages.ContainsKey(url);
        }

        public void Add(string url)
        {
            pages[url] = true;
        }
    }
}
