using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// giao diện Ribbon: https://www.codeproject.com/Articles/364272/Easily-Add-a-Ribbon-into-a-WinForms-Application-Cs

namespace CodeGenerator
{
    public partial class frmMainForm : Form
    {
        BackgroundWorker bgw = new BackgroundWorker();
        public frmMainForm()
        {
            InitializeComponent();
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void InitControls()
        {
            this.cmbDatabase.SelectedIndex = 0;
            this.statusLabel.Text = "Chọn database: " + cmbDatabase.SelectedItem.ToString();
            this.btnCreateVSP.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnSaveAll.Enabled = false;
            this.btnGenClass.Enabled = false;
        }

        private void EnableControls(bool b)
        {
            this.btnCreateVSP.Enabled = b;
            this.btnSaveAll.Enabled = b;
            this.btnGenClass.Enabled = b;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Kết nối database";
            frmConnectDB frmConnect = new frmConnectDB();
            frmConnect.ShowDialog();

            if(Global.connectionString != "")
            {
                this.btnGenClass.Enabled = true;
                //Load các tables trong csdl vào Treeview
                FillTablesTree();
            }
            
        }

        // Đọc các tables trong csdl vào TreeView
        private void FillTablesTree()
        {
            TreeNode node = treeTables.Nodes.Add("Tables");
            DataTable dt = LoadDatabaseTables();

            treeTables.Nodes[0].Nodes.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tbl = dt.Rows[i]["TABLE_NAME"].ToString();
                node.Nodes.Add(tbl);
            }
            treeTables.Nodes[0].ExpandAll();
        }

        private void txtGenClass_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Sinh các lớp";
            try
            {
                DataTable dt = LoadDatabaseTables();
                FillList(dt);

                this.btnCreateVSP.Enabled = true;
                this.btnSaveAll.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Đọc các tables từ csdl
        private DataTable LoadDatabaseTables()
        {
            string strSQL = "SELECT * FROM INFORMATION_SCHEMA.TABLES " 
                           + " WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'dtproperties' AND TABLE_NAME<>'sysdiagrams' " 
                           + " ORDER BY TABLE_NAME";
            SqlDataAdapter da = new SqlDataAdapter(strSQL, Global.connectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // Cách khác đọc các tables từ csdl
        public List<String> ListTables(String connectionString)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES "
                                + "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'dtproperties' AND TABLE_NAME<>'sysdiagrams'"
                                + " ORDER BY TABLE_NAME";

                SqlDataReader reader = cmd.ExecuteReader();

                List<String> list = new List<String>();
                while (reader.Read())
                {
                    list.Add(reader["TABLE_NAME"].ToString());
                }

                reader.Close();
                reader.Dispose();
                con.Close();
                con.Dispose();

                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Class Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<String>();
            }
        }

        private void FillList(DataTable dt)
        {
            lstClasses.Items.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tbl = dt.Rows[i]["TABLE_NAME"].ToString();
                string str = tbl + "-Entities";
                lstClasses.Items.Add(str);
                str = tbl + "-DAL";
                lstClasses.Items.Add(str);
                str = tbl + "-BLL";
                lstClasses.Items.Add(str);
                str = tbl + "-GUI";
                lstClasses.Items.Add(str);
            }
            lstClasses.Items.Add("KetNoi");
        }

        private void lstClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;

            try
            {
                if (lstClasses.SelectedItem == null)
                    return;

                GenClasses gen = null;
                string txt = lstClasses.SelectedItem.ToString();
                if (txt.Equals("KetNoi"))
                {
                    lblFileName.Text = txt + ".cs";
                    gen = new GenClasses("notable", txtNamespace.Text, Global.connectionString);
                    txtSource.Text = gen.Get_Chuoi();
                    return;
                }

                string[] s = txt.Split('-');
                string tablename = s[0].Trim();
                lblFileName.Text = tablename + "_" + s[1].Trim() + ".cs";
                statusLabel.Text = lblFileName.Text;

                gen = new GenClasses(tablename, txtNamespace.Text, Global.connectionString);
                switch (s[1].Trim())
                {
                    case "Entities":
                        txtSource.Text = gen.GetInfo();
                        break;
                    case "DAL":
                        txtSource.Text = gen.GetDAL();
                        break;
                    case "BLL":
                        txtSource.Text = gen.GetBLL();
                        break;
                    case "GUI":
                        txtSource.Text = gen.GetGUI();
                        break;
                        //Case "Store Procedure"
                        //    txtSource.Text = gen.GetStore()
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Lưu tất cả các lớp";
            Cursor.Current = Cursors.WaitCursor;
                        

            folderBrowserDialog1.ShowDialog();  // Mở folder để lưu trữ
            string folderStorage = folderBrowserDialog1.SelectedPath;
                        
            //Khai báo các sub folder
            string BLLsubFolder = "";
            string DALsubFolder = "";
            string GUIsubFolder = "";
            string EntitiessubFolder = "";

            GenClasses gen = null;

            for (int i = 0; i < lstClasses.Items.Count - 1; i++)  // Giới hạn i chạy như vậy để xử lý item cuối cùng 
            {                                                     // trong lstClasses là lớp KetNoi
                string txt = lstClasses.Items[i].ToString();
                string[] s = txt.Split('-');
                string tablename = s[0].Trim();
                lblFileName.Text = tablename + "_" + s[1].Trim();
                gen = new GenClasses(tablename, txtNamespace.Text, Global.connectionString);
                switch (s[1].Trim())
                {
                    case "Entities":
                        txtSource.Text = gen.GetInfo();
                        break;
                    case "DAL":
                        txtSource.Text = gen.GetDAL();
                        break;
                    case "BLL":
                        txtSource.Text = gen.GetBLL();
                        break;
                    case "GUI":
                        txtSource.Text = gen.GetGUI();
                        break;
                }

                //Tạo subfolder có tên Namespace_BLL, Namespace_DAL, Namespace_GUI, Namespace_Entities
                BLLsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_BLL");
                System.IO.Directory.CreateDirectory(BLLsubFolder);
                DALsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_DAL");
                System.IO.Directory.CreateDirectory(DALsubFolder);
                GUIsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_GUI");
                System.IO.Directory.CreateDirectory(GUIsubFolder);
                EntitiessubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_Entities");
                System.IO.Directory.CreateDirectory(EntitiessubFolder);
                
                //Lưu các file class .cs vào các sub folder tương ứng
                string layer = lblFileName.Text.Substring(lblFileName.Text.Length - 3); 
                string layer1 = lblFileName.Text.Substring(lblFileName.Text.Length - 8); 

                if ((layer == "DAL"))
                {
                    txtSource.SaveFile(DALsubFolder + "\\" + lblFileName.Text + ".cs", RichTextBoxStreamType.PlainText);
                }
                else if ((layer == "BLL"))
                {
                    txtSource.SaveFile(BLLsubFolder + "\\" + lblFileName.Text + ".cs", RichTextBoxStreamType.PlainText);
                }
                else  if ((layer == "GUI"))
                {
                    txtSource.SaveFile(GUIsubFolder + "\\" + lblFileName.Text + ".cs", RichTextBoxStreamType.PlainText);
                }

                if ((layer1 == "Entities"))
                {
                    txtSource.SaveFile(EntitiessubFolder + "\\" + lblFileName.Text + ".cs", RichTextBoxStreamType.PlainText);
                }
            }

            //Tạo lớp kết nối và lưu vào sub folder Namespace_DAL
            string classKN = "KetNoi.cs";
            txtSource.Text = gen.Get_Chuoi();
            txtSource.SaveFile(DALsubFolder + "\\" + classKN, RichTextBoxStreamType.PlainText);

            //Tạo các store procedures, lưu trữ file .sql vào folder DAL
            ExecuteProcedures(DALsubFolder);        
            
            Cursor.Current = Cursors.Default;

            MessageBox.Show("Đã lưu các file class thành công.\nThư mục lưu trữ: " 
                          + folderStorage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            System.Diagnostics.Process.Start(folderStorage); 
        }

        //Tạo các store procedures: GetAll, Insert, Update, Delete, GetById
        private void ExecuteProcedures(string folder)
        {
            List<String> tables = ListTables(Global.connectionString);
            GenStoreProcedures genSP = new GenStoreProcedures();

            string sqlInsert = "";
            string sqlSelectAll = "";
            string sqlSelectById = "";
            string sqlDelete = "";
            string sqlUpdate = "";

            foreach (var table in tables)
            {
                sqlInsert += genSP.GenInsertSP(table);
                sqlSelectAll += genSP.GenSelectAllSP(table);
                sqlSelectById += genSP.GenSelectByIdSP(table);
                sqlDelete += genSP.GenDeleteSP(table);
                sqlUpdate += genSP.GenUpdateSP(table);
            }

            try
            {
                File.WriteAllText(folder + "\\spInsert.sql", sqlInsert);
                File.WriteAllText(folder + "\\spSelectAll.sql", sqlSelectAll);
                File.WriteAllText(folder + "\\spSelectById.sql", sqlSelectById);
                File.WriteAllText(folder + "\\spUpdate.sql", sqlUpdate);
                File.WriteAllText(folder + "\\spDelete.sql", sqlDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Lưu class";

            folderBrowserDialog1.ShowDialog();  // Mở folder để lưu trữ
            string folderStorage = folderBrowserDialog1.SelectedPath;
            //Lưu file vào folder chỉ định
            txtSource.SaveFile(folderStorage + "\\" + lblFileName.Text, RichTextBoxStreamType.PlainText);

            System.Diagnostics.Process.Start(folderStorage);
        }

        private void LoadProgressGenCode()
        {
            statusProgressBar1.Visible = true;
            statusLabel1.Visible = true;
            statusLabel2.Visible = true;

            bgw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bgw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            bgw.WorkerReportsProgress = true;
            bgw.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int total = 300; //some number (this is your variable to change)!!

            for (int i = 0; i <= total; i++) //some number (total)
            {
                System.Threading.Thread.Sleep(10);
                int percents = (i * 100) / total;
                bgw.ReportProgress(percents, i);

            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusProgressBar1.Value = e.ProgressPercentage;
            statusLabel2.Text = String.Format("Progress: {0} %", e.ProgressPercentage);
            if (e.ProgressPercentage == 100)
            {
                statusLabel2.Text = String.Format("Generated.. {0} %", e.ProgressPercentage);

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusProgressBar1.Visible = false;
            statusLabel1.Visible = false;
            statusLabel2.Visible = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Trợ giúp";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Thông tin tác giả";
        }
        
        private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusLabel.Text = "Chọn database: " + cmbDatabase.SelectedItem.ToString();
        }
        
    }
}
