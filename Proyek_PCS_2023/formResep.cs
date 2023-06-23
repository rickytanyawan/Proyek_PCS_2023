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
            /*dataResep.Columns.Add("ID Resep");
            dataResep.Columns.Add("Resep Makanan");
            dataResep.Columns.Add("Bahan Makanan");
            dataResep.Columns.Add("Qty Bahan");

            DataTable resep = DB.query("SELECT R.ID_RESEP, F.NAMA_FNB, B.NAMA_BAHAN, R.STOK FROM RESEP R JOIN BAHAN B ON B.ID_BAHAN = R.ID_BAHAN JOIN FNB F ON F.ID_FNB = R.ID_FNB");
            int jumlahData = resep.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = resep.Rows[i];
                dataResep.Rows.Add(r.Field<int>("ID_RESEP"), r.Field<string>("NAMA_FNB"), r.Field<string>("NAMA_BAHAN"), r.Field<int>("STOK"));
            }
            
            dataGridView1.Columns[0].Visible = false;*/

            bindDataSet();
            loadCombo();
        }

        private void bindDataSet()
        {
            /*dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataResep;*/

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
            id = Convert.ToInt32(txtId.Text);
            nama = txtNama.Text.ToString();
            bahan = Convert.ToInt32(comboBahan.SelectedIndex.ToString());
            stock = Convert.ToInt32(numStock.Value.ToString());

            string query = "SELECT fnb.id_fnb FROM fnb WHERE fnb.nama_fnb = @nama";
            using (MySqlConnection connection = new MySqlConnection("server = localhost;" + "user id = root;" + "database = db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama", nama);
                    id_fnb= (int)command.ExecuteScalar();
                }
                connection.Close();
            }

            MySqlCommand commandUpdate = new MySqlCommand("UPDATE resep SET ID_FNB = @id_fnb, ID_BAHAN = @bahan, STOK = @stock where ID_RESEP = @id;", DB.conn);

            commandUpdate.Parameters.AddWithValue("@id", id);
            commandUpdate.Parameters.AddWithValue("@id_fnb", id_fnb);
            commandUpdate.Parameters.AddWithValue("@bahan", bahan);
            commandUpdate.Parameters.AddWithValue("@stock", stock);

            try
            {
                DB.open();
                commandUpdate.ExecuteNonQuery();
                DB.close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
            bindDataSet();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            id = Convert.ToInt32(txtId.Text.ToString());
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = e.RowIndex;
            txtId.Text = dataResep.Rows[idx][0].ToString();
            txtNama.Text = dataResep.Rows[idx][1].ToString();
            comboBahan.Text = dataResep.Rows[idx][2].ToString();
            numStock.Value = Convert.ToInt32(dataResep.Rows[idx][3].ToString());
            txtId.Enabled = false;
        }

        int idx = -1;

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string nama;
            id = Convert.ToInt32(txtId.Text);
            nama = txtNama.Text.ToString();
            bahan = Convert.ToInt32(comboBahan.SelectedIndex.ToString());
            stock = Convert.ToInt32(numStock.Value.ToString());

            string fnbQuery = "SELECT ID_FNB FROM fnb WHERE NAMA_FNB = @nama";
            id_fnb = 0;

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                using (MySqlCommand command = new MySqlCommand(fnbQuery, connection))
                {
                    command.Parameters.AddWithValue("@nama", nama);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        id_fnb = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("The name doesn't exist.");
                        return;
                    }
                }
            }

            // Check if the id_fnb already exists in resep table
            string resepQuery = "SELECT COUNT(*) FROM resep WHERE ID_FNB = @id_fnb";

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                using (MySqlCommand command = new MySqlCommand(resepQuery, connection))
                {
                    command.Parameters.AddWithValue("@id_fnb", id_fnb);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Resep Sudah Ada Di Table");
                        return;
                    }
                }
            }

            // Insert into resep table
            string insertQuery = "INSERT INTO resep (ID_RESEP, ID_FNB, ID_BAHAN, STOK) VALUES (@id_resep, @id_fnb, @id_bahan, @stok)";

            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id_resep", id);
                    command.Parameters.AddWithValue("@id_fnb", id_fnb);
                    command.Parameters.AddWithValue("@id_bahan", bahan);
                    command.Parameters.AddWithValue("@stok", stock);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully.");
                        bindDataSet();
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }
            }
        }
    }
}
