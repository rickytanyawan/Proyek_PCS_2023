﻿using System;
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

        }
    }
}
