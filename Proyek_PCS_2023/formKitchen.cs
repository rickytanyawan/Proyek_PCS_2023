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
    public partial class formKitchen : Form
    {
        DataTable dataKitchen = new DataTable();
        public formKitchen()
        {
            InitializeComponent();
        }

        private void formKitchen_Load(object sender, EventArgs e)
        {
            dataKitchen.Columns.Add("Nomor Nota");
            dataKitchen.Columns.Add("Makanan");
            dataKitchen.Columns.Add("Qty");

            DataTable detailMenu = DB.query("SELECT H.NOMOR_NOTA_HTRANS, D.NAMA_FNB, D.QTY FROM H_TRANS H JOIN D_TRANS D ON D.NOMOR_NOTA_DTRANS = H.NOMOR_NOTA_HTRANS");
            int jumlahData = detailMenu.Rows.Count;
            for (int i = 0; i < jumlahData; i++)
            {
                DataRow r = detailMenu.Rows[i];
                dataKitchen.Rows.Add(r.Field<int>("ID_FNB"), r.Field<string>("NAMA_FNB"), r.Field<string>("JENIS_FNB"), r.Field<int>("HARGA"), r.Field<int>("PROMO"));
            }
            bindDataSet();
        }
        private void bindDataSet()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataKitchen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            if (index < 0) return;
            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Yellow;
        }
    }
}
