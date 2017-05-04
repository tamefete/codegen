using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace ClassGenerator
{
    public partial class MainForm : Form
    {
        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        private Boolean UserCancelled = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void optIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            lblPassword.Enabled = false;
            lblUsername.Enabled = false;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
        }

        private void optSqlAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            lblPassword.Enabled = true;
            lblUsername.Enabled = true;
            txtUsername.Enabled = true;
            txtPassword.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            diaFolder.ShowDialog();
            txtOutput.Text = diaFolder.SelectedPath;
        }

        private void btnListTables_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            builder.DataSource = txtServer.Text;
            builder.IntegratedSecurity = optIntegratedSecurity.Checked;

            if (optSqlAuthentication.Checked)
            {
                builder.UserID = txtUsername.Text;
                builder.Password = txtPassword.Text;
            }

            builder.InitialCatalog = txtDatabase.Text;

            List<String> tables = DBRepository.ListTables(builder.ConnectionString);

            tvwMain.Nodes[0].Nodes.Clear();

            foreach (String table in tables)
            {
                tvwMain.Nodes[0].Nodes.Add(table);
            }

            btnSelectAll.Enabled = tables.Count > 0;
            btnSelectAll.Text = "&Select All";

            tvwMain.Nodes[0].ExpandAll();

            Cursor.Current = Cursors.Default;

        }

        private void tvwMain_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            int count = 0;

            foreach (TreeNode node in tvwMain.Nodes[0].Nodes)
            {
                if (node.Checked)
                    count += 1;
            }

            btnUseTables.Enabled = count > 0;

            Cursor.Current = Cursors.Default;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (btnSelectAll.Text == "&Select All")
            {
                foreach (TreeNode node in tvwMain.Nodes[0].Nodes)
                {
                    node.Checked = true;
                }
                btnSelectAll.Text = "&Deselect All";
            }
            else
            {
                foreach (TreeNode node in tvwMain.Nodes[0].Nodes)
                {
                    node.Checked = false;
                }
                btnSelectAll.Text = "&Select All";
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnUseTables_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            lvwMain.Items.Clear();

            foreach (TreeNode node in tvwMain.Nodes[0].Nodes)
            {
                if (node.Checked)
                {
                    ListViewItem lvi = new ListViewItem(node.Text);
                    lvi.SubItems.Add("");
                    lvwMain.Items.Add(lvi);
                }
            }

            btnBuild.Enabled = DoValidation();

            Cursor.Current = Cursors.Default;
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
           btnBuild.Enabled = DoValidation();
        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
           btnBuild.Enabled = DoValidation();
        }

        private Boolean DoValidation()
        {
            Boolean done = true;

            if (!Directory.Exists(txtOutput.Text))
            {
                btnOpenFolder.Enabled = false;
                done = false;
            }
            else
                btnOpenFolder.Enabled = true;

            if (String.IsNullOrWhiteSpace(txtNamespace.Text))
                done = false;

            if (lvwMain.Items.Count == 0)
                done = false;

            return done;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            UserCancelled = false;
            btnCancel.Enabled = true;
            Application.DoEvents();

            foreach (ListViewItem lvi in lvwMain.Items)
            {
                CodeFile cf = new CodeFile();
                CodeFile c = cf.WriteCode(builder.ConnectionString, lvi.Text, txtOutput.Text, txtNamespace.Text);
                lvi.SubItems[1].Text = c.ClassCodePath;
                lvi.EnsureVisible();

                Application.DoEvents();

                if (UserCancelled)
                {
                    MessageBox.Show("Build process cancelled.", "Code Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }

            btnCancel.Enabled = false;
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserCancelled = true;
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txtOutput.Text);
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // MainForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(627, 397);
        //    this.Name = "MainForm";
        //    this.ResumeLayout(false);

        //}
    }
}
