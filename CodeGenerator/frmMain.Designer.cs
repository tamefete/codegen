namespace CodeGenerator
{
    partial class frmMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbDatabase = new System.Windows.Forms.ToolStripComboBox();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAll = new System.Windows.Forms.ToolStripButton();
            this.btnCreateVSP = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeTables = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstClasses = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.btnGenClass = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbDatabase,
            this.btnConnect,
            this.btnSave,
            this.btnSaveAll,
            this.btnCreateVSP,
            this.toolStripSeparator1,
            this.btnHelp,
            this.btnAbout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1113, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel1.Text = "Chọn database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.Items.AddRange(new object[] {
            "MS SQL Server",
            "MySQL",
            "Oracle"});
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(121, 25);
            this.cmbDatabase.ToolTipText = "Chọn cơ sở dữ liệu để kết nối";
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(23, 22);
            this.btnConnect.Text = "toolStripButton1";
            this.btnConnect.ToolTipText = "Kết nối cơ sở dữ liệu";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.ToolTipText = "Lưu lớp đang chọn";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAll.Image")));
            this.btnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(23, 22);
            this.btnSaveAll.Text = "toolStripButton1";
            this.btnSaveAll.ToolTipText = "Lưu tất cả các lớp";
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnCreateVSP
            // 
            this.btnCreateVSP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateVSP.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateVSP.Image")));
            this.btnCreateVSP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateVSP.Name = "btnCreateVSP";
            this.btnCreateVSP.Size = new System.Drawing.Size(23, 22);
            this.btnCreateVSP.Text = "toolStripButton1";
            this.btnCreateVSP.ToolTipText = "Tạo Visual Studio Project";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "Trợ giúp: Hướng dẫn sử dụng";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnAbout.Image")));
            this.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(23, 22);
            this.btnAbout.Text = "toolStripButton1";
            this.btnAbout.ToolTipText = "Thông tin tác giả";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeTables);
            this.groupBox1.Location = new System.Drawing.Point(6, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 476);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Tables";
            // 
            // treeTables
            // 
            this.treeTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeTables.Location = new System.Drawing.Point(6, 26);
            this.treeTables.Name = "treeTables";
            this.treeTables.Size = new System.Drawing.Size(159, 444);
            this.treeTables.TabIndex = 2;
            this.toolTip1.SetToolTip(this.treeTables, "Danh sách các tables trong cơ sở dữ liệu");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstClasses);
            this.groupBox2.Location = new System.Drawing.Point(183, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 476);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tạo Lớp";
            // 
            // lstClasses
            // 
            this.lstClasses.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClasses.FormattingEnabled = true;
            this.lstClasses.ItemHeight = 15;
            this.lstClasses.Location = new System.Drawing.Point(9, 26);
            this.lstClasses.Name = "lstClasses";
            this.lstClasses.Size = new System.Drawing.Size(175, 439);
            this.lstClasses.TabIndex = 4;
            this.toolTip1.SetToolTip(this.lstClasses, "Danh sách các lớp sẽ được tạo");
            this.lstClasses.SelectedIndexChanged += new System.EventHandler(this.lstClasses_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Namespace";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(76, 13);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(207, 20);
            this.txtNamespace.TabIndex = 1;
            this.txtNamespace.Text = "Gencode";
            this.toolTip1.SetToolTip(this.txtNamespace, "Namespace cho tất cả các lớp. Mặc định là Gencode");
            // 
            // btnGenClass
            // 
            this.btnGenClass.Location = new System.Drawing.Point(289, 11);
            this.btnGenClass.Name = "btnGenClass";
            this.btnGenClass.Size = new System.Drawing.Size(75, 23);
            this.btnGenClass.TabIndex = 2;
            this.btnGenClass.Text = "Sinh Lớp";
            this.toolTip1.SetToolTip(this.btnGenClass, "Click button Sinh Lớp để xem các lớp sẽ được tạo trong ứng dụng");
            this.btnGenClass.UseVisualStyleBackColor = true;
            this.btnGenClass.Click += new System.EventHandler(this.txtGenClass_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Location = new System.Drawing.Point(0, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(383, 545);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.btnGenClass);
            this.groupBox4.Controls.Add(this.txtNamespace);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(377, 47);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.lblFileName);
            this.groupBox5.Controls.Add(this.txtSource);
            this.groupBox5.Location = new System.Drawing.Point(382, 25);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(730, 545);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nội dung file: ";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(86, 16);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(16, 13);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "   ";
            // 
            // txtSource
            // 
            this.txtSource.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.Location = new System.Drawing.Point(6, 32);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(718, 507);
            this.txtSource.TabIndex = 0;
            this.txtSource.Text = "";
            this.toolTip1.SetToolTip(this.txtSource, "Xem nội dung của lớp đang chọn");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusLabel1,
            this.statusProgressBar1,
            this.statusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 573);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1113, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(13, 17);
            this.statusLabel.Text = "  ";
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(90, 17);
            this.statusLabel1.Text = "Code Generator";
            this.statusLabel1.Visible = false;
            // 
            // statusProgressBar1
            // 
            this.statusProgressBar1.Name = "statusProgressBar1";
            this.statusProgressBar1.Size = new System.Drawing.Size(300, 16);
            this.statusProgressBar1.Visible = false;
            // 
            // statusLabel2
            // 
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(35, 17);
            this.statusLabel2.Text = "100%";
            this.statusLabel2.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Thông tin";
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 595);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3 Layer Code Generator";
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbDatabase;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnSaveAll;
        private System.Windows.Forms.ToolStripButton btnCreateVSP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstClasses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Button btnGenClass;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox txtSource;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnAbout;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TreeView treeTables;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripProgressBar statusProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

