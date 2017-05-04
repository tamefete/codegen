namespace ClassGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Tables");
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnListTables = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.optSqlAuthentication = new System.Windows.Forms.RadioButton();
            this.optIntegratedSecurity = new System.Windows.Forms.RadioButton();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnUseTables = new System.Windows.Forms.Button();
            this.tvwMain = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.lvwMain = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.diaFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 61);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Generates C# classes from database tables.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "GenCode Tool";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnListTables);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.lblUsername);
            this.groupBox1.Controls.Add(this.optSqlAuthentication);
            this.groupBox1.Controls.Add(this.optIntegratedSecurity);
            this.groupBox1.Controls.Add(this.txtDatabase);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(11, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Setup Database Connection";
            // 
            // btnListTables
            // 
            this.btnListTables.Location = new System.Drawing.Point(141, 170);
            this.btnListTables.Name = "btnListTables";
            this.btnListTables.Size = new System.Drawing.Size(131, 23);
            this.btnListTables.TabIndex = 6;
            this.btnListTables.Text = "&Get Connect";
            this.tipMain.SetToolTip(this.btnListTables, "Click this button to load all the tables for the specified database.");
            this.btnListTables.UseVisualStyleBackColor = true;
            this.btnListTables.Click += new System.EventHandler(this.btnListTables_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(80, 144);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(192, 20);
            this.txtPassword.TabIndex = 5;
            this.tipMain.SetToolTip(this.txtPassword, "Enter the login password.");
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(80, 118);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(192, 20);
            this.txtUsername.TabIndex = 4;
            this.tipMain.SetToolTip(this.txtUsername, "Enter the login username.");
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Enabled = false;
            this.lblPassword.Location = new System.Drawing.Point(18, 147);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Enabled = false;
            this.lblUsername.Location = new System.Drawing.Point(18, 121);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username:";
            // 
            // optSqlAuthentication
            // 
            this.optSqlAuthentication.AutoSize = true;
            this.optSqlAuthentication.Location = new System.Drawing.Point(9, 92);
            this.optSqlAuthentication.Name = "optSqlAuthentication";
            this.optSqlAuthentication.Size = new System.Drawing.Size(139, 17);
            this.optSqlAuthentication.TabIndex = 3;
            this.optSqlAuthentication.Text = "Use SQL Authentication";
            this.tipMain.SetToolTip(this.optSqlAuthentication, "Select this option to use SQL Server Authentication.");
            this.optSqlAuthentication.UseVisualStyleBackColor = true;
            this.optSqlAuthentication.CheckedChanged += new System.EventHandler(this.optSqlAuthentication_CheckedChanged);
            // 
            // optIntegratedSecurity
            // 
            this.optIntegratedSecurity.AutoSize = true;
            this.optIntegratedSecurity.Checked = true;
            this.optIntegratedSecurity.Location = new System.Drawing.Point(9, 70);
            this.optIntegratedSecurity.Name = "optIntegratedSecurity";
            this.optIntegratedSecurity.Size = new System.Drawing.Size(136, 17);
            this.optIntegratedSecurity.TabIndex = 2;
            this.optIntegratedSecurity.TabStop = true;
            this.optIntegratedSecurity.Text = "Use Integrated Security";
            this.tipMain.SetToolTip(this.optIntegratedSecurity, "Select this option to use Integrated Security.");
            this.optIntegratedSecurity.UseVisualStyleBackColor = true;
            this.optIntegratedSecurity.CheckedChanged += new System.EventHandler(this.optIntegratedSecurity_CheckedChanged);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(80, 45);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(192, 20);
            this.txtDatabase.TabIndex = 1;
            this.tipMain.SetToolTip(this.txtDatabase, "Enter the name of the database to connect to.");
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(80, 19);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(192, 20);
            this.txtServer.TabIndex = 0;
            this.tipMain.SetToolTip(this.txtServer, "Enter the server name and instance name. For example, Server1\\SQL2008");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Database:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Server:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectAll);
            this.groupBox2.Controls.Add(this.btnUseTables);
            this.groupBox2.Controls.Add(this.tvwMain);
            this.groupBox2.Location = new System.Drawing.Point(11, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 244);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Select Database Tables";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Enabled = false;
            this.btnSelectAll.Location = new System.Drawing.Point(6, 215);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(131, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "&Select All";
            this.tipMain.SetToolTip(this.btnSelectAll, "This button selects and deselects all the tables in the tree.");
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnUseTables
            // 
            this.btnUseTables.Enabled = false;
            this.btnUseTables.Location = new System.Drawing.Point(141, 215);
            this.btnUseTables.Name = "btnUseTables";
            this.btnUseTables.Size = new System.Drawing.Size(131, 23);
            this.btnUseTables.TabIndex = 2;
            this.btnUseTables.Text = "&Use Selected Tables";
            this.tipMain.SetToolTip(this.btnUseTables, "This button selects the checked items and inserts them into the listview on the r" +
                    "ight to await processing.");
            this.btnUseTables.UseCompatibleTextRendering = true;
            this.btnUseTables.UseVisualStyleBackColor = true;
            this.btnUseTables.Click += new System.EventHandler(this.btnUseTables_Click);
            // 
            // tvwMain
            // 
            this.tvwMain.CheckBoxes = true;
            this.tvwMain.Location = new System.Drawing.Point(6, 19);
            this.tvwMain.Name = "tvwMain";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Tables";
            this.tvwMain.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvwMain.Size = new System.Drawing.Size(266, 190);
            this.tvwMain.TabIndex = 0;
            this.tipMain.SetToolTip(this.tvwMain, "Select the tables that you wish to create classes for.");
            this.tvwMain.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwMain_AfterCheck);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnBrowse);
            this.groupBox3.Controls.Add(this.txtOutput);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(295, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(460, 52);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Select Output Folder";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(379, 17);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "B&rowse";
            this.tipMain.SetToolTip(this.btnBrowse, "This button opens a dialog box that allows you to navigate to a folder of your ch" +
                    "oice. This folder will be used as the output folder for all the built class file" +
                    "s.");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(84, 19);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(289, 20);
            this.txtOutput.TabIndex = 0;
            this.tipMain.SetToolTip(this.txtOutput, "Enter the path to the folder where the class files will be generated. ");
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Output Folder:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtNamespace);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(295, 126);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(460, 52);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "4. Add Class Namespace";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(84, 19);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(370, 20);
            this.txtNamespace.TabIndex = 0;
            this.tipMain.SetToolTip(this.txtNamespace, "Add the namespace for the class files. Do not put the \"namespace\" keyword in as t" +
                    "his is done automatically by the application.");
            this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Namespace:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnOpenFolder);
            this.groupBox5.Controls.Add(this.btnCancel);
            this.groupBox5.Controls.Add(this.btnBuild);
            this.groupBox5.Controls.Add(this.lvwMain);
            this.groupBox5.Location = new System.Drawing.Point(295, 186);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(460, 332);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "5. Build Class Files";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Enabled = false;
            this.btnOpenFolder.Location = new System.Drawing.Point(167, 302);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(125, 23);
            this.btnOpenFolder.TabIndex = 3;
            this.btnOpenFolder.Text = "&Open Output Folder";
            this.tipMain.SetToolTip(this.btnOpenFolder, "This button opens the output folder in Windows Explorer.");
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(298, 302);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.tipMain.SetToolTip(this.btnCancel, "This button cancels the build process.");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Enabled = false;
            this.btnBuild.Location = new System.Drawing.Point(379, 302);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 2;
            this.btnBuild.Text = "&Build";
            this.tipMain.SetToolTip(this.btnBuild, "This button begins the build process that creates each of the class files.");
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // lvwMain
            // 
            this.lvwMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvwMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwMain.Location = new System.Drawing.Point(6, 19);
            this.lvwMain.Name = "lvwMain";
            this.lvwMain.Size = new System.Drawing.Size(448, 277);
            this.lvwMain.TabIndex = 0;
            this.tipMain.SetToolTip(this.lvwMain, "This listview displays a list of table names that will be processed.");
            this.lvwMain.UseCompatibleStateImageBehavior = false;
            this.lvwMain.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 142;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Location";
            this.columnHeader2.Width = 298;
            // 
            // diaFolder
            // 
            this.diaFolder.Description = "select the output folder to place your built class files.";
            // 
            // tipMain
            // 
            this.tipMain.IsBalloon = true;
            this.tipMain.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipMain.ToolTipTitle = "GenCode Tool";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 527);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenCode Tool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.RadioButton optSqlAuthentication;
        private System.Windows.Forms.RadioButton optIntegratedSecurity;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnListTables;
        private System.Windows.Forms.Button btnUseTables;
        private System.Windows.Forms.TreeView tvwMain;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.ListView lvwMain;
        private System.Windows.Forms.FolderBrowserDialog diaFolder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.Button btnOpenFolder;
    }
}