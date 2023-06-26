using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Proyek_PCS_2023
{
    public partial class formKitchen : Form
    {
        DataTable dataKitchen = new DataTable();
        public formKitchen()
        {
            InitializeComponent();
        }
        int index = 0;

        private void formKitchen_Load(object sender, EventArgs e)
        {
            bindDataSet();
        }
        private void bindDataSet()
        {
            MySqlDataAdapter da;
            dataKitchen = new DataTable("resep");
            da = new MySqlDataAdapter("SELECT H.NOMOR_NOTA_HTRANS as 'Nomor Nota', D.NAMA_FNB as 'Makanan', D.QTY as 'QTY', UPPER(H.STATUS) as 'Status' FROM H_TRANS H JOIN D_TRANS D ON D.NOMOR_NOTA_DTRANS = H.NOMOR_NOTA_HTRANS", DB.conn);
            da.Fill(dataKitchen);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataKitchen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                string tempnonota = row.Cells["Nomor Nota"].Value.ToString();


                string makanan = row.Cells["Makanan"].Value.ToString();
                int id_fnb = GetFnbId(makanan);

                if (id_fnb != -1)
                {
                    List<int> id_bahanList = GetBahanIds(id_fnb);

                    if (id_bahanList.Count > 0)
                    {
                        foreach (int id_bahan in id_bahanList)
                        {
                            ReduceBahanStock(id_bahan);
                        }

                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            index++;
                            row = dataGridView1.Rows[index];

                            if (row.Cells["Nomor Nota"].Value.ToString() == tempnonota)
                            {
                                // Update the status to "COOKING"
                                row.Cells["Status"].Value = "COOKING";
                                UpdateRowBackgroundColor(row);
                            }
                            
                        }

                        

                        // Update the status in the h_trans table
                        string nomorNotaHTrans = row.Cells["Nomor Nota"].Value.ToString();
                        UpdateHTransStatus(nomorNotaHTrans, "COOKING");
                    }
                    else
                    {
                        MessageBox.Show("No ingredients found for the selected food item.");
                    }
                }
                else
                {
                    MessageBox.Show("Food item not found in the database.");
                }
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();

                    if (status == "PAID")
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (status == "COOKING")
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }
                    else if (status == "DONE")
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                string nomorNotaHTrans = row.Cells["Nomor Nota"].Value.ToString();

                // Update the status to "DONE"
                row.Cells["Status"].Value = "DONE";
                UpdateRowBackgroundColor(row);

                // Update the status in the h_trans table
                UpdateHTransStatus(nomorNotaHTrans, "DONE");
            }
        }

        private void UpdateHTransStatus(string nomorNotaHTrans, string status)
        {
            string updateQuery = "UPDATE h_trans SET STATUS = @status WHERE NOMOR_NOTA_HTRANS = @nomorNotaHTrans";

            using (MySqlConnection connection = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@nomorNotaHTrans", nomorNotaHTrans);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateStatus(string newStatus)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                row.Cells["Status"].Value = newStatus;
                UpdateRowBackgroundColor(row);
            }
        }

        private void UpdateRowBackgroundColor(DataGridViewRow row)
        {
            string status = row.Cells["Status"].Value.ToString();

            if (status == "PAID")
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;
            }
            else if (status == "COOKING")
            {
                row.DefaultCellStyle.BackColor = Color.Orange;
            }
            else if (status == "DONE")
            {
                row.DefaultCellStyle.BackColor = Color.Green;
            }
        }

        private int GetFnbId(string nama_fnb)
        {
            int id_fnb = -1;
            string query = "SELECT id_fnb FROM fnb WHERE nama_fnb = @nama_fnb";

            using (MySqlConnection connection = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama_fnb", nama_fnb);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        id_fnb = Convert.ToInt32(result);
                    }
                }
            }

            return id_fnb;
        }

        private List<int> GetBahanIds(int id_fnb)
        {
            List<int> id_bahanList = new List<int>();
            string query = "SELECT id_bahan FROM resep WHERE id_fnb = @id_fnb";

            using (MySqlConnection connection = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_fnb", id_fnb);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id_bahan = reader.GetInt32("id_bahan");
                            id_bahanList.Add(id_bahan);
                        }
                    }
                }
            }

            return id_bahanList;
        }

        private void ReduceBahanStock(int id_bahan)
        {
            string updateQuery = "UPDATE bahan SET stok = stok - 1 WHERE id_bahan = @id_bahan";

            using (MySqlConnection connection = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id_bahan", id_bahan);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
