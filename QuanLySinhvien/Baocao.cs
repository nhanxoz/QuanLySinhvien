using Microsoft.Reporting.WinForms;
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
    public partial class Baocao : Form
    {
        public DataTable dataTable = new DataTable();
        public string tensv;
        public string diachi;
        public string masv;
        public string khoa;
        public string diemtrungbinhmon;
        public string xeploai;
        public Baocao()
        {
            InitializeComponent();
        }

        private void Baocao_Load(object sender, EventArgs e)
        {
            ReportParameterCollection rpc = new ReportParameterCollection();
            rpc.Add(new ReportParameter("day", DateTime.Today.Day.ToString()));
            rpc.Add(new ReportParameter("month", DateTime.Today.Month.ToString()));
            rpc.Add(new ReportParameter("year", DateTime.Today.Year.ToString()));
            rpc.Add(new ReportParameter("tensv", tensv));
            rpc.Add(new ReportParameter("diachi", diachi));
            rpc.Add(new ReportParameter("khoa", khoa));
            rpc.Add(new ReportParameter("masv", masv));
            rpc.Add(new ReportParameter("diemtrungbinhmon", diemtrungbinhmon));
            rpc.Add(new ReportParameter("xeploai", xeploai));
            sinhvien sv = new sinhvien();
            sv.tenmon = "1";
            sv.tensv = "1";
            BindingSource bd = new BindingSource();
            bd.Add(dataTable);
            //Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1",dataTable);
            //this.reportViewer1.LocalReport.DataSources.Clear();

            //this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            //this.reportViewer1.LocalReport.DataSources[0].Value = this.reportViewer1.LocalReport.DataSources[2].Value;
            this.reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource RDS1 = new ReportDataSource("DataSet1", dataTable);
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            
            this.reportViewer1.LocalReport.DataSources.Add(RDS1);
            this.reportViewer1.LocalReport.SetParameters(rpc);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
