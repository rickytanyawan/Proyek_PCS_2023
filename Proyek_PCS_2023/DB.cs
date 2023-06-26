using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Proyek_PCS_2023
{
    class DB
    {
        public static MySqlConnection conn = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023");
        public DB()
        {

        }
        public static void open()
        {
            if (ConnectionState.Open == conn.State)
            {
                conn.Close();
            }
            conn.Open();
        }
        public static void close()
        {
            conn.Close();
        }

        public static DataTable query(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, DB.conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
        public static void updateDB(string query)
        {
            DB.open();
            MySqlTransaction transaction = DB.conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand(query, DB.conn, transaction);
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }
        public static string getScalar(string query)
        {

            MySqlCommand cmd = new MySqlCommand(query, DB.conn);
            DB.open();
            string res = cmd.ExecuteScalar().ToString();
            DB.close();
            return res;
        }
        public static List<String> getList(string query, string rowname)
        {
            DataTable d = DB.query(query);
            List<String> data = new List<string>();
            for (int i = 0; i < d.Rows.Count; i++)
            {
                DataRow dr = d.Rows[i];
                data.Add(dr.Field<string>(rowname));
            }
            return data;
        }
    }
}
