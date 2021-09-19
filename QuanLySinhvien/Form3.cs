using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{

    public partial class Form3 : Form
    {
        public static int isvalid = 0;
        public Form3()
        {
            InitializeComponent();
            if (QuanLySinhvien.Properties.Settings.Default.username != string.Empty)
            {
                username.Text = QuanLySinhvien.Properties.Settings.Default.username;
                password.Text = QuanLySinhvien.Properties.Settings.Default.password;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(!(username.Text == "admin" && password.Text == "admin"))
            {
                
                this.label4.Text = "Tên đăng nhập sai hoặc mật khẩu không tồn tại";
            }
            else {
                if (radioButton1.Checked)
                {
                    QuanLySinhvien.Properties.Settings.Default.username = username.Text;
                    QuanLySinhvien.Properties.Settings.Default.password = password.Text;
                    QuanLySinhvien.Properties.Settings.Default.Save();
                }
                this.Dispose();
                isvalid = 1;
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
