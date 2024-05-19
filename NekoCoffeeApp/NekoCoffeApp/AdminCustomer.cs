﻿using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
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
    public partial class AdminCustomer : UserControl
    {
        public AdminCustomer()
        {
            InitializeComponent();
        }

        private static AdminCustomer _instance;
        public static AdminCustomer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AdminCustomer();
                return _instance;
            }
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "f5A5LselW6L4lKJHpNGVH6NZHGKIZilErMoUOoLC",
            BasePath = "https://neko-coffe-database-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient ctm;

        private void AdminCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                ctm = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("Kiểm tra lại mạng", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            viewData();
        }
        private void AdminAddCustomer_Click(object sender, EventArgs e)
        {
            if (
               string.IsNullOrWhiteSpace(AdminFillCustomerDateOfBirth.Text) ||
               string.IsNullOrWhiteSpace(AdminFillCustomerGender.Text) ||
               string.IsNullOrWhiteSpace(AdminFillCustomerAddress.Text) ||
               string.IsNullOrWhiteSpace(AdminFillCustomerPhoneNumber.Text) ||
               string.IsNullOrWhiteSpace(AdminFillCustomerEmail.Text) ||
               string.IsNullOrWhiteSpace(AdminFillCustomerID.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FirebaseResponse res = ctm.Get(@"Customers/" + AdminFillCustomerID.Text);
            NekoCustomer ResCustomer= res.ResultAs<NekoCustomer>();

            NekoCustomer CurCustomer = new NekoCustomer()
            {
                ID = AdminFillCustomerID.Text
            };

            if (NekoCustomer.Search(ResCustomer, CurCustomer))
            {
                NekoCustomer.ShowError_2();
                return;
            }

            NekoCustomer customer  = new NekoCustomer()
            {
                Name = AdminFillCustomerName.Text,
                ID = AdminFillCustomerID.Text,
                DateOfBirth = AdminFillCustomerDateOfBirth.Text,
                Gender = AdminFillCustomerGender.Text,
                Address = AdminFillCustomerAddress.Text,
                PhoneNumber = AdminFillCustomerPhoneNumber.Text,
                Email = AdminFillCustomerEmail.Text,
            };

            SetResponse set = ctm.Set(@"Customers/" + AdminFillCustomerID.Text, customer);


            if (set.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show($"Thêm thành công nhân viên {AdminFillCustomerID.Text}!", "Chúc mừng!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void viewData()
        {
            var data = ctm.Get(@"/Customers");
            var mList = JsonConvert.DeserializeObject<IDictionary<string, NekoCustomer>>(data.Body);
            var listNumber = mList.Values.ToList();
            AdminViewAllYourCustomers.DataSource = listNumber;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AdminFillCustomerDateOfBirth_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
