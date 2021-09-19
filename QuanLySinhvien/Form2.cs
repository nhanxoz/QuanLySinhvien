using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhvien
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = richTextBox1.Text;
            string query = $"INSERT INTO myimages values ('{msg}', (SELECT * FROM OPENROWSET(BULK N'C:\\Users\\ADMIN\\source\\repos\\QuanLySinhvien\\QuanLySinhvien\\bin\\Debug\\test.jpg', SINGLE_BLOB) as T1))";
            Form1.GetData(query);
            this.Dispose();
        }
    }
}
