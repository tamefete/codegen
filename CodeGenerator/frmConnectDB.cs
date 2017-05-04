using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class frmConnectDB : Form
    {
        public frmConnectDB()
        {
            InitializeComponent();
        }

        private void frmConnectDB_Load(object sender, EventArgs e)
        {
            this.cmbAuthentication.SelectedIndex = 0;
            this.txtUsername.Enabled = false;
            this.txtPassword.Enabled = false;
        }

        private void cmbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAuthentication.SelectedIndex == 1)
            {
                this.txtUsername.Enabled = true;
                this.txtPassword.Enabled = true;
                this.txtUsername.Text = "sa";
                this.txtPassword.Text = "123";
            }
            else
            {
                this.txtUsername.Enabled = false;
                this.txtPassword.Enabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbAuthentication.SelectedIndex == 0)
            {
                Global.connectionString = "Data Source=" + txtServer.Text + ";Initial Catalog=" + txtDatabase.Text 
                                         + ";Integrated Security=True";
            }
            else
            {
                Global.connectionString = "Data Source=" + txtServer.Text + ";Initial Catalog=" + txtDatabase.Text 
                                         + ";User Id=" + txtUsername.Text + ";Password=" + txtPassword.Text;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Global.connectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            lblConnectMessage.ForeColor = Color.Blue;
                            lblConnectMessage.Text = "Kết nối database thành công";
                        }
                        //Load database tables vào Treeview trên form Main

                    }
                    catch (SqlException)
                    {
                        lblConnectMessage.ForeColor = Color.Red;
                        lblConnectMessage.Text = "Kết nối database không thành công";
                        Global.connectionString = "";
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                        lblConnectMessage.Visible = true;
                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                lblConnectMessage.ForeColor = Color.Red;
                lblConnectMessage.Text = "Kết nối database không thành công";
            }
        }

        
    }
}
