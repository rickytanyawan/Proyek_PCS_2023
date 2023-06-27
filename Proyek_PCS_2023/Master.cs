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
    public partial class Master : Form
    {
        public Master()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            formBahan b = new formBahan();
            b.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formResep r = new formResep();
            r.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int code = 0;
            menuFNB f = new menuFNB(code);
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            formCustomer f = new formCustomer();
            f.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            formKitchen f = new formKitchen();
            f.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            /*FormHistory f = new FormHistory();
            f.ShowDialog();*/
        }
    }
}
