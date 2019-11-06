using Accupay.Payslip;
using Accupay.Utils;
using GlobagilityShared.EmailSender;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace AccupayWindowsService
{
    //  C:\Windows\Microsoft.NET\Framework\v4.0.30319
    //  installUtil E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    //  installUtil /u E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    public partial class EmailService : ServiceBase
    {
        private Timer _emailTimer;
        private EmailSender _emailSender;

        private const string PayslipsFolderName = "Payslips";
        private const string LogsFolderName = "Logs";
        private readonly string PayslipsFolder;

        public EmailService()
        {
            InitializeComponent();

            _emailTimer = new Timer();
            _emailSender = new EmailSender(new EmailConfig());

            PayslipsFolder = Path.Combine(
                                Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,
                                PayslipsFolderName);
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service has started.");
            //Interval in miliseconds
            _emailTimer.Elapsed += new ElapsedEventHandler(OnElapsedTimeEmail);
            _emailTimer.Interval = 30000;
            _emailTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service has stopped.");
        }

        private void OnElapsedTimeEmail(object source, ElapsedEventArgs e)
        {
            try
            {
                WriteToFile("--OnElapsedTimeEmail--");

                var currentPayPeriod = new PayPeriod
                {
                    RowID = 620,
                    PayFromDate = new DateTime(2019, 10, 1),
                    PayToDate = new DateTime(2019, 10, 15)
                };

                var nextPayPeriod = new PayPeriod
                {
                    RowID = 621,
                    PayFromDate = new DateTime(2019, 10, 16),
                    PayToDate = new DateTime(2019, 10, 31)
                };

                var employeeId = 3;
                var organizationId = 2;

                var employeeIds = new int[] { employeeId };
                var employeeLog = $"[employeeId: {employeeId}]";
                var errorTitle = $"[Error] {employeeLog}";

                var payslipCreator = new PayslipCreator(currentPayPeriod, isActual: 0)
                                            .CreateReportDocument(organizationId, nextPayPeriod, employeeIds);

                var employee = payslipCreator.GetFirstEmployee();

                if (employee == null)
                {
                    WriteToFile($"{errorTitle} Cannot fetch employee from datatable. [organizationId: {organizationId}, payperiodId: {currentPayPeriod?.RowID}]");
                    return;
                }

                string saveFolderPath = GetOrCreateDirectory(PayslipsFolderName);
                var employeeNumber = employee["COL1"] == null ? "" : employee["COL1"].ToString();
                var fileName = $"Payslip-{currentPayPeriod.PayToDate.ToString("yyyy-MM-dd")}-{employeeNumber}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
                var birthDate = ObjectUtils.ToNullableDateTime(employee["Birthdate"]);

                if (employee == null)
                {
                    string birthDateString = employee["Birthdate"] == null ? "<null>" : employee["Birthdate"].ToString();
                    WriteToFile($"{errorTitle} Cannot parse. [birthdate: {birthDateString}]");
                    return;
                }

                string password = birthDate.Value.ToString("MMddyyyy");

                payslipCreator.GeneratePDF(saveFolderPath, fileName)
                                .AddPdfPassword(password);

                var pdfFile = payslipCreator.GetPDF();

                var cutoffDate = nextPayPeriod.PayToDate.ToString("MMMM d, yyyy");

                if (employee["EmailAddress"] == null)
                {
                    WriteToFile($"{errorTitle} Email address is null.");
                    return;
                }

                var sendTo = employee["EmailAddress"].ToString();
                if (string.IsNullOrWhiteSpace(sendTo))
                {
                    WriteToFile($"{errorTitle} Email address is empty.");
                    return;
                }

                var subject = $"Payslip for {cutoffDate}";

                var body = $"Please see attached payslip for {cutoffDate}. " +
                    $"\n\n\n" +
                    $"Kindly contact the Human Resources Dept. at 571-2000 local 102 or e-mail at hrd@cinema2000.com.ph for any inquiries or corrections regarding your salary." +
                    $"\n\n" +
                    $"Thank you," +
                    $"\n" +
                    $"HRD";

                var attachments = new string[] { pdfFile };

                WriteToFile($"{employeeLog} Sending...");

                _emailSender.SendEmail(sendTo, subject, body, attachments);

                WriteToFile($"{employeeLog} Email Sent! [Email address: {sendTo}]");
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
            }
        }

        private void WriteToFile(string Message)
        {
            string path = GetOrCreateDirectory(LogsFolderName);

            string fileName = $"ServiceLog_{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.txt";

            string filepath = Path.Combine(path, fileName);
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine($"[{DateTime.Now}] {Message}");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine($"[{DateTime.Now}] {Message}");
                }
            }
        }

        private string GetOrCreateDirectory(string folderName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}