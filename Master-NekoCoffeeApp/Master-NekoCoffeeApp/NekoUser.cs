﻿using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master_NekoCoffeeApp
{
    internal class NekoUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public int Point { get; set; }
        public string Birthday { get; set; }

        public string Master { get; set; }

        private static string error1 = "Tài khoản không tồn tại!";
        private static string error2 = "Cảnh báo";
        public DateTime RegistrationDate { get; set; } // Thêm thuộc tính này

        public static void ShowError()
        {

            System.Windows.Forms.MessageBox.Show(error1, error2, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError_2()
        {
            System.Windows.Forms.MessageBox.Show("Tài khoản đã tồn tại!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError_3()
        {
            System.Windows.Forms.MessageBox.Show("Không tồn tại!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static bool IsEqual(NekoUser user1, NekoUser user2)
        {
            if (user1 == null || user2 == null) { return false; }

            if (user1.Username != user2.Username)
            {
                error1 = "Tài khoản không tồn tại!";
                return false;
            }


            else if (BCrypt.Net.BCrypt.EnhancedVerify(user2.Password, user1.Password, hashType: HashType.SHA384) == false)
            {
                error1 = "Sai tài khoản hoặc mật khẩu!";
                return false;
            }

            //else if (user2.Username != "22521385")
            //{
            //    error1 = "Không phải tài khoản Admin!";
            //    return false;

            //}

            return true;
        }

        public static bool IsExist(NekoUser user1, NekoUser user2)
        {
            if (user1 == null || user2 == null) { return false; }

            if (user1.Username != user2.Username || user1.Email != user2.Email || user1.PhoneNumber != user2.PhoneNumber)
            {
                error1 = "Tài khoản không tồn tại!";
                return false;
            }


            return true;
        }


        public static bool Search(NekoUser user1, NekoUser user2)
        {
            if (user1 == null || user2 == null) { return false; }

            if (user1.Username != user2.Username)
            {
                //System.Windows.Forms.MessageBox.Show("Tài khoản không tồn tại!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                return false;
            }

            return true;
        }
    }
}
