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
    public partial class formKitchen : Form
    {
        DataTable dataKitchen = new DataTable();
        public formKitchen()
        {
            InitializeComponent();
        }

        private void formKitchen_Load(object sender, EventArgs e)
        {
            bindDataSet();
        }
        private void bindDataSet()
        {
            MySqlDataAdapter da;
            dataKitchen = new DataTable("resep");
            da = new MySqlDataAdapter("SELECT H.NOMOR_NOTA_HTRANS as 'Nomor Nota', D.NAMA_FNB as 'Makanan', D.QTY as 'QTY', UPPER(H.STATUS) as 'Status' FROM H_TRANS H JOIN D_TRANS D ON D.NOMOR_NOTA_DTRANS = H.NOMOR_NOTA_HTRANS", DB.conn);
            da.Fill(dataKitchen);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataKitchen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateStatus("COOKING");
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["Status"].Value != null)
                {
                    string status = row.Cells["Status"].Value.ToString();

                    if (status == "PAID")
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (status == "COOKING")
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }
                    else if (status == "DONE")
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
            }
        }

        int id, idx;
        string status;

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateStatus("DONE");
        }

        private void UpdateStatus(string newStatus)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                row.Cells["Status"].Value = newStatus;
                UpdateRowBackgroundColor(row);
            }
        }

        private void UpdateRowBackgroundColor(DataGridViewRow row)
        {
            string status = row.Cells["Status"].Value.ToString();

            if (status == "PAID")
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;
            }
            else if (status == "COOKING")
            {
                row.DefaultCellStyle.BackColor = Color.Orange;
            }
            else if (status == "DONE")
            {
                row.DefaultCellStyle.BackColor = Color.Green;
            }
        }
    }
}
