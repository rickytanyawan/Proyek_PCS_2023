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
    public partial class formResep : Form
    {
        DataTable dataResep = new DataTable();
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

            DataTable resep = DB.query("SELECT R.ID_RESEP, F.NAMA_FNB, B.NAMA_BAHAN, R.STOK FROM RESEP R JOIN BAHAN B ON B.ID_BAHAN = R.ID_BAHAN JOIN FNB F ON F.ID_FNB = R.ID_FNB");
            int jumlahData = resep.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = resep.Rows[i];
                dataResep.Rows.Add(r.Field<int>("ID_RESEP"), r.Field<string>("NAMA_FNB"), r.Field<string>("NAMA_BAHAN"), r.Field<int>("STOK"));
            }
            bindDataSet();
            dataGridView1.Columns[0].Visible = false;

            comboBox1.DataSource = DB.getList("SELECT NAMA_BAHAN FROM BAHAN", "NAMA_BAHAN");
        }

        private void bindDataSet()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataResep;
        }
    }
}
