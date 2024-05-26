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
    public partial class AdminOrder : UserControl
    {


        private static AdminOrder _instance;
        public static AdminOrder Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AdminOrder();
                return _instance;
            }
        }

        public AdminOrder()
        {
            InitializeComponent();

        }

        private void AdminOrderBtn_Click(object sender, EventArgs e)
        {
            edit_Order edit_Order = new edit_Order();
            edit_Order.Show();
        }

        


        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        

        

        

        private void AdminOrder_Load(object sender, EventArgs e)
        {

        }

        private void AdminOrderTable1_Click(object sender, EventArgs e)
        {
            TableDetail tableDetail = new TableDetail();
            tableDetail.Show();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            edit_Table edit_Table = new edit_Table();
            edit_Table.Show();
        }
    }
}
