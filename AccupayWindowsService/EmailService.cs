using Accupay.Data.Repositories;
using Accupay.Payslip;
using Accupay.Utils;
using GlobagilityShared.EmailSender;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Linq;
using System.Data;

namespace AccupayWindowsService
{
    //  C:\Windows\Microsoft.NET\Framework\v4.0.30319
    //  installUtil /u E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    //  installUtil E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
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
            Accupay.Data.Entities.PaystubEmail paystubEmail = null;

            try
            {
                WriteToFile("--OnElapsedTimeEmail--");

                var repo = new PaystubEmailRepository();

                paystubEmail = repo.FirstWithPaystubDetails();
                if (paystubEmail == null)
                {
                    WriteToFile("No queued email.");
                    return;
                }
                else
                {
                    paystubEmail.SetStatusToProcessing();
                }

                var paystubEmailLog = $"[paystubId: {paystubEmail.Paystub?.RowID}]";
                var errorTitle = $"[Error] {paystubEmailLog}";

                var currentPayPeriod = paystubEmail.Paystub?.PayPeriod;
                var nextPayPeriod = currentPayPeriod.NextPayPeriod();
                var employeeId = paystubEmail.Paystub?.Employee?.RowID;
                var employee = paystubEmail.Paystub?.Employee;
                var organizationId = paystubEmail.Paystub?.OrganizationID;

                if (Validate(errorTitle,
                    currentPayPeriod,
                    nextPayPeriod,
                    employeeId,
                    organizationId) == false)
                {
                    paystubEmail.ResetStatus();
                    return;
                }

                var employeeIds = new int[] { employeeId.Value };

                var payslipCreator = new PayslipCreator(currentPayPeriod, isActual: 0)
                                            .CreateReportDocument(organizationId.Value, nextPayPeriod, employeeIds);

                if (payslipCreator.GetPayslipDatatable()
                                    .AsEnumerable()
                                    .Where(x => x.Field<int>("EmployeeRowID") == employeeId.Value)
                                    .Any() == false)
                {
                    paystubEmail.ResetStatus();
                    WriteToFile($"{errorTitle} Cannot find employee in the payslip report datatable.");
                    return;
                }

                string saveFolderPath = GetOrCreateDirectory(PayslipsFolderName);
                var employeeNumber = employee.EmployeeNo ?? "";
                var fileName = $"Payslip-{currentPayPeriod.PayToDate.ToString("yyyy-MM-dd")}-{employeeNumber}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
                var birthDate = employee.BirthDate;

                string password = birthDate.ToString("MMddyyyy");

                payslipCreator.GeneratePDF(saveFolderPath, fileName)
                                .AddPdfPassword(password);

                var pdfFile = payslipCreator.GetPDF();

                var cutoffDate = nextPayPeriod.PayToDate.ToString("MMMM d, yyyy");

                if (string.IsNullOrWhiteSpace(employee.EmailAddress))
                {
                    paystubEmail.ResetStatus();
                    WriteToFile($"{errorTitle} Email address is null or empty.");
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

                WriteToFile($"{paystubEmailLog} Sending...");

                _emailSender.SendEmail(employee.EmailAddress, subject, body, attachments);

                WriteToFile($"{paystubEmailLog} Email Sent! [Email address: {employee.EmailAddress}]");

                paystubEmail.Finish(fileName);
            }
            catch (Exception ex)
            {
                if (paystubEmail != null)
                {
                    paystubEmail.ResetStatus();
                }

                WriteToFile(ex.Message);
                WriteToFile(ex.StackTrace);
            }
        }

        private bool Validate(string errorTitle,
                            Accupay.Data.Entities.PayPeriod currentPayPeriod,
                            Accupay.Data.Entities.PayPeriod nextPayPeriod,
                            int? employeeId,
                            int? organizationId)
        {
            if (currentPayPeriod == null)
            {
                WriteToFile($"{errorTitle} currentPayPeriod is null.");
                return false;
            }
            if (nextPayPeriod == null)
            {
                WriteToFile($"{errorTitle} nextPayPeriod is null.");
                return false;
            }
            if (employeeId == null)
            {
                WriteToFile($"{errorTitle} employeeId is null.");
                return false;
            }
            if (organizationId == null)
            {
                WriteToFile($"{errorTitle} organizationId is null.");
                return false;
            }

            return true;
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