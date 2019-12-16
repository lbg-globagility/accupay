namespace SampleBioK30
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.cbRunOnStartUp = new System.Windows.Forms.CheckBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxDeviceIP = new System.Windows.Forms.TextBox();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.btnPingDevice = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxMachineNumber = new System.Windows.Forms.TextBox();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDownloadFingerPrint = new System.Windows.Forms.Button();
            this.btnPullData = new System.Windows.Forms.Button();
            this.btnGetAllUserID = new System.Windows.Forms.Button();
            this.btnGetDeviceTime = new System.Windows.Forms.Button();
            this.btnBeep = new System.Windows.Forms.Button();
            this.btnEnableDevice = new System.Windows.Forms.Button();
            this.btnDisableDevice = new System.Windows.Forms.Button();
            this.btnRestartDevice = new System.Windows.Forms.Button();
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.btnUploadUserInfo = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.niAccupayFP = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pingDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDeviceTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errProvConfig = new System.Windows.Forms.ErrorProvider(this.components);
            this.dtpFromLog = new System.Windows.Forms.DateTimePicker();
            this.dtpToLog = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.cmsNotifyIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.pnlHeader.Controls.Add(this.cbRunOnStartUp);
            this.pnlHeader.Controls.Add(this.btnSaveConfig);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.tbxDeviceIP);
            this.pnlHeader.Controls.Add(this.tbxPort);
            this.pnlHeader.Controls.Add(this.btnPingDevice);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Controls.Add(this.btnConnect);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.tbxMachineNumber);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(815, 37);
            this.pnlHeader.TabIndex = 0;
            // 
            // cbRunOnStartUp
            // 
            this.cbRunOnStartUp.AutoSize = true;
            this.cbRunOnStartUp.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbRunOnStartUp.Location = new System.Drawing.Point(426, 8);
            this.cbRunOnStartUp.Name = "cbRunOnStartUp";
            this.cbRunOnStartUp.Size = new System.Drawing.Size(100, 17);
            this.cbRunOnStartUp.TabIndex = 18;
            this.cbRunOnStartUp.Text = "Run on StartUp";
            this.cbRunOnStartUp.UseVisualStyleBackColor = true;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveConfig.Location = new System.Drawing.Point(555, 7);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 17;
            this.btnSaveConfig.Text = "&Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Device IP";
            // 
            // tbxDeviceIP
            // 
            this.tbxDeviceIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDeviceIP.Location = new System.Drawing.Point(69, 7);
            this.tbxDeviceIP.Name = "tbxDeviceIP";
            this.tbxDeviceIP.Size = new System.Drawing.Size(99, 20);
            this.tbxDeviceIP.TabIndex = 1;
            this.tbxDeviceIP.Text = "192.168.1.5";
            // 
            // tbxPort
            // 
            this.tbxPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxPort.Location = new System.Drawing.Point(206, 7);
            this.tbxPort.MaxLength = 6;
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(56, 20);
            this.tbxPort.TabIndex = 2;
            this.tbxPort.Text = "4370";
            this.tbxPort.TextChanged += new System.EventHandler(this.tbxPort_TextChanged);
            // 
            // btnPingDevice
            // 
            this.btnPingDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPingDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPingDevice.Location = new System.Drawing.Point(724, 7);
            this.btnPingDevice.Name = "btnPingDevice";
            this.btnPingDevice.Size = new System.Drawing.Size(75, 23);
            this.btnPingDevice.TabIndex = 5;
            this.btnPingDevice.Text = "Ping Device";
            this.btnPingDevice.UseVisualStyleBackColor = true;
            this.btnPingDevice.Click += new System.EventHandler(this.btnPingDevice_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Port";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(631, 7);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(93, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Machine Number";
            // 
            // tbxMachineNumber
            // 
            this.tbxMachineNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMachineNumber.Location = new System.Drawing.Point(366, 7);
            this.tbxMachineNumber.MaxLength = 3;
            this.tbxMachineNumber.Name = "tbxMachineNumber";
            this.tbxMachineNumber.Size = new System.Drawing.Size(37, 20);
            this.tbxMachineNumber.TabIndex = 3;
            this.tbxMachineNumber.Text = "2";
            this.tbxMachineNumber.TextChanged += new System.EventHandler(this.tbxMachineNumber_TextChanged);
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceInfo.Location = new System.Drawing.Point(7, 49);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(414, 19);
            this.lblDeviceInfo.TabIndex = 893;
            this.lblDeviceInfo.Text = "Device Info : --";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dgvRecords);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(12, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 414);
            this.panel1.TabIndex = 894;
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.AllowUserToOrderColumns = true;
            this.dgvRecords.AllowUserToResizeColumns = false;
            this.dgvRecords.AllowUserToResizeRows = false;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(0, 54);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.Size = new System.Drawing.Size(791, 360);
            this.dgvRecords.TabIndex = 883;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnDownloadFingerPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnPullData);
            this.flowLayoutPanel1.Controls.Add(this.btnGetAllUserID);
            this.flowLayoutPanel1.Controls.Add(this.btnGetDeviceTime);
            this.flowLayoutPanel1.Controls.Add(this.btnBeep);
            this.flowLayoutPanel1.Controls.Add(this.btnEnableDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnDisableDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnRestartDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnPowerOff);
            this.flowLayoutPanel1.Controls.Add(this.btnUploadUserInfo);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(791, 54);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnDownloadFingerPrint
            // 
            this.btnDownloadFingerPrint.Location = new System.Drawing.Point(3, 3);
            this.btnDownloadFingerPrint.Name = "btnDownloadFingerPrint";
            this.btnDownloadFingerPrint.Size = new System.Drawing.Size(112, 48);
            this.btnDownloadFingerPrint.TabIndex = 6;
            this.btnDownloadFingerPrint.Text = "Get All User Info";
            this.btnDownloadFingerPrint.UseVisualStyleBackColor = true;
            this.btnDownloadFingerPrint.Click += new System.EventHandler(this.btnDownloadFingerPrint_Click);
            // 
            // btnPullData
            // 
            this.btnPullData.Location = new System.Drawing.Point(121, 3);
            this.btnPullData.Name = "btnPullData";
            this.btnPullData.Size = new System.Drawing.Size(80, 48);
            this.btnPullData.TabIndex = 7;
            this.btnPullData.Text = "Get Log Data";
            this.btnPullData.UseVisualStyleBackColor = true;
            this.btnPullData.Click += new System.EventHandler(this.btnPullData_Click);
            // 
            // btnGetAllUserID
            // 
            this.btnGetAllUserID.Location = new System.Drawing.Point(207, 3);
            this.btnGetAllUserID.Name = "btnGetAllUserID";
            this.btnGetAllUserID.Size = new System.Drawing.Size(72, 48);
            this.btnGetAllUserID.TabIndex = 8;
            this.btnGetAllUserID.Text = "Get All User ID";
            this.btnGetAllUserID.UseVisualStyleBackColor = true;
            this.btnGetAllUserID.Click += new System.EventHandler(this.btnGetAllUserID_Click);
            // 
            // btnGetDeviceTime
            // 
            this.btnGetDeviceTime.Location = new System.Drawing.Point(285, 3);
            this.btnGetDeviceTime.Name = "btnGetDeviceTime";
            this.btnGetDeviceTime.Size = new System.Drawing.Size(78, 48);
            this.btnGetDeviceTime.TabIndex = 9;
            this.btnGetDeviceTime.Text = "Get Device Time";
            this.btnGetDeviceTime.UseVisualStyleBackColor = true;
            this.btnGetDeviceTime.Click += new System.EventHandler(this.btnGetDeviceTime_Click);
            // 
            // btnBeep
            // 
            this.btnBeep.Location = new System.Drawing.Point(369, 3);
            this.btnBeep.Name = "btnBeep";
            this.btnBeep.Size = new System.Drawing.Size(59, 48);
            this.btnBeep.TabIndex = 10;
            this.btnBeep.Text = "Beep";
            this.btnBeep.UseVisualStyleBackColor = true;
            this.btnBeep.Click += new System.EventHandler(this.btnBeep_Click);
            // 
            // btnEnableDevice
            // 
            this.btnEnableDevice.Location = new System.Drawing.Point(434, 3);
            this.btnEnableDevice.Name = "btnEnableDevice";
            this.btnEnableDevice.Size = new System.Drawing.Size(65, 48);
            this.btnEnableDevice.TabIndex = 11;
            this.btnEnableDevice.Text = "Enable Device";
            this.btnEnableDevice.UseVisualStyleBackColor = true;
            this.btnEnableDevice.Click += new System.EventHandler(this.btnEnableDevice_Click);
            // 
            // btnDisableDevice
            // 
            this.btnDisableDevice.Location = new System.Drawing.Point(505, 3);
            this.btnDisableDevice.Name = "btnDisableDevice";
            this.btnDisableDevice.Size = new System.Drawing.Size(65, 48);
            this.btnDisableDevice.TabIndex = 12;
            this.btnDisableDevice.Text = "Disable Device";
            this.btnDisableDevice.UseVisualStyleBackColor = true;
            this.btnDisableDevice.Click += new System.EventHandler(this.btnDisableDevice_Click);
            // 
            // btnRestartDevice
            // 
            this.btnRestartDevice.Location = new System.Drawing.Point(576, 3);
            this.btnRestartDevice.Name = "btnRestartDevice";
            this.btnRestartDevice.Size = new System.Drawing.Size(65, 48);
            this.btnRestartDevice.TabIndex = 13;
            this.btnRestartDevice.Text = "Restart Device";
            this.btnRestartDevice.UseVisualStyleBackColor = true;
            this.btnRestartDevice.Click += new System.EventHandler(this.btnRestartDevice_Click);
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.Location = new System.Drawing.Point(647, 3);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(65, 48);
            this.btnPowerOff.TabIndex = 14;
            this.btnPowerOff.Text = "Power Off";
            this.btnPowerOff.UseVisualStyleBackColor = true;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // btnUploadUserInfo
            // 
            this.btnUploadUserInfo.Location = new System.Drawing.Point(718, 3);
            this.btnUploadUserInfo.Name = "btnUploadUserInfo";
            this.btnUploadUserInfo.Size = new System.Drawing.Size(65, 48);
            this.btnUploadUserInfo.TabIndex = 15;
            this.btnUploadUserInfo.Text = "Upload User Info";
            this.btnUploadUserInfo.UseVisualStyleBackColor = true;
            this.btnUploadUserInfo.Click += new System.EventHandler(this.btnUploadUserInfo_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(0, 483);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblStatus.Size = new System.Drawing.Size(815, 25);
            this.lblStatus.TabIndex = 895;
            this.lblStatus.Text = "label3";
            // 
            // niAccupayFP
            // 
            this.niAccupayFP.ContextMenuStrip = this.cmsNotifyIcon;
            this.niAccupayFP.Icon = ((System.Drawing.Icon)(resources.GetObject("niAccupayFP.Icon")));
            this.niAccupayFP.Text = "accupay finger print application";
            this.niAccupayFP.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niAccupayFP_MouseDoubleClick);
            // 
            // cmsNotifyIcon
            // 
            this.cmsNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.pingDeviceToolStripMenuItem,
            this.getDeviceTimeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.cmsNotifyIcon.Name = "cmsNotifyIcon";
            this.cmsNotifyIcon.Size = new System.Drawing.Size(161, 92);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // pingDeviceToolStripMenuItem
            // 
            this.pingDeviceToolStripMenuItem.Name = "pingDeviceToolStripMenuItem";
            this.pingDeviceToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.pingDeviceToolStripMenuItem.Text = "&Ping Device";
            this.pingDeviceToolStripMenuItem.Click += new System.EventHandler(this.pingDeviceToolStripMenuItem_Click);
            // 
            // getDeviceTimeToolStripMenuItem
            // 
            this.getDeviceTimeToolStripMenuItem.Name = "getDeviceTimeToolStripMenuItem";
            this.getDeviceTimeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.getDeviceTimeToolStripMenuItem.Text = "Get &Device Time";
            this.getDeviceTimeToolStripMenuItem.Click += new System.EventHandler(this.getDeviceTimeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // errProvConfig
            // 
            this.errProvConfig.ContainerControl = this;
            // 
            // dtpFromLog
            // 
            this.dtpFromLog.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dtpFromLog.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromLog.Location = new System.Drawing.Point(481, 45);
            this.dtpFromLog.Name = "dtpFromLog";
            this.dtpFromLog.Size = new System.Drawing.Size(147, 20);
            this.dtpFromLog.TabIndex = 896;
            // 
            // dtpToLog
            // 
            this.dtpToLog.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dtpToLog.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToLog.Location = new System.Drawing.Point(657, 45);
            this.dtpToLog.Name = "dtpToLog";
            this.dtpToLog.Size = new System.Drawing.Size(144, 20);
            this.dtpToLog.TabIndex = 897;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(634, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "to";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 508);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpToLog);
            this.Controls.Add(this.dtpFromLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDeviceInfo);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(615, 425);
            this.Name = "frmMain";
            this.Text = "Accupay Finger Print Connector App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.cmsNotifyIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errProvConfig)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxDeviceIP;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.Button btnPingDevice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxMachineNumber;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnDownloadFingerPrint;
        private System.Windows.Forms.Button btnPullData;
        private System.Windows.Forms.Button btnGetAllUserID;
        private System.Windows.Forms.Button btnGetDeviceTime;
        private System.Windows.Forms.Button btnBeep;
        private System.Windows.Forms.Button btnEnableDevice;
        private System.Windows.Forms.Button btnDisableDevice;
        private System.Windows.Forms.Button btnRestartDevice;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.Button btnUploadUserInfo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.NotifyIcon niAccupayFP;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pingDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDeviceTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.ErrorProvider errProvConfig;
        private System.Windows.Forms.CheckBox cbRunOnStartUp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpToLog;
        private System.Windows.Forms.DateTimePicker dtpFromLog;
        private System.Windows.Forms.Timer timer1;
    }
}

