﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class AdminMenu : UserControl
    {
        private static AdminMenu _instance;
        public static AdminMenu Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AdminMenu();
                return _instance;
            }
        }
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuPanel2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AdminTitle3_Click(object sender, EventArgs e)
        {
                    }

        private void MostRecentPay2_Click(object sender, EventArgs e)
        {
                    }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void AdminTableCircle_ProgressChanged(object sender, Bunifu.UI.WinForms.BunifuCircleProgress.ProgressChangedEventArgs e)
        {

        }

        private void AdminLabel3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }

        private void AdminComePayment_Click(object sender, EventArgs e)
        {

        }

        private void AdminComeOder_Click(object sender, EventArgs e)
        {

        }
    }
}
