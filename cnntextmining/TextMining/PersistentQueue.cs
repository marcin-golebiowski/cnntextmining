using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TextMining
{
    public class PersistentQueue
    {
        private readonly SqlConnection conn;
        private readonly Queue<Uri> items = new Queue<Uri>();

        public PersistentQueue(SqlConnection conn)
        {
            this.conn = conn;

            Load();
        }

        private void Load()
        {

            using (var command
                = new SqlCommand("SELECT URL FROM dbo.[Queue]", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            items.Enqueue(new Uri(reader["URL"].ToString()));
                        }
                    }
                }
            }
        }


        public int Count
        {
            get
            {
                lock (items)
                {
                    return items.Count;
                }
            }
        }

        public void Enqueue(Uri url)
        {
            lock (items)
            {
                items.Enqueue(url);
            }
        }

        public Uri Dequeue()
        {
            lock (items)
            {
                return items.Dequeue();
            }
        }


        public void Save()
        {
            SqlTransaction tran =  conn.BeginTransaction();
            lock (items)
            {
                try
                {
                    using (var command1
                        = new SqlCommand("DELETE FROM dbo.[Queue]", conn))
                    {
                        command1.ExecuteNonQuery();
                    }


                    using (var command2
                        = new SqlCommand("INSERT INTO dbo.[Queue](URL, Created) VALUES(@url, @date)", conn))
                    {
                        command2.Parameters.Add("@url", SqlDbType.VarChar);
                        command2.Parameters.Add("@date", SqlDbType.DateTime);


                        foreach (var uri in items)
                        {
                            command2.Parameters["@url"].Value = uri.OriginalString;
                            command2.Parameters["@date"].Value = DateTime.Now.ToString();
                            command2.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
        }
    }
}
