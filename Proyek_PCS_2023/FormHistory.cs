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
    public partial class FormHistory : Form
    {
        public FormHistory()
        {
            InitializeComponent();
        }
        DataTable dataHistory = new DataTable();

        private void FormHistory_Load(object sender, EventArgs e)
        {
            dataHistory.Columns.Add("NOMOR_NOTA_DTRANS");
            dataHistory.Columns.Add("NAMA_FNB");
            dataHistory.Columns.Add("QTY");
            dataHistory.Columns.Add("HARGA");
            dataHistory.Columns.Add("SUBTOTAL");
            dataHistory.Columns.Add("HARGA_BAYAR");
            dataHistory.Columns.Add("KEMBALIAN");

            dataHistory = DB.query("SELECT * FROM H_TRANS");
            updatecart();
        }

        void updatecart()
        {
            dataHistory = DB.query("SELECT * FROM H_TRANS");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataHistory;
        }
    }
}
