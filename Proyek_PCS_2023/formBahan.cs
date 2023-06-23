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
    public partial class formBahan : Form
    {
        public formBahan()
        {
            InitializeComponent();
        }
        DataTable dataBahan = new DataTable();

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void formBahan_Load(object sender, EventArgs e)
        {
            dataBahan.Columns.Add("ID_BAHAN");
            dataBahan.Columns.Add("NAMA");
            dataBahan.Columns.Add("STOK");
            dataBahan.Columns.Add("SATUAN");

            dataBahan = DB.query("SELECT * FROM BAHAN");

            updatedatacart();
        }

        void updatedatacart()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataBahan;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strtotalbahan = DB.getScalar("SELECT COUNT(*) FROM BAHAN");

            int inttotalbahan = Int32.Parse(strtotalbahan);
            inttotalbahan++;


            string tempnamabahan = textBoxNamaBahan.Text;
            int tempstok = Convert.ToInt32(numericUpDownStock.Value);
            string tempsatuanbahan = textBoxSatuan.Text;


            MySqlCommand cmdins = new MySqlCommand($"INSERT INTO BAHAN VALUES(@idbahan,@nama,@stok,@satuan)", DB.conn);
            cmdins.Parameters.AddWithValue("@idbahan", inttotalbahan);
            cmdins.Parameters.AddWithValue("@nama", tempnamabahan);
            cmdins.Parameters.AddWithValue("@stok", tempstok);
            cmdins.Parameters.AddWithValue("@satuan", tempsatuanbahan);

            DB.open();

            try
            {
                cmdins.ExecuteNonQuery();
                updatedatacart();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

            DB.close();

            MessageBox.Show("Berhasil Insert Bahan");
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (index < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[index];
            string tempidbahan = row.Cells["ID_BAHAN"].Value.ToString();
            string tempnamabahan = row.Cells["NAMA_BAHAN"].Value.ToString();
            string tempstok = row.Cells["STOK"].Value.ToString();
            string tempsatuan = row.Cells["SATUAN"].Value.ToString();

            int intsatuan = Convert.ToInt32(tempstok);

            textBoxNamaBahan.Text = tempnamabahan;
            numericUpDownStock.Value = intsatuan;
            textBoxSatuan.Text = tempsatuan;
            textBoxID.Text = tempidbahan;
            

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string tempnama = textBoxNamaBahan.Text;
            int tempstok = Convert.ToInt32(numericUpDownStock.Value);
            string satuan = textBoxSatuan.Text;
            string idbahan = textBoxID.Text;

            DB.open();
            
            MySqlCommand commandUpdate = new MySqlCommand("UPDATE BAHAN SET NAMA_BAHAN = @nama, STOK = @stok, SATUAN = @satuan WHERE ID_BAHAN = @id", DB.conn);
            commandUpdate.Parameters.AddWithValue("@nama", tempnama);
            commandUpdate.Parameters.AddWithValue("@stok", tempstok);
            commandUpdate.Parameters.AddWithValue("@satuan", satuan);
            commandUpdate.Parameters.AddWithValue("@id", idbahan);
            commandUpdate.ExecuteNonQuery();

            DB.close();

            updatedatacart();

            MessageBox.Show("Berhasil Update Barang");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string idbahan = textBoxID.Text;

            DB.open();
            try
            {
                MySqlCommand commandDelete = new MySqlCommand("DELETE FROM BAHAN WHERE ID_BAHAN = @id", DB.conn);
                commandDelete.Parameters.AddWithValue("@id", idbahan);
                commandDelete.ExecuteNonQuery();
                MessageBox.Show("Berhasil Delete");
            }
            catch (Exception exc)
            {

                throw new Exception(exc.Message.ToString());
            }

            updatedatacart();

            DB.close();
        }
    }
}
