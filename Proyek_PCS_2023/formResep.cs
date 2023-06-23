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
        DataTable dataResep;
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
            bindDataSet();
            loadCombo();
        }

        private void bindDataSet()
        {
            MySqlDataAdapter da;
            dataResep = new DataTable("resep");
            da = new MySqlDataAdapter("SELECT R.ID_RESEP as 'ID Resep', F.NAMA_FNB as 'Resep Makanan', B.NAMA_BAHAN as 'Bahan Makanan', R.STOK as 'Qty Bahan' FROM RESEP R JOIN BAHAN B ON B.ID_BAHAN = R.ID_BAHAN JOIN FNB F ON F.ID_FNB = R.ID_FNB", DB.conn);
            da.Fill(dataResep);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string nama;
            nama = txtNama.Text.ToString();
            string bahanText = comboBahan.Text;

            // Get the ID_BAHAN based on the selected text in comboBahan
            string query = "SELECT ID_BAHAN FROM bahan WHERE NAMA_BAHAN = @bahanText";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bahanText", bahanText);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        bahan = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Invalid bahan selected");
                        return;
                    }
                }
            }

            stock = Convert.ToInt32(numStock.Value.ToString());

            string updateQuery = "UPDATE resep SET ID_BAHAN = @bahan, STOK = @stock WHERE ID_RESEP = @id";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@bahan", bahan);
                    command.Parameters.AddWithValue("@stock", stock);
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString());
                    }
                }
            }

            bindDataSet();
        }

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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = e.RowIndex;
            id = Convert.ToInt32(dataResep.Rows[idx][0].ToString());
            txtNama.Text = dataResep.Rows[idx][1].ToString();
            comboBahan.Text = dataResep.Rows[idx][2].ToString();
            numStock.Value = Convert.ToInt32(dataResep.Rows[idx][3].ToString());
        }

        int idx = -1;

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string nama;
            int bahan;
            int stock;
            int id_fnb = 0;

            nama = txtNama.Text;

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

            // Check if the name exists in fnb table
            string query = "SELECT ID_FNB FROM fnb WHERE NAMA_FNB = @nama";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama", nama);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        id_fnb = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Name does not exist in fnb table");
                        return;
                    }
                }
            }

            // Check if id_fnb and id_bahan already exist in resep table
            query = "SELECT COUNT(*) FROM resep WHERE ID_FNB = @id_fnb AND ID_BAHAN = @bahan";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_fnb", id_fnb);
                    command.Parameters.AddWithValue("@bahan", bahan);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Bahan already exists for the given ID_FNB");
                        return;
                    }
                }
            }

            // Get the next available ID_RESEP using auto-increment
            query = "SELECT MAX(ID_RESEP) FROM resep";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    int nextId = result != null && result != DBNull.Value ? Convert.ToInt32(result) + 1 : 1;

                    // Insert into resep table
                    query = "INSERT INTO resep (ID_RESEP, ID_FNB, ID_BAHAN, STOK) VALUES (@id, @id_fnb, @bahan, @stock)";
                    using (MySqlCommand insertCommand = new MySqlCommand(query, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@id", nextId);
                        insertCommand.Parameters.AddWithValue("@id_fnb", id_fnb);
                        insertCommand.Parameters.AddWithValue("@bahan", bahan);
                        insertCommand.Parameters.AddWithValue("@stock", stock);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Data inserted successfully");
            bindDataSet();
        }
    }
}
