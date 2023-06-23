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
    public partial class formCustomer : Form
    {
        public formCustomer()
        {
            InitializeComponent();
        }
        DataTable dataKeranjang = new DataTable();
        int subtotal = 0;
        private void formCustomer_Load(object sender, EventArgs e)
        {
            dataKeranjang.Columns.Add("NAMA");
            dataKeranjang.Columns.Add("JENIS");
            dataKeranjang.Columns.Add("HARGA");
            dataKeranjang.Columns.Add("PROMO");
            dataKeranjang.Columns.Add("QTY");
            dataKeranjang.Columns.Add("SUBTOTAL");

            comboBox1.DataSource = DB.getList("SELECT NAMA_FNB FROM FNB WHERE JENIS_FNB = 'MAKANAN'", "NAMA_FNB");
            comboBox2.DataSource = DB.getList("SELECT NAMA_FNB FROM FNB WHERE JENIS_FNB = 'MINUMAN'", "NAMA_FNB");

            bindDataSet();
        }

        private void bindDataSet()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataKeranjang;
        }
        private void dataChange()
        {
            int total = 0;
            int potongan = 0;
            for (int i = 0; i < dataKeranjang.Rows.Count; i++)
            {
                DataRow r = dataKeranjang.Rows[i];
                int harga = Int32.Parse(r.Field<string>("HARGA"));
                int jumlah = Int32.Parse(r.Field<string>("QTY"));
                harga = harga * jumlah;
                total = total + harga;
                harga = Int32.Parse(r.Field<string>("PROMO"));
                potongan = potongan + harga;
            }
            textBox1.Text = "Rp. "+total;
            textBox2.Text = "Rp. " + potongan;
            
            subtotal = total-potongan;

            textBox3.Text = "Rp. " + subtotal;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string makanan = comboBox1.SelectedItem.ToString();
            int jumlah = Int32.Parse(numericUpDown1.Value.ToString());
            DataTable harga = DB.query($"SELECT HARGA, PROMO FROM FNB WHERE NAMA_FNB = '{makanan}'");
            DataRow r = harga.Rows[0];
            string jenis = "Makanan";
            int subtotal = jumlah * r.Field<int>("HARGA");
            bool ada = false;
            int getIndex=0;
            for (int i = 0; i < dataKeranjang.Rows.Count; i++)
            {
                DataRow r2 = dataKeranjang.Rows[i];
                if (r2.Field<string>("NAMA")==makanan)
                {
                    ada = true;
                    getIndex = i;
                }
            }

            if (!ada)
            {
                dataKeranjang.Rows.Add(makanan, jenis, r.Field<int>("HARGA"), r.Field<int>("PROMO"), jumlah, subtotal);
            }
            else
            {
                DataRow r2 = dataKeranjang.Rows[getIndex];
                int newQty = Int32.Parse(r2.Field<string>("QTY")) + jumlah;
                r2.BeginEdit();
                r2["QTY"] = newQty;
                r2.EndEdit();
            }
            bindDataSet();
            dataChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string minuman = comboBox2.SelectedItem.ToString();
            int jumlah = Int32.Parse(numericUpDown1.Value.ToString());
            DataTable harga = DB.query($"SELECT HARGA, PROMO FROM FNB WHERE NAMA_FNB = '{minuman}'");
            DataRow r = harga.Rows[0];
            string jenis = "Minuman";
            int subtotal = jumlah * r.Field<int>("HARGA");
            bool ada = false;
            int getIndex = 0;
            for (int i = 0; i < dataKeranjang.Rows.Count; i++)
            {
                DataRow r2 = dataKeranjang.Rows[i];
                if (r2.Field<string>("NAMA") == minuman)
                {
                    ada = true;
                    getIndex = i;
                }
            }

            if (!ada)
            {
                dataKeranjang.Rows.Add(minuman, jenis, r.Field<int>("HARGA"), r.Field<int>("PROMO"), jumlah, subtotal);
            }
            else
            {
                DataRow r2 = dataKeranjang.Rows[getIndex];
                int newQty = Int32.Parse(r2.Field<string>("QTY")) + jumlah;
                r2.BeginEdit();
                r2["QTY"] = newQty;
                r2.EndEdit();
            }
            bindDataSet();
            dataChange();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataKeranjang.Clear();
            bindDataSet();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataKeranjang.Rows[index].Delete();
            bindDataSet();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int money = Int32.Parse(textBox4.Text);
            if (money >= subtotal)
            {
                int cal = money - subtotal;
                textBox5.Text = "Rp. "+cal;
                

                string nomorNotaDTrans;

                // Get the last value of nomor_nota_dtrans from the d_trans table
                string query = "SELECT MAX(CAST(nomor_nota_dtrans AS UNSIGNED)) FROM d_trans";
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        int lastNomorNotaDTrans;
                        if (result != null && result != DBNull.Value)
                        {
                            lastNomorNotaDTrans = Convert.ToInt32(result);
                        }
                        else
                        {
                            lastNomorNotaDTrans = 0;
                        }
                        nomorNotaDTrans = (lastNomorNotaDTrans + 1).ToString();
                    }
                }

                // Insert data from dataKeranjang DataTable to d_trans table
                string insertQuery = "INSERT INTO d_trans (nomor_nota_dtrans, nama_fnb, qty, harga, subtotal) VALUES (@nomorNotaDTrans, @namaFNB, @qty, @harga, @subtotal)";
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=db_proyek_pcs_2023"))
                {
                    connection.Open();
                    foreach (DataRow row in dataKeranjang.Rows)
                    {
                        string namaFNB = row[0].ToString();
                        int qty = Convert.ToInt32(row[5]);
                        int harga = Convert.ToInt32(row[2]);

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@nomorNotaDTrans", nomorNotaDTrans);
                            command.Parameters.AddWithValue("@namaFNB", namaFNB);
                            command.Parameters.AddWithValue("@qty", qty);
                            command.Parameters.AddWithValue("@harga", harga);
                            command.Parameters.AddWithValue("@subtotal", subtotal);

                            command.ExecuteNonQuery();
                        }
                    }
                }

                button5_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Uang tidak cukup");
            }
        }
    }
}
