using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            textBox1.Text = "   Đây là công cụ tự động tạo các lớp theo mô hình 3 lớp có tên gọi T-LOG (Three Layer cOde Generator).";
            textBox1.Text += "\r\n   T-LOG được phát triển trên nền tảng: .NET Framework 4.5 và Visual Studio 2015.";


        }
    }
}
