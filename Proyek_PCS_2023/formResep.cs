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
    public partial class formResep : Form
    {
        DataTable dataResep = new DataTable();
        DataTable dataBahan = new DataTable();
        int id, id_fnb, bahan, stock;

        public formResep()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formResep_Load(object sender, EventArgs e)
        {
            dataResep.Columns.Add("ID Resep");
            dataResep.Columns.Add("Resep Makanan");
            dataResep.Columns.Add("Bahan Makanan");
            dataResep.Columns.Add("Qty Bahan");

            dataResep = DB.query("SELECT R.ID_RESEP, F.NAMA_FNB, B.NAMA_BAHAN, R.STOK FROM RESEP R JOIN BAHAN B ON B.ID_BAHAN = R.ID_BAHAN JOIN FNB F ON F.ID_FNB = R.ID_FNB");

            bindDataSet();
            loadCombo();
        }

        private void bindDataSet()
        {
            dataResep = DB.query("SELECT R.ID_RESEP, F.NAMA_FNB, B.NAMA_BAHAN, R.STOK FROM RESEP R JOIN BAHAN B ON B.ID_BAHAN = R.ID_BAHAN JOIN FNB F ON F.ID_FNB = R.ID_FNB");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataResep;
            dataGridView1.Columns[0].Visible = false;
        }

        private void loadCombo()
        {
            MySqlDataAdapter daBahan;
            dataBahan = new DataTable("bahan");
            daBahan = new MySqlDataAdapter("Select id_bahan, nama_bahan from bahan", DB.conn);
            daBahan.Fill(dataBahan);
            comboBahan.DataSource = dataBahan;
            comboBahan.DisplayMember = dataBahan.Columns[1].ToString();
            comboBahan.ValueMember = dataBahan.Columns[0].ToString();
            comboBahan.SelectedIndex = -1;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = e.RowIndex;
            id = Convert.ToInt32(dataResep.Rows[idx][0].ToString());
            txtNama.Text = dataResep.Rows[idx][1].ToString();
            comboBahan.Text = dataResep.Rows[idx][2].ToString();
            numStock.Value = Convert.ToInt32(dataResep.Rows[idx][3].ToString());
        }

        private MySqlConnection connection = DB.conn;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DB.open();
            try
            {
                MySqlCommand cmdDelete = new MySqlCommand("DELETE from resep where ID_resep = @id", DB.conn);
                cmdDelete.Parameters.AddWithValue("@id", id);
                cmdDelete.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message.ToString());
            }
            DB.close();
            bindDataSet();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtNama.Text = "";
            comboBahan.SelectedIndex = -1;
            numStock.Value = 0;
        }


        int idx = -1;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string nama;

            nama = txtNama.Text.ToString();
            int id_fnb = GetFnbIdFromName(nama);

            int bahanID = comboBahan.SelectedIndex + 1;
            stock = Convert.ToInt32(numStock.Value.ToString());

            DB.open();

            MySqlCommand commandUpdate = new MySqlCommand("UPDATE RESEP SET id_fnb = @id_fnb, id_bahan = @id_bahan, stok = @stok WHERE id_resep = @id_resep", DB.conn);
            commandUpdate.Parameters.AddWithValue("@id_resep", id);
            commandUpdate.Parameters.AddWithValue("@id_fnb", id_fnb);
            commandUpdate.Parameters.AddWithValue("@id_bahan", bahanID);
            commandUpdate.Parameters.AddWithValue("@stok", stock);
            commandUpdate.ExecuteNonQuery();

            DB.close();

            MessageBox.Show("Berhasil Insert");

            bindDataSet();
        }

        public int GetFnbIdFromName(string name)
        {
            int fnbId = -1; // Default value if the name is not found

            try
            {
                DB.open();

                string query = $"SELECT ID_FNB FROM fnb WHERE NAMA_FNB = @Name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", name);

                object result = cmd.ExecuteScalar();

                if (result != null)
                    fnbId = Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                DB.close();
            }

            return fnbId;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string nama;
            int bahan;
            int stock;

            nama = txtNama.Text;

            nama = txtNama.Text.ToString();
            int id_fnb = GetFnbIdFromName(nama);

            if (comboBahan.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a bahan");
                return;
            }
            else
            {
                bahan = Convert.ToInt32(comboBahan.SelectedValue);
            }

            stock = (int)numStock.Value;

            // Check if id_fnb and id_bahan already exist in resep table
            string query = "SELECT COUNT(*) FROM resep WHERE ID_FNB = @id_fnb AND ID_BAHAN = @bahan";
            DB.open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_fnb", id_fnb);
                command.Parameters.AddWithValue("@bahan", bahan);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Bahan Sudah Ada Untuk Makanan Ini");
                    return;
                }
            }
            DB.close();

            int nextId;

            // Get the next available ID_RESEP using auto-increment
            query = "SELECT MAX(ID_RESEP) FROM resep";
            DB.open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                nextId = result != null && result != DBNull.Value ? Convert.ToInt32(result) + 1 : 1;
            }
            DB.close();

            DB.updateDB($"INSERT INTO resep (ID_RESEP, ID_FNB, ID_BAHAN, STOK) VALUES ({nextId}, {id_fnb}, {bahan}, {stock})");

            MessageBox.Show("Data inserted successfully");
            bindDataSet();
        }
    }
}
