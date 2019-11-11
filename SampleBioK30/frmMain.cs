using AccupayFingerPrintApp.Repo;
using BioMetrixCore;
using Microsoft.Win32;
using SampleBioK30.Info;
using SampleBioK30.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleBioK30
{
    public partial class frmMain : Form
    {
        // For The Device manipulation
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        private bool isDeviceConnected = false;

        public bool IsDeviceConnected
        {
            get { return isDeviceConnected; }
            set
            {
                isDeviceConnected = value;
                if (isDeviceConnected)
                {
                    ShowStatusBar("The device is connected !!", true);
                    btnConnect.Text = "Disconnect";
                    connectToolStripMenuItem.Text = "Disconnect";
                    ToggleControls(true);
                }
                else
                {
                    ShowStatusBar("The device is diconnected !!", true);
                    objZkeeper.Disconnect();
                    btnConnect.Text = "Connect";
                    connectToolStripMenuItem.Text = "Connect";
                    ToggleControls(false);
                }
            }
        }


        private void ToggleControls(bool value)
        {
            btnBeep.Enabled = value;
            btnDownloadFingerPrint.Enabled = value;
            btnPullData.Enabled = value;
            btnPowerOff.Enabled = value;
            btnRestartDevice.Enabled = value;
            btnGetDeviceTime.Enabled = value;
            btnEnableDevice.Enabled = value;
            btnDisableDevice.Enabled = value;
            btnGetAllUserID.Enabled = value;
            btnUploadUserInfo.Enabled = value;
            btnSaveConfig.Enabled = !value;
            tbxMachineNumber.Enabled = !value;
            tbxPort.Enabled = !value;
            tbxDeviceIP.Enabled = !value;
            cbRunOnStartUp.Enabled = !value;

        }

        public frmMain()
        {
            InitializeComponent();
            ToggleControls(false);
            ShowStatusBar(string.Empty, true);
            DisplayEmpty();
            this.frmInit();
        }


        /// <summary>
        /// Your Events will reach here if implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="actionType"></param>
        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        ShowStatusBar("The device is switched off", true);
                        DisplayEmpty();
                        btnConnect.Text = "Connect";
                        ToggleControls(false);
                        break;
                    }

                default:
                    break;
            }

        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.Connect();
        }


        public void ShowStatusBar(string message, bool type)
        {

           setLblStatus(message, type);
           if (!this.Visible)
            {
                setNotificationStatus(message, type);
            }
            
        }

        private void setNotificationStatus(string message, bool type)
        {
            if (String.IsNullOrEmpty(message))
            {
                return;
            }
            this.niAccupayFP.BalloonTipTitle = "Accupay Finger Print App Notification";
            this.niAccupayFP.BalloonTipText = message;

            if (type)
            {
                this.niAccupayFP.BalloonTipIcon = ToolTipIcon.Info;
            } else
            {
                this.niAccupayFP.BalloonTipIcon = ToolTipIcon.Error;
            }
            this.niAccupayFP.ShowBalloonTip(2000);
        }

        private void setLblStatus(string message, bool type)
        {
            if (message.Trim() == string.Empty)
            {
                lblStatus.Visible = false;
                return;
            }

            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.White;

            if (type)
                lblStatus.BackColor = Color.FromArgb(79, 208, 154);
            else
                lblStatus.BackColor = Color.FromArgb(230, 112, 134);
        }

        private void btnPingDevice_Click(object sender, EventArgs e)
        {
            UniversalStatic.GetConfigurationInfo();
            PingDevice();
        }

        private void PingDevice()
        {
            var machineLogs = MachineInfoRepo.GetMachineLogs();
            ShowStatusBar(string.Empty, true);

            string ipAddress = tbxDeviceIP.Text.Trim();

            bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
            if (!isValidIpA)
                throw new Exception("The Device IP is invalid !!");

            isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
            if (isValidIpA)
                ShowStatusBar("The device is active", true);
            else
                ShowStatusBar("Could not read any response", false);
        }

        private void btnGetAllUserID_Click(object sender, EventArgs e)
        {
            try
            {
                ICollection<UserIDInfo> lstUserIDInfo = manipulator.GetAllUserID(objZkeeper, int.Parse(tbxMachineNumber.Text.Trim()));

                if (lstUserIDInfo != null && lstUserIDInfo.Count > 0)
                {
                    BindToGridView(lstUserIDInfo);
                    ShowStatusBar(lstUserIDInfo.Count + " records found !!", true);
                }
                else
                {
                    DisplayEmpty();
                    DisplayListOutput("No records found");
                }

            }
            catch (Exception ex)
            {
                DisplayListOutput(ex.Message);
            }

        }

        private void btnBeep_Click(object sender, EventArgs e)
        {
            objZkeeper.Beep(100);
        }

        private void btnDownloadFingerPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ShowStatusBar(string.Empty, true);

                ICollection<UserInfo> lstFingerPrintTemplates = manipulator.GetAllUserInfo(objZkeeper, int.Parse(tbxMachineNumber.Text.Trim()));
                if (lstFingerPrintTemplates != null && lstFingerPrintTemplates.Count > 0)
                {
                    BindToGridView(lstFingerPrintTemplates);
                    ShowStatusBar(lstFingerPrintTemplates.Count + " records found !!", true);
                }
                else
                    DisplayListOutput("No records found");
            }
            catch (Exception ex)
            {
                DisplayListOutput(ex.Message);
            }

        }


        private void btnPullData_Click(object sender, EventArgs e)
        {
            try
            {
                ShowStatusBar(string.Empty, true);
                ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, int.Parse(tbxMachineNumber.Text.Trim()), dtpFromLog.Value, dtpToLog.Value);

                if (lstMachineInfo != null && lstMachineInfo.Count > 0)
                {
                    BindToGridView(lstMachineInfo);
                    ShowStatusBar(lstMachineInfo.Count + " records found !!", true);
                }
                else
                    DisplayListOutput("No records found");
            }
            catch (Exception ex)
            {
                DisplayListOutput(ex.Message);
            }

        }


        private void ClearGrid()
        {
            if (dgvRecords.Controls.Count > 2)
            { dgvRecords.Controls.RemoveAt(2); }


            dgvRecords.DataSource = null;
            dgvRecords.Controls.Clear();
            dgvRecords.Rows.Clear();
            dgvRecords.Columns.Clear();
        }
        private void BindToGridView(object list)
        {
            ClearGrid();
            dgvRecords.DataSource = list;
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UniversalStatic.ChangeGridProperties(dgvRecords);
        }



        private void DisplayListOutput(string message)
        {
            if (dgvRecords.Controls.Count > 2)
            { dgvRecords.Controls.RemoveAt(2); }

            ShowStatusBar(message, false);
        }

        private void DisplayEmpty()
        {
            ClearGrid();
            dgvRecords.Controls.Add(new DataEmpty());
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        { UniversalStatic.DrawLineInFooter(pnlHeader, Color.FromArgb(204, 204, 204), 2); }



        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var resultDia = DialogResult.None;
            resultDia = MessageBox.Show("Do you wish to Power Off the Device ??", "Power Off Device", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultDia == DialogResult.Yes)
            {
                bool deviceOff = objZkeeper.PowerOffDevice(int.Parse(tbxMachineNumber.Text.Trim()));

            }

            this.Cursor = Cursors.Default;
        }

        private void btnRestartDevice_Click(object sender, EventArgs e)
        {

            DialogResult rslt = MessageBox.Show("Do you wish to restart the device now ??", "Restart Device", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rslt == DialogResult.Yes)
            {
                if (objZkeeper.RestartDevice(int.Parse(tbxMachineNumber.Text.Trim())))
                    ShowStatusBar("The device is being restarted, Please wait...", true);
                else
                    ShowStatusBar("Operation failed,please try again", false);
            }

        }

        private void btnGetDeviceTime_Click(object sender, EventArgs e)
        {
            GetDeviceTime();
        }

        private void GetDeviceTime()
        {
            int machineNumber = int.Parse(tbxMachineNumber.Text.Trim());
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;

            bool result = objZkeeper.GetDeviceTime(machineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);

            string deviceTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();
            List<DeviceTimeInfo> lstDeviceInfo = new List<DeviceTimeInfo>();
            lstDeviceInfo.Add(new DeviceTimeInfo() { DeviceTime = deviceTime });
            BindToGridView(lstDeviceInfo);
        }

        private void btnEnableDevice_Click(object sender, EventArgs e)
        {
            // This is of no use since i implemented zkemKeeper the other way
            bool deviceEnabled = objZkeeper.EnableDevice(int.Parse(tbxMachineNumber.Text.Trim()), true);

        }



        private void btnDisableDevice_Click(object sender, EventArgs e)
        {
            // This is of no use since i implemented zkemKeeper the other way
            bool deviceDisabled = objZkeeper.DisableDeviceWithTimeOut(int.Parse(tbxMachineNumber.Text.Trim()), 3000);
        }

        private void tbxPort_TextChanged(object sender, EventArgs e)
        { UniversalStatic.ValidateInteger(tbxPort); }

        private void tbxMachineNumber_TextChanged(object sender, EventArgs e)
        { UniversalStatic.ValidateInteger(tbxMachineNumber); }

        private void btnUploadUserInfo_Click(object sender, EventArgs e)
        {
            // Add you new UserInfo Here and  uncomment the below code
            //List<UserInfo> lstUserInfo = new List<UserInfo>();
            //manipulator.UploadFTPTemplate(objZkeeper, int.Parse(tbxMachineNumber.Text.Trim()), lstUserInfo);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            HideOnSystemTray();
        }

        private void HideOnSystemTray()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            niAccupayFP.Visible = true;
        }

        private void niAccupayFP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.CenterToScreen();
            niAccupayFP.Visible = false;
            
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Connect();   
        }

        private void pingDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PingDevice();
        }

        private void getDeviceTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDeviceTime();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check device is connected and disconnect it first
            if (IsDeviceConnected)
            {
                IsDeviceConnected = false;
            }
            niAccupayFP.Visible = false;
            System.Environment.Exit(1);
        }

        private void Connect()
        {
            try
            {
                
                this.Cursor = Cursors.WaitCursor;
                ShowStatusBar(string.Empty, true);

                if (IsDeviceConnected)
                {
                    
                    IsDeviceConnected = false;
                    this.Cursor = Cursors.Default;

                    return;
                }
                // Get and Check if configuration is present
                var config = UniversalStatic.GetConfigurationInfo();
                if (config is null)
                {
                    config = new ConfigurationInfo()
                    {
                        IpAddress = tbxDeviceIP.Text.Trim(),
                        Port = tbxPort.Text.Trim(),
                        MachineNumber = tbxMachineNumber.Text.Trim(),
                        RunOnStartUp = cbRunOnStartUp.Checked ? "1" : "0"
                        
                    };
                } else
                {
                    tbxDeviceIP.Text = config.IpAddress;
                    tbxPort.Text = config.Port;
                    tbxMachineNumber.Text = config.MachineNumber;
                    cbRunOnStartUp.Checked = config.RunOnStartUp == "1" ? true : false;
                }

                if (config.IpAddress == string.Empty || config.Port == string.Empty)
                    throw new Exception("The Device IP Address and Port is mandatory !!");

                int portNumber = 4370;
                if (!int.TryParse(config.Port, out portNumber))
                    throw new Exception("Not a valid port number");

                bool isValidIpA = UniversalStatic.ValidateIP(config.IpAddress);
                if (!isValidIpA)
                    throw new Exception("The Device IP is invalid !!");

                isValidIpA = UniversalStatic.PingTheDevice(config.IpAddress);
                if (!isValidIpA)
                    throw new Exception("The device at " + config.IpAddress + ":" + config.Port + " did not respond!!");

                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                IsDeviceConnected = objZkeeper.Connect_Net(config.IpAddress, portNumber);

                if (IsDeviceConnected)
                {
                    string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, int.Parse(config.MachineNumber));
                    lblDeviceInfo.Text = deviceInfo;
                }

                
            }
            catch (Exception ex)
            {
                ShowStatusBar(ex.Message, false);
            }
            this.Cursor = Cursors.Default;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            timer1.Interval = 5000;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            var ipAddress = tbxDeviceIP.Text.Trim();
            var port = tbxPort.Text.Trim();
            var machineNumber = tbxMachineNumber.Text.Trim();
            if (CheckConfigInputs())
            {
                var config = new ConfigurationInfo()
                {
                    IpAddress = ipAddress,
                    Port = port,
                    MachineNumber = machineNumber,
                    RunOnStartUp = cbRunOnStartUp.Checked ? "1" : "0"
                }; 
                try
                {
                    UniversalStatic.SaveConfigurations(config);
                    SetStartUpRegistry();
                }
                catch (Exception ex)
                {
                    ShowStatusBar(ex.Message, false);
                }
            }
        }

        private bool CheckConfigInputs()
        {
            errProvConfig.Clear();
            if (string.IsNullOrWhiteSpace(tbxDeviceIP.Text))
            {
                errProvConfig.SetError(tbxDeviceIP, "IP Address Required");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(tbxPort.Text))
            {
                errProvConfig.SetError(tbxPort, "Port Required");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(tbxMachineNumber.Text))
            {
                errProvConfig.SetError(tbxMachineNumber, "Machine Number Required");
                return false;
            }
            return true;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetStartUpRegistry()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (cbRunOnStartUp.Checked)
                rk.SetValue("AccuPay Finger Print App", Application.ExecutablePath);
            else
                rk.DeleteValue("AccuPay Finger Print App", false);
        }

        private void frmInit()
        {
            Connect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, 2, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                if (lstMachineInfo != null && lstMachineInfo.Count > 0)
                {
                    MachineInfoRepo.InsertMachineLogs(lstMachineInfo.ToList());
                }
            }
            catch (Exception ex)
            {

                DisplayListOutput(ex.Message);
            }
        }
    }
}
