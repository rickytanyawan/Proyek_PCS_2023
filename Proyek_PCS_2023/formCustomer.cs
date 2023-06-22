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
    public partial class formCustomer : Form
    {
        public formCustomer()
        {
            InitializeComponent();
        }
        DataTable dataKeranjang = new DataTable();
        private void formCustomer_Load(object sender, EventArgs e)
        {
            dataKeranjang.Columns.Add("NAMA");
            dataKeranjang.Columns.Add("JENIS");
            dataKeranjang.Columns.Add("HARGA");
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

        private void button1_Click(object sender, EventArgs e)
        {
            string makanan = comboBox1.SelectedItem.ToString();
            int jumlah = Int32.Parse(numericUpDown1.Value.ToString());
            DB.query("SELECT HARGA FROM FNB WHERE NAMA_FNB = MAKANAN");


        }
    }
}
