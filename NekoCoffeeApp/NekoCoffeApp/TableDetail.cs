﻿using FireSharp;
using FireSharp.Config;
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
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using static Google.Apis.Requests.BatchRequest;

namespace UI
{
    public partial class TableDetail : Form
    {
        DataTable mydt = new DataTable();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "f5A5LselW6L4lKJHpNGVH6NZHGKIZilErMoUOoLC",
            BasePath = "https://neko-coffe-database-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        public TableDetail() { }
        private NekoTable _table;
        public TableDetail(NekoTable table)
        {
            InitializeComponent();
            _table = table;
            LoadTableDetails();
        }
        private void LoadTableDetails()
        {

        }

        private async void TableDetail_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            mydt.Columns.Add("Tên Món");
            mydt.Columns.Add("SL");
            mydt.Columns.Add("Thành Tiền");

            dataGridView1.DataSource = mydt;
            try
            {
                var response = await client.GetAsync("/Drinks");

                // Check if response or response.Body is null
                if (response == null || response.Body == null)
                {
                    MessageBox.Show("No data retrieved from Firebase.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string jsonData = response.Body;

                // Clear existing items in the ComboBox
                comboBox1.Items.Clear();

                // Kiểm tra xem jsonData có phải là mảng JSON không
                if (jsonData.TrimStart().StartsWith("["))
                {
                    // Nếu là mảng JSON, xử lý nó như một danh sách
                    var drinksList = JsonConvert.DeserializeObject<List<NekoDrink>>(jsonData);

                    // Check if drinksList is null or empty
                    if (drinksList == null || !drinksList.Any())
                    {
                        MessageBox.Show("No drinks found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Add drink names to the ComboBox
                    foreach (var drink in drinksList)
                    {
                        if (drink != null && !string.IsNullOrEmpty(drink.Name))
                        {
                            comboBox1.Items.Add(drink.Name);
                        }
                    }
                }
                else
                {
                    // Nếu không phải mảng JSON, xử lý nó như một đối tượng JSON
                    var drinksDict = JsonConvert.DeserializeObject<Dictionary<string, NekoDrink>>(jsonData);

                    // Check if drinksDict is null or empty
                    if (drinksDict == null || !drinksDict.Any())
                    {
                        MessageBox.Show("No drinks found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Add drink names to the ComboBox
                    foreach (var drink in drinksDict.Values)
                    {
                        if (drink != null && !string.IsNullOrEmpty(drink.Name))
                        {
                            comboBox1.Items.Add(drink.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var response1 = await client.GetAsync("/Cats");
            var cats = JsonConvert.DeserializeObject<Dictionary<string, NekoCat>>(response1.Body);
            foreach (var cat in cats.Values)
            {
                comboBox2.Items.Add(cat.Name);
            }
        }

        private async void TableDetailsAdd_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có chắc chắn muốn xóa món này ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                if (comboBox1.SelectedIndex <= -1 && comboBox2.SelectedIndex <= -1)
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm");
                    return;
                }
                if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
                {
                    MessageBox.Show("Vui lòng chỉ chọn một sản phẩm");
                    return;
                }
                string selectedNumber = numericUpDown1.Value.ToString();
                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Vui lòng chọn số lượng của sản phẩm");
                    return;
                }
                string selectedItem;

                if (comboBox1.SelectedIndex >= 0)
                {
                    selectedItem = comboBox1.SelectedItem.ToString();
                }
                else
                {
                    selectedItem = comboBox2.SelectedItem.ToString();
                }
                FirebaseResponse response = await client.DeleteAsync("TableDetails/" + _table.ID + "/" + selectedItem);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Tên Món"].Value != null && row.Cells["Tên Món"].Value.ToString() == selectedItem)
                    {
                        dataGridView1.Rows.Remove(row);
                        break;
                    }
                }
                MessageBox.Show("Đã xóa thành công !!! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void bunifuButton1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có mục nào được chọn hay không
            if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.");
                return;
            }
            // Kiểm tra xem có nhiều hơn một mục được chọn hay không
            else if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
            {
                MessageBox.Show("Vui lòng chỉ chọn một sản phẩm.");
                return;
            }

            int selectedNumber = (int)numericUpDown1.Value;
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Vui lòng chọn số lượng của sản phẩm");
                return;
            }
            string selectedItem;
            int money = 0;

            // Lấy mục được chọn từ ComboBox1 hoặc ComboBox2
            if (comboBox1.SelectedIndex >= 0)
            {
                selectedItem = comboBox1.SelectedItem?.ToString(); // Use null-conditional operator
                var response = await client.GetAsync("/Drinks");
                if (response == null || response.Body == null)
                {
                    MessageBox.Show("No data retrieved from Firebase.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string jsonData = response.Body;
                if (jsonData?.TrimStart().StartsWith("[") == true) // Use null-conditional operator
                {
                    // Nếu là mảng JSON, xử lý nó như một danh sách
                    var drinksList = JsonConvert.DeserializeObject<List<NekoDrink>>(jsonData);

                    // Check if drinksList is null or empty
                    if (drinksList == null || !drinksList.Any())
                    {
                        MessageBox.Show("No drinks found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (var drink in drinksList)
                    {
                        if (drink?.Name == selectedItem && !string.IsNullOrEmpty(drink.Name)) // Use null-conditional operator
                        {
                            if (int.TryParse(drink.Price, out int price))
                            {
                                money = price * selectedNumber;
                            }
                        }
                    }
                }
            }
            else
            {
                selectedItem = comboBox2.SelectedItem.ToString();
                var response = await client.GetAsync("/Cats");
                var drinks = JsonConvert.DeserializeObject<Dictionary<string, NekoCat>>(response.Body);
                foreach (var drink in drinks.Values)
                {
                    if (drink.Name == selectedItem)
                    {
                        if (int.TryParse(drink.Price, out int price))
                        {
                            money = price * selectedNumber;
                        }
                    }
                }
            }

            // Lấy dữ liệu từ Firebase

            // Tạo dữ liệu chi tiết bàn
            var data = new NekoTableDetail
            {
                Name = selectedItem,
                SL = selectedNumber,
                Total = money
            };

            // Lưu dữ liệu chi tiết bàn vào Firebase
            SetResponse response1 = await client.SetAsync("TableDetails/" + _table.ID + "/" + selectedItem, data);
            if (response1.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Đã thêm món thành công!");
                _table.Status = "Booked";
                var tableUpdateResponse = await client.SetAsync("Tables/" + _table.ID + "/Status", _table.Status);
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm món.");
            }
            _table.Status = "Booked";

            // Reset các control
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            numericUpDown1.Value = 0;
        }

        private async void TableDetailsView_Click(object sender, EventArgs e)
        {
            try
            {
                mydt.Rows.Clear();
                FirebaseResponse resp2 = await client.GetAsync("TableDetails/" + _table.ID);

                if (resp2.Body == "null" || resp2 == null)
                {
                    MessageBox.Show("Bàn này chưa được sử dụng");
                    return;
                }
                string jsonData = resp2.Body;
                if (jsonData.TrimStart().StartsWith("["))
                {
                    var tableDetailsList = JsonConvert.DeserializeObject<List<NekoTableDetail>>(jsonData);
                    foreach (var detail in tableDetailsList)
                    {
                        if (detail != null && !string.IsNullOrEmpty(detail.Name))
                        {
                            DataRow row = mydt.NewRow();
                            row["Tên Món"] = detail.Name;
                            row["SL"] = detail.SL;
                            row["Thành Tiền"] = detail.Total;
                            mydt.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    mydt.Rows.Clear();
                    FirebaseResponse resp3 = await client.GetAsync("TableDetails/" + _table.ID);

                    if (resp3.Body == "null")
                    {
                        MessageBox.Show("Bàn này chưa được sử dụng");
                        return;
                    }

                    var tableDetails = JsonConvert.DeserializeObject<Dictionary<string, NekoTableDetail>>(resp2.Body);

                    foreach (var detail in tableDetails.Values)
                    {
                        DataRow row = mydt.NewRow();
                        row["Tên Món"] = detail.Name;
                        row["SL"] = detail.SL;
                        row["Thành Tiền"] = detail.Total;
                        mydt.Rows.Add(row);
                    }
                }

                dataGridView1.DataSource = mydt; // Bind the updated DataTable to DataGridView

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving table details: " + ex.Message);
            }
        }

        private async void TableDetailsPayment_Click(object sender, EventArgs e)
        {
            try
            {
                FirebaseResponse resp2 = await client.GetAsync("TableDetails/" + _table.ID);
                if (resp2.Body == "null")
                {
                    MessageBox.Show("Bàn này chưa được sử dụng");
                    return;
                }

                var tableDetails = JsonConvert.DeserializeObject<Dictionary<string, NekoTableDetail>>(resp2.Body);

                FirebaseResponse resp = await client.GetAsync("Counter/node");
                CountClass get = resp.ResultAs<CountClass>();
                int currentBillCount = Convert.ToInt32(get.count);

                int total = tableDetails.Values.Sum(detail => detail.Total);

                string paymentTime = DateTime.Now.ToString("dd/MM/yy - HH:mm"); // Định dạng dd/MM/yy - HH:mm

                var bill = new Bills
                {
                    billId = (currentBillCount + 1).ToString(),
                    tableId = _table.ID,
                    Total = total,
                    Details = tableDetails.Values.ToList(),
                    PaymentTime = paymentTime // Lưu thời gian thanh toán
                };

                using (var form = new BillDetailForm(bill))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult confirmResult = MessageBox.Show("Bạn có muốn thanh toán bàn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmResult == DialogResult.Yes)
                        {
                            SetResponse response = await client.SetAsync("Bills/" + bill.billId, bill);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var updateCount = new CountClass { count = (currentBillCount + 1).ToString() };
                                await client.SetAsync("Counter/node", updateCount);

                                await client.DeleteAsync("TableDetails/" + _table.ID);
                                mydt.Rows.Clear();
                                _table.Status = "Available";
                                var tableUpdateResponse = await client.SetAsync("Tables/" + _table.ID + "/Status", _table.Status);
                                MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Có lỗi xảy ra khi lưu hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }
        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            numericUpDown1.Value = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.Rows[e.RowIndex];
            foreach (string s in comboBox1.Items)
            {
                if (s == Convert.ToString(row.Cells["Tên Món"].Value))
                {
                    comboBox1.SelectedItem = Convert.ToString(row.Cells["Tên Món"].Value);
                }
            }
            foreach (string s in comboBox2.Items)
            {
                if (s == Convert.ToString(row.Cells["Tên Món"].Value))
                {
                    comboBox2.SelectedItem = Convert.ToString(row.Cells["Tên Món"].Value);
                }
            }
            numericUpDown1.Value = Convert.ToInt64(row.Cells["SL"].Value);
        }
    }
}

