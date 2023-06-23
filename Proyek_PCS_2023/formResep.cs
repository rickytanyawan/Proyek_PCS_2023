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
            int id;
            string nama;
            int bahan;
            int stock;
            int id_fnb = 0;

            if (!int.TryParse(txtId.Text, out id))
            {
                MessageBox.Show("Invalid ID");
                return;
            }

            nama = txtNama.Text;

            if (comboBahan.SelectedIndex == -1)
            {
                MessageBox.Show("Tolong Memilih bahan");
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
                        MessageBox.Show("Nama FNB Tidak Ada Pada fnb table");
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
                        MessageBox.Show("Bahan Sudah Terdaftar Untuk FNB Ini");
                        return;
                    }
                }
            }

            // Check if id_resep already exists in resep table
            query = "SELECT COUNT(*) FROM resep WHERE ID_RESEP = @id";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("ID_RESEP Sudah Ada Di table");
                        return;
                    }
                }
            }

            // Insert into resep table
            query = "INSERT INTO resep (ID_RESEP, ID_FNB, ID_BAHAN, STOK) VALUES (@id, @id_fnb, @bahan, @stock)";
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@id_fnb", id_fnb);
                    command.Parameters.AddWithValue("@bahan", bahan);
                    command.Parameters.AddWithValue("@stock", stock);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data Berhasil Dimasukkan");
            bindDataSet();
        }
    }
}
