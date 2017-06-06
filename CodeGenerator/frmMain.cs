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

using Db;
using Db.DbObject;
using Db.Helpers;
using Db.POCOIterator;
using System.Security.Principal;
using CodeGenerator.Extensions;
using CodeGenerator.POCOWriter;

// giao diện Ribbon: https://www.codeproject.com/Articles/364272/Easily-Add-a-Ribbon-into-a-WinForms-Application-Cs

namespace CodeGenerator
{
    public partial class frmMainForm : Form
    {
       
        public frmMainForm()
        {
            InitializeComponent();
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            InitControls();
            InitProperties();
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

        private void InitProperties()
        {
            IsProperties = true;
            IsNavigationProperties = true;
            IsNavigationPropertiesICollection = true;
            IsNavigationPropertiesVirtual = true;
            Namespace = txtNamespace.Text + "_Entities";
            IsUsing = true;
            IsComments = true;
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
                //FillTablesTree();
                SetConnectionString(Global.connectionString);
                BuildServerTree();
            }
            
        }

        // Đọc các tables trong csdl vào TreeView
        private void FillTablesTree()
        {
            TreeNode node = trvServer.Nodes.Add("Tables");
            DataTable dt = LoadDatabaseTables();

            trvServer.Nodes[0].Nodes.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tbl = dt.Rows[i]["TABLE_NAME"].ToString();
                node.Nodes.Add(tbl);
            }
            trvServer.Nodes[0].ExpandAll();
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
            string StoreProceduresFolder = "";

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

                //Tạo subfolder có tên Namespace_BLL, Namespace_DAL, Namespace_GUI, Namespace_Entities, Namespace_Sql
                BLLsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_BLL");
                System.IO.Directory.CreateDirectory(BLLsubFolder);
                DALsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_DAL");
                System.IO.Directory.CreateDirectory(DALsubFolder);
                GUIsubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_GUI");
                System.IO.Directory.CreateDirectory(GUIsubFolder);
                EntitiessubFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_Entities");
                System.IO.Directory.CreateDirectory(EntitiessubFolder);
                StoreProceduresFolder = System.IO.Path.Combine(folderStorage, txtNamespace.Text + "_Sql");
                System.IO.Directory.CreateDirectory(StoreProceduresFolder);

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

            //Tạo các lớp entities có relationships lưu vào sub folder Namespace_Entities
            SaveEntitiesWithRelationships(EntitiessubFolder);

            //Tạo lớp kết nối và lưu vào sub folder Namespace_DAL
            string classKN = "KetNoi.cs";
            txtSource.Text = gen.Get_Chuoi();
            txtSource.SaveFile(DALsubFolder + "\\" + classKN, RichTextBoxStreamType.PlainText);

            //Tạo các store procedures, lưu trữ file .sql vào sub folder Namespace_Sql
            ExecuteProcedures(StoreProceduresFolder);        
            
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

        #region Gen entity classes with relationships

        #region Properties
        public bool IsProperties { get; set; }
        public bool IsNavigationProperties { get; set; }
        public bool IsNavigationPropertiesICollection { get; set; }
        public bool IsNavigationPropertiesVirtual { get; set; }
        public string Namespace { get; set; }
        public bool IsUsing { get; set; }
        public bool IsComments { get; set; }

        private Server Server;
        private string InitialCatalog;
        #endregion

        #region Load database server on Treeview
        private class NodeTag
        {
            public Db.DbObject.DbType NodeType { get; set; }
            public IDbObject DbObject { get; set; }
        }

        private enum ImageType
        {
            Server,
            Database,
            Folder,
            Table,
            View,
            Procedure,
            Function,
            Column,
            PrimaryKey,
            ForeignKey,
            UniqueKey,
            Index
        }

        private void SetConnectionString(string connectionString)
        {
            DbHelper.ConnectionString = connectionString;

            int index = connectionString.IndexOf("Data Source=");
            if (index != -1)
            {
                string server = connectionString.Substring(index + "Data Source=".Length);
                index = server.IndexOf(';');
                if (index != -1)
                    server = server.Substring(0, index);
                string instanceName = null;
                index = server.LastIndexOf("\\");
                if (index != -1)
                {
                    instanceName = server.Substring(index + 1);
                    server = server.Substring(0, index);
                }

                Server = new Server()
                {
                    ServerName = server,
                    InstanceName = instanceName
                };

                index = connectionString.IndexOf("User ID=");
                if (index != -1)
                {
                    string userId = connectionString.Substring(index + "User ID=".Length);
                    index = userId.IndexOf(';');
                    if (index != -1)
                        userId = userId.Substring(0, index);
                    Server.UserId = userId;
                }
                else
                {
                    index = connectionString.IndexOf("Integrated Security=True");
                    if (index != -1)
                        Server.UserId = WindowsIdentity.GetCurrent().Name;
                }

                Server.Version = DbHelper.GetServerVersion();
            }

            index = connectionString.IndexOf("Initial Catalog=");
            if (index != -1)
            {
                string initialCatalog = connectionString.Substring(index + "Initial Catalog=".Length);
                index = initialCatalog.IndexOf(';');
                if (index != -1)
                    initialCatalog = initialCatalog.Substring(0, index);
                InitialCatalog = initialCatalog;
            }
        }

        private TreeNode AddDatabaseNode(TreeNode serverNode, Database database)
        {
            TreeNode databaseNode = BuildDatabaseNode(database);
            serverNode.Nodes.AddSorted(databaseNode);
            serverNode.Expand();
            Application.DoEvents();
            return databaseNode;
        }

        private TreeNode AddTablesNode(TreeNode tablesNode, Database databaseCurrent, TreeNode databaseNodeCurrent)
        {
            if (tablesNode == null)
            {
                tablesNode = BuildTablesNode(databaseCurrent);
                databaseNodeCurrent.Nodes.Insert(0, tablesNode);
                Application.DoEvents();
            }
            return tablesNode;
        }

        private void AddTableNode(TreeNode tablesNode, Table table)
        {
            TreeNode tableNode = BuildTableNode(table);
            tablesNode.Nodes.AddSorted(tableNode);
            Application.DoEvents();
        }

        private TreeNode BuildServerNode()
        {
            string serverName = Server.ToString();
            if (string.IsNullOrEmpty(Server.Version) == false)
            {
                serverName += string.Format(" (SQL Server {0}", Server.Version.Substring(0, Server.Version.LastIndexOf('.')));
                if (string.IsNullOrEmpty(Server.UserId) == false)
                    serverName += " - " + Server.UserId;
                serverName += ")";
            }
            TreeNode serverNode = new TreeNode(serverName);
            serverNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Server, DbObject = Server };
            serverNode.ImageIndex = (int)ImageType.Server;
            serverNode.SelectedImageIndex = (int)ImageType.Server;
            return serverNode;
        }

        private TreeNode BuildDatabaseNode(Database database)
        {
            TreeNode databaseNode = new TreeNode(database.ToString());
            databaseNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Database, DbObject = database };
            databaseNode.ImageIndex = (int)ImageType.Database;
            databaseNode.SelectedImageIndex = (int)ImageType.Database;
            return databaseNode;
        }

        private TreeNode BuildTablesNode(Database database)
        {
            TreeNode tablesNode = new TreeNode("Tables");
            tablesNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Tables, DbObject = database };
            tablesNode.ImageIndex = (int)ImageType.Folder;
            tablesNode.SelectedImageIndex = (int)ImageType.Folder;
            return tablesNode;
        }

        private TreeNode BuildTableNode(Table table)
        {
            TreeNode tableNode = new TreeNode(table.ToString());
            tableNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Table, DbObject = table };
            tableNode.ImageIndex = (int)ImageType.Table;
            tableNode.SelectedImageIndex = (int)ImageType.Table;

            TreeNode columnsNode = new TreeNode("Columns");
            columnsNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
            columnsNode.ImageIndex = (int)ImageType.Folder;
            columnsNode.SelectedImageIndex = (int)ImageType.Folder;
            tableNode.Nodes.Add(columnsNode);

            if (table.TableColumns != null)
            {
                foreach (TableColumn column in table.TableColumns.OrderBy(c => c.ordinal_position ?? 0))
                    columnsNode.Nodes.Add(BuildTableColumn(column));

                if (table.TableColumns.Exists(c => c.IsPrimaryKey))
                    tableNode.Nodes.Add(BuildPrimaryKeysNode(table));

                if (table.TableColumns.Exists(c => c.HasUniqueKeys))
                    tableNode.Nodes.Add(BuildUniqueKeysNode(table));

                if (table.TableColumns.Exists(c => c.HasForeignKeys))
                    tableNode.Nodes.Add(BuildForeignKeysNode(table));

            }
            else if (table.Error != null)
            {
                tableNode.ForeColor = Color.Red;
            }

            return tableNode;
        }

        private TreeNode BuildPrimaryKeysNode(Table table)
        {
            TreeNode primaryKeysNode = new TreeNode("Primary Keys");
            primaryKeysNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
            primaryKeysNode.ImageIndex = (int)ImageType.Folder;
            primaryKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var primaryKeys = table.TableColumns.Where(c => c.IsPrimaryKey).Select(c => c.PrimaryKey.Name).Distinct().OrderBy(n => n);
            foreach (string primaryKey in primaryKeys)
            {
                TreeNode columnNode = new TreeNode(primaryKey);
                columnNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.PrimaryKey;
                columnNode.SelectedImageIndex = (int)ImageType.PrimaryKey;
                primaryKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.IsPrimaryKey && c.PrimaryKey.Name == primaryKey).OrderBy(c => c.PrimaryKey.Ordinal))
                    columnNode.Nodes.Add(BuildTableColumn(column));
            }

            return primaryKeysNode;
        }

        private TreeNode BuildUniqueKeysNode(Table table)
        {
            TreeNode uniqueKeysNode = new TreeNode("Unique Keys");
            uniqueKeysNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
            uniqueKeysNode.ImageIndex = (int)ImageType.Folder;
            uniqueKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var uniqueKeys = table.TableColumns.Where(c => c.HasUniqueKeys).SelectMany(c => c.UniqueKeys).Select(uk => uk.Name).Distinct().OrderBy(n => n);
            foreach (string uniqueKey in uniqueKeys)
            {
                TreeNode columnNode = new TreeNode(uniqueKey);
                columnNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.UniqueKey;
                columnNode.SelectedImageIndex = (int)ImageType.UniqueKey;
                uniqueKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.HasUniqueKeys && c.UniqueKeys.Exists(uk => uk.Name == uniqueKey)).OrderBy(c => c.UniqueKeys.First(uk => uk.Name == uniqueKey).Ordinal))
                    columnNode.Nodes.Add(BuildTableColumn(column));
            }

            return uniqueKeysNode;
        }

        private TreeNode BuildForeignKeysNode(Table table)
        {
            TreeNode foreignKeysNode = new TreeNode("Foreign Keys");
            foreignKeysNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
            foreignKeysNode.ImageIndex = (int)ImageType.Folder;
            foreignKeysNode.SelectedImageIndex = (int)ImageType.Folder;

            var foreignKeys = table.TableColumns.Where(c => c.HasForeignKeys).SelectMany(c => c.ForeignKeys).Select(fk => fk.Name).Distinct().OrderBy(n => n);
            foreach (string foreignKey in foreignKeys)
            {
                TreeNode columnNode = new TreeNode(foreignKey);
                columnNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Columns, DbObject = table };
                columnNode.ImageIndex = (int)ImageType.ForeignKey;
                columnNode.SelectedImageIndex = (int)ImageType.ForeignKey;
                foreignKeysNode.Nodes.Add(columnNode);

                foreach (TableColumn column in table.TableColumns.Where(c => c.HasForeignKeys && c.ForeignKeys.Exists(fk => fk.Name == foreignKey)).OrderBy(c => c.ForeignKeys.First(fk => fk.Name == foreignKey).Ordinal))
                {
                    ForeignKey fk = column.ForeignKeys.First(fk1 => fk1.Name == foreignKey);
                    string primary = string.Format(" -> {0}.{1}.{2}", fk.Primary_Schema, fk.Primary_Table, fk.Primary_Column);
                    columnNode.Nodes.Add(BuildTableColumn(column, primary));
                }
            }

            return foreignKeysNode;
        }

        private TreeNode BuildTableColumn(TableColumn column, string postfix = null)
        {
            TreeNode tableColumnNode = new TreeNode(column.ToStringFull() + postfix);
            tableColumnNode.Tag = new NodeTag() { NodeType = Db.DbObject.DbType.Column, DbObject = column };
            tableColumnNode.ImageIndex = (int)(column.IsPrimaryKey ? ImageType.PrimaryKey : (column.HasForeignKeys ? ImageType.ForeignKey : ImageType.Column));
            tableColumnNode.SelectedImageIndex = (int)(column.IsPrimaryKey ? ImageType.PrimaryKey : (column.HasForeignKeys ? ImageType.ForeignKey : ImageType.Column));
            return tableColumnNode;
        }

        private void BuildServerTree()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                TreeNode serverNode = BuildServerNode();
                trvServer.Nodes.Add(serverNode);
                Application.DoEvents();

                Database databaseCurrent = null;
                TreeNode databaseNodeCurrent = null;
                TreeNode tablesNode = null;

                Action<IDbObject> buildingDbObject = (IDbObject dbObject) =>
                {
                    if (dbObject is Database)
                    {
                        Database database = dbObject as Database;
                        TreeNode databaseNode = AddDatabaseNode(serverNode, database);

                        databaseCurrent = database;
                        databaseNodeCurrent = databaseNode;
                        tablesNode = null;
                    }

                    //ShowBuildingStatus(dbObject);
                };

                Action<IDbObject> builtDbObject = (IDbObject dbObject) =>
                {
                    if (dbObject is Database)
                    {
                        Database database = dbObject as Database;
                        if (database.Errors.Count > 0)
                            databaseNodeCurrent.ForeColor = Color.Red;
                        Application.DoEvents();
                    }
                    else if (dbObject is Table && (dbObject is Db.DbObject.View) == false)
                    {
                        Table table = dbObject as Table;
                        tablesNode = AddTablesNode(tablesNode, databaseCurrent, databaseNodeCurrent);
                        AddTableNode(tablesNode, table);
                    }
                };

                DbHelper.BuildServerSchema(Server, InitialCatalog, buildingDbObject, builtDbObject);

                trvServer.SelectedNode = serverNode;

                Cursor.Current = Cursors.Default;
            }
            catch
            {

            }
        }

        private void trvServer_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Db.DbObject.DbType nodeType = ((NodeTag)e.Node.Tag).NodeType;

            trvServer.HideCheckBox(e.Node);
            e.DrawDefault = true;
        }

        //Xử lý sự kiện khi click trên table thì xem class entity tương ứng được phát sinh trong editor
        private void trvServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            WritePocoToEditor(false);
        }

        #endregion

        #region View entity classes in editor

        private void IterateDbObjects(IDbObjectTraverse dbObject, StringBuilder sb = null)
        {
            IPOCOIterator iterator = GetPOCOIterator(new IDbObjectTraverse[] { dbObject }, sb);
            iterator.Iterate();
        }

        private IPOCOIterator GetPOCOIterator(IEnumerable<IDbObjectTraverse> dbObjects, StringBuilder sb)
        {
            IPOCOWriter pocoWriter = GetPOCOWriter(sb);
            IPOCOIterator iterator = GetPOCOIterator(dbObjects, pocoWriter);

            iterator.IsProperties = IsProperties;
            iterator.IsNavigationProperties = IsNavigationProperties;  //hiển thị thuộc tính Navigation (relationships)
            iterator.IsNavigationPropertiesICollection = IsNavigationPropertiesICollection;  // Kiểu dữ liệu của Navigation
            iterator.IsNavigationPropertiesVirtual = IsNavigationPropertiesVirtual; //thuộc tính Navagate là virtual
            iterator.Namespace = Namespace;  // Namespace cho class
            iterator.IsUsing = IsUsing;   //Thêm từ khóa using cho class
            iterator.IsComments = IsComments;  //Thêm chú thích

            return iterator;
        }

        private IPOCOWriter GetPOCOWriter(StringBuilder sb)
        {
            if (sb == null)
                return new RichTextBoxWriterFactory(txtSource).CreatePOCOWriter();
            else
                return new StringBuilderWriterFactory(sb).CreatePOCOWriter();
        }

        private IPOCOIterator GetPOCOIterator(IEnumerable<IDbObjectTraverse> dbObjects, IPOCOWriter pocoWriter)
        {
            return new DbIteratorFactory().CreateIterator(dbObjects, pocoWriter);
        }

        private void WritePocoToEditor(bool forceRefresh = true)
        {
            TreeNode selectedNode = trvServer.SelectedNode;

            if (selectedNode != null)
            {
                IDbObjectTraverse dbObject = null;
                Db.DbObject.DbType nodeType = ((NodeTag)selectedNode.Tag).NodeType;
                if (nodeType == Db.DbObject.DbType.Table)
                    dbObject = (Table)((NodeTag)selectedNode.Tag).DbObject;

                if (dbObject != null)
                {
                    IterateDbObjects(dbObject);
                }
            }
        }
        #endregion

        #region Save entity classes with relationships

        private List<IDbObjectTraverse> GetSelectedObjects()
        {
            List<IDbObjectTraverse> selectedObjects = new List<IDbObjectTraverse>();
            TreeNode serverNode = trvServer.Nodes[0];
            GetSelectedObjects(serverNode, selectedObjects);
            return selectedObjects;

        }

        private void GetSelectedObjects(TreeNode node, List<IDbObjectTraverse> selectedObjects)
        {
            Db.DbObject.DbType nodeType = ((NodeTag)node.Tag).NodeType;

            bool isDbObjectTraverse = (nodeType == Db.DbObject.DbType.Table);

            if (isDbObjectTraverse)
                selectedObjects.Add(((NodeTag)node.Tag).DbObject as IDbObjectTraverse);

            if (isDbObjectTraverse == false)
            {
                foreach (TreeNode child in node.Nodes)
                    GetSelectedObjects(child, selectedObjects);
            }
        }

        private List<IDbObjectTraverse> GetExportObjects()
        {
            List<IDbObjectTraverse> exportResults = GetSelectedObjects();

            return exportResults;
        }


        private void WritePocoToFiles(List<IDbObjectTraverse> exportObjects, string folder)
        {
            string fileName;
            foreach (var dbObject in exportObjects)
            {
                StringBuilder sb = new StringBuilder();
                IterateDbObjects(dbObject, sb);

                fileName = dbObject.ClassName + ".cs";
                foreach (char c in Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(c.ToString(), string.Empty);

                WritePocoToFile(folder, fileName, sb.ToString());
            }

        }

        private bool WritePocoToFile(string folder, string fileName, string content)
        {
            try
            {
                string path = folder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + fileName;
                File.WriteAllText(path, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveEntitiesWithRelationships(string folder)
        {
            List<IDbObjectTraverse> exportObjects = GetExportObjects();
            if (exportObjects.Count == 0)
                return;

            WritePocoToFiles(exportObjects, folder);
        }

        #endregion
        #endregion

        #region Progress bar Background
        private BackgroundWorker bgw = new BackgroundWorker();
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

        #endregion
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
