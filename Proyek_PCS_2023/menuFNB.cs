using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyek_PCS_2023
{
    public partial class menuFNB : Form
    {
        DataTable dataMenu = new DataTable();

        public menuFNB()
        {
            InitializeComponent();
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

            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
        }

        private void bindDataSet()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataMenu;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO FROM FNB WHERE PROMO > 0");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO FROM FNB");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO FROM FNB WHERE AVAILABLE > 0");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataMenu.Clear();
            DataTable detailMenu = DB.query("SELECT ID_FNB, NAMA_FNB, JENIS_FNB, HARGA, PROMO FROM FNB WHERE PAKET");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataMenu.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

        }
    }
}
