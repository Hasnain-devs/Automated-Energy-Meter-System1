using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace electricity
{
    public class common
    {
        string connection = ConfigurationManager.AppSettings["data"] ?? ConfigurationManager.AppSettings["Data"] ?? "";

        private MySqlConnection OpenConnection()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new InvalidOperationException("Missing connection string in appSettings key 'Data'.");
            }
            MySqlConnection con = new MySqlConnection(connection);
            con.Open();
            return con;
        }

        public int transq(string query)
        {
            using (MySqlConnection con = OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public double aggregate(string query)
        {
            using (MySqlConnection con = OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                object scalar = cmd.ExecuteScalar();
                if (scalar == null || scalar == DBNull.Value)
                {
                    return 0;
                }

                double res;
                if (!double.TryParse(scalar.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
        }

        public DataTable nontransq(string query)
        {
            DataTable tab = new DataTable();
            using (MySqlConnection con = OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {
                sda.Fill(tab);
                return tab;
            }
        }
    }
}
