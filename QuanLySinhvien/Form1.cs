using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace QuanLySinhvien
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = this.GetData("SELECT TenKhoa, makhoa FROM khoa");
            this.PopulateTreeView(dt, null);
        }

        private void PopulateTreeView(DataTable dtParent, TreeNode treeNode)
        {
            foreach (DataRow row in dtParent.Rows)
            {
                TreeNode child = new TreeNode
                { 
                    Text = row[0].ToString(),
                    Tag = row[1]
                };
                
                if (treeNode == null)
                {
                    treeView1.Nodes.Add(child);
                    DataTable dtChild = this.GetData("SELECT tenlop, malop FROM Lophoc WHERE makhoa = \'" + child.Tag+"\'");
                    PopulateTreeView(dtChild, child);
                }
                else
                {
                    treeNode.Nodes.Add(child);
                }
                
                
                
            }
        }

        private DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source=LAPTOP-3IINN1IQ;Initial Catalog=SINHVIEN;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
                return dt;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string choose1 = treeView1.SelectedNode.FullPath.Split('\\')[0];
            string choose2 = null;
            try { choose2 = treeView1.SelectedNode.FullPath.Split('\\')[1]; }
            catch (Exception) { }
            
            string query;
            if (choose2 == null)
            {
                query = $"select hoten, diachi, masv, sinhvien.malop from sinhvien, lophoc , khoa where sinhvien.malop = lophoc.malop and lophoc.makhoa = khoa.makhoa and khoa.tenkhoa = N\'{choose1}\'";
            }
            else
            {
                query = $"select hoten, diachi, masv, sinhvien.malop from sinhvien, lophoc , khoa where sinhvien.malop = lophoc.malop and lophoc.makhoa = khoa.makhoa and lophoc.tenlop = N\'{choose2}\' and khoa.tenkhoa = N\'{choose1}\'";
            }
            
            DataTable dt = this.GetData(query);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string idsinhvien="None";
            //User selected WHOLE ROW (by clicking in the margin)
            if (dgv.SelectedRows.Count > 0)
            {
                idsinhvien = dgv.SelectedRows[0].Cells[2].Value.ToString();
                
                
            }
            try
            {
                textBox_HoTen.Text = dgv.SelectedRows[0].Cells[0].Value.ToString();
                textBox_diachi.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                textBox_MaSV.Text = idsinhvien;
                textBox_Malop.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();
            }
            catch (Exception) { };
            
            string query = $"select  tenmon, diemthuongxuyen, diemchuyencan, diemcuoiky, diemtongket from bangdiem, MONHOC where bangdiem.mamon = monhoc.mamon and masv =\'{idsinhvien}\'";
            System.Data.DataTable dt = this.GetData(query);
            dataGridView2.Refresh();
            dataGridView2.DataSource = dt;



            //User selected a cell (show the first cell in the row)
            
            //User selected a cell, show that cell
            //if (dgv.SelectedCells.Count > 0)
            //    MessageBox.Show(dgv.SelectedCells[0].Value.ToString());
        }

        private void button_xuatBaocao_Click(object sender, EventArgs e)
        {
            Baocao bc = new Baocao();
            
            string idsinhvien = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            bc.masv = idsinhvien;
            bc.tensv = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            bc.diachi = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            bc.khoa = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string query = $"select format(avg(diemtongket),'N2') from bangdiem where masv=\'{idsinhvien}\'";
            DataTable dt1 = this.GetData(query);
            bc.diemtrungbinhmon = dt1.Rows[0][0].ToString();
            query = $"select  tenmon, diemthuongxuyen, diemchuyencan, diemcuoiky, diemtongket from bangdiem, MONHOC where bangdiem.mamon = monhoc.mamon and masv =\'{idsinhvien}\'";
            System.Data.DataTable dt = this.GetData(query) ;
            dataGridView2.DataSource = dt;
            bc.dataTable = dt;
            
            Microsoft.Reporting.WinForms.ReportDataSource rprtDTSource = new Microsoft.Reporting.WinForms.ReportDataSource(dt.TableName, dt);
            //bc.reportViewer1.LocalReport.DataSources.Add(rprtDTSource);
            bc.reportViewer1.LocalReport.DataSources.Add(rprtDTSource);
           
            bc.ShowDialog();
        }

        private void textBox_HoTen_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_xoa_Click(object sender, EventArgs e)
        {
            string query;
            string id = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            query = $"delete from sinhvien where masv = \'{id}\'";
            this.GetData(query);
        }

        private void button_sua_Click(object sender, EventArgs e)
        {
            string query; 
            string id = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string tensv = textBox_HoTen.Text;
            string diachi = textBox_diachi.Text;
            string malop = textBox_Malop.Text;
            query = $"update sinhvien " +
                $"set hoten = N\'{tensv}\', diachi = N\'{diachi}\', malop =\'{malop}\', masv =\'{textBox_MaSV.Text}\'" +
                $"where masv = \'{id}\' ";
            this.GetData(query);
            string choose1 = treeView1.SelectedNode.FullPath.Split('\\')[0];
            string choose2 = null;
            try { choose2 = treeView1.SelectedNode.FullPath.Split('\\')[1]; }
            catch (Exception) { }

            
            if (choose2 == null)
            {
                query = $"select hoten, diachi, masv, sinhvien.malop from sinhvien, lophoc , khoa where sinhvien.malop = lophoc.malop and lophoc.makhoa = khoa.makhoa and khoa.tenkhoa = N\'{choose1}\'";
            }
            else
            {
                query = $"select hoten, diachi, masv, sinhvien.malop from sinhvien, lophoc , khoa where sinhvien.malop = lophoc.malop and lophoc.makhoa = khoa.makhoa and lophoc.tenlop = N\'{choose2}\' and khoa.tenkhoa = N\'{choose1}\'";
            }
            System.Data.DataTable dt = this.GetData(query);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            
        }

        private void textBox_mamon_Validating(object sender, CancelEventArgs e)
        {

        }

        private void textBox_chuyencan_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.Match(textBox_chuyencan.Text, @"^(([0-9]?((\.([0-9])*)?))|(10))$").Success)
            {
                e.Cancel = true;
                textBox_mamon.Focus();
                errorProvider1.SetError(textBox_chuyencan, "Điểm không hợp lệ");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox_chuyencan, null);
            }
        }

        private void textBox_thuongxuyen_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.Match(textBox_thuongxuyen.Text, @"^(([0-9]?((\.([0-9])*)?))|(10))$").Success)
            {
                e.Cancel = true;
                textBox_mamon.Focus();
                errorProvider1.SetError(textBox_thuongxuyen, "Điểm không hợp lệ");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox_thuongxuyen, null);
            }
        }

        private void textBox_cuoiky_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.Match(textBox_cuoiky.Text, @"^(([0-9]?((\.([0-9])*)?))|(10))$").Success)
            {
                e.Cancel = true;
                textBox_mamon.Focus();
                errorProvider1.SetError(textBox_cuoiky, "Điểm không hợp lệ");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox_cuoiky, null);
            }
        }
    }
}
