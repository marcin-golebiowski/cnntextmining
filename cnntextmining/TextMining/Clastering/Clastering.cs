using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TextMining
{
    class Clastering
    {
        private readonly SqlConnection conn;

        public Clastering(SqlConnection conn)
        {
            this.conn = conn;
        }

        public void Run()
        {
            DateTime start = DateTime.Now;

            List<CNNPage> pages = GetAllNews();

        }

        public List<CNNPage> GetAllNews()
        {
            List<CNNPage> 

            using (var command
                    = new SqlCommand("SELECT * FROM dbo.[News]", conn))
            {
               
            }
        }



    }
}
