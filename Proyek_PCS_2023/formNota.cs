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
    public partial class formNota : Form
    {
        string nota;
        CrystalReport1 report;
        public formNota(string nomorNota)
        {
            InitializeComponent();
            nota = nomorNota;
        }

        private void formNota_Load(object sender, EventArgs e)
        {
            report = new CrystalReport1();
            report.SetParameterValue("paramNota", nota);

            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report;
        }
    }
}
