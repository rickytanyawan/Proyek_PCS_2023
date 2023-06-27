﻿using System;
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
    public partial class menuFNB : Form
    {
        DataTable dataMenu = new DataTable();
        int code;

        public menuFNB(int param)
        {
            InitializeComponent();
            code = param;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void menuFNB_Load(object sender, EventArgs e)
        {
            dataMenu.Columns.Add("ID Menu");
            dataMenu.Columns.Add("Nama Menu");
            dataMenu.Columns.Add("Jenis");
            dataMenu.Columns.Add("Harga");
            dataMenu.Columns.Add("Promo");
            dataMenu.Columns.Add("Paket");

            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

            if(code == -1)
            {
                label1.Visible = true;
                textBox1.Visible = true;
                label1.Enabled = true;
                textBox1.Enabled = true;
                label2.Visible = true;
                textBox2.Visible = true;
                label2.Enabled = true;
                textBox2.Enabled = true;
                label3.Visible = true;
                textBox3.Visible = true;
                label3.Enabled = true;
                textBox3.Enabled = true;
                label4.Visible = true;
                comboBox1.Visible = true;
                label4.Enabled = true;
                comboBox1.Enabled = true;
                label5.Visible = true;
                numericUpDown1.Visible = true;
                label5.Enabled = true;
                numericUpDown1.Enabled = true;
                button8.Visible = true;
                button8.Enabled = true;
            }
        }

        private void bindDataSet()
        {
            dataMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataMenu;
            dataGridView1.Columns[0].Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB WHERE PROMO > 0");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB WHERE AVAILABLE > 0");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB WHERE PAKET > 0");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int promo = Convert.ToInt32(textBox1.Text);

            promo = Int32.Parse(textBox1.Text);
            DB.updateDB($"UPDATE FNB SET PROMO = {promo} WHERE ID_FNB = {id}");
            

            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            label1.Enabled = false;
            textBox1.Enabled = false;
            label2.Visible = false;
            textBox2.Visible = false;
            label2.Enabled = false;
            textBox2.Enabled = false;
            
        }

        int id;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            label1.Visible = true;
            textBox1.Visible = true;
            label1.Enabled = true;
            textBox1.Enabled = true;
            label2.Visible = true;
            textBox2.Visible = true;
            label2.Enabled = true;
            textBox2.Enabled = true;
            label3.Visible = true;
            textBox3.Visible = true;
            label3.Enabled = true;
            textBox3.Enabled = true;
            label4.Visible = true;
            comboBox1.Visible = true;
            label4.Enabled = true;
            comboBox1.Enabled = true;
            label5.Visible = true;
            numericUpDown1.Visible = true;
            label5.Enabled = true;
            numericUpDown1.Enabled = true;

            id = Convert.ToInt32(dataMenu.Rows[idx][0].ToString());
            textBox1.Text = dataMenu.Rows[idx][4].ToString();
            textBox1.Text = dataMenu.Rows[idx][5].ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int paket = Convert.ToInt32(textBox2.Text);
            string updateQuery = "UPDATE fnb SET PAKET = @paket WHERE ID_FNB = @id";
            using (MySqlConnection connection = new MySqlConnection("server=172.29.233.212;user id=root;database=db_proyek_pcs_2023"))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@paket", paket);
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

            paket = Int32.Parse(textBox2.Text);
            DB.updateDB($"UPDATE fnb SET PAKET = {paket} WHERE ID_FNB = {id}");

            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO, PAKET FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"), r.Field<int>("PAKET"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            label1.Enabled = false;
            textBox1.Enabled = false;
            label2.Visible = false;
            textBox2.Visible = false;
            label2.Enabled = false;
            textBox2.Enabled = false;
        }

        private MySqlConnection connection = DB.conn;

        private void button8_Click(object sender, EventArgs e)
        {
            string nama = textBox3.Text.ToUpper() ;
            string jenis = comboBox1.SelectedItem.ToString().ToUpper();
            int harga = Convert.ToInt32(numericUpDown1.Value.ToString());
            int promo = Convert.ToInt32(textBox1.Text.ToString());
            int paket = Convert.ToInt32(textBox2.Text.ToString());

            int newID = -1;

            string query = "SELECT MAX(ID_FNB) FROM FNB";
            DB.open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                newID = result != null && result != DBNull.Value ? Convert.ToInt32(result) + 1 : 1;
            }
            DB.close();

            int id_resep = newID;

            int status = 1;



            DB.updateDB($"INSERT INTO fnb (ID_FNB, NAMA_FNB, JENIS_FNB, ID_RESEP, HARGA, PROMO, PAKET, AVAILABLE) VALUES ({newID}, '{nama}', '{jenis}', {id_resep}, {harga}, {promo}, {paket}, {status})");

            MessageBox.Show("Data inserted successfully");
            bindDataSet();
        }
    }
}
