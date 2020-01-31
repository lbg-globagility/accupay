using Accupay.Data.Repositories;
using Accupay.Payslip;
using GlobagilityShared.EmailSender;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Linq;
using System.Data;

namespace AccupayWindowsService
{
    // Steps to install the windows service to client
    // 1. Build the project
    // 2. Compressed the files in the AccupayWindowsService\bin\Debug (except for the Payslip and Logs folders)
    // 3. Paste the zip file to clients server
    // 4. Open command prompt in server as Administrator and type `cd C:\Windows\Microsoft.NET\Framework\v4.0.30319`
    // 5. Before installing the service, make sure to stop the service if it is running and uninstall it by typing `installUtil /u {path to .exe}\AccupayWindowsService.exe` ex. installUtil /u C:\AccupayWindowsService\AccupayWindowsService.exe
    // 6. Then install the service installUtil {path to .exe}\AccupayWindowsService.exe` ex. installUtil C:\AccupayWindowsService\AccupayWindowsService.exe
    // 7. Make sure the service is started.

    //  C:\Windows\Microsoft.NET\Framework\v4.0.30319
    //  installUtil /u E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    //  installUtil E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    public partial class EmailService : ServiceBase
    {
        private Timer _emailTimer;
        private EmailSender _emailSender;

        private const string PayslipsFolderName = "Payslips";
        private const string LogsFolderName = "Logs";

        public EmailService()
        {
            InitializeComponent();

            _emailTimer = new Timer();
            _emailSender = new EmailSender(new EmailConfig());
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

                paystubEmail = repo.FirstOnQueueWithPaystubDetails();
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

                var validationErrorMessage = GetErrorMessage(errorTitle,
                                                            currentPayPeriod,
                                                            nextPayPeriod,
                                                            employeeId,
                                                            organizationId);

                if (string.IsNullOrWhiteSpace(validationErrorMessage) == false)
                {
                    WriteToFile(validationErrorMessage);
                    paystubEmail.SetStatusToFailed(validationErrorMessage);
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
                    validationErrorMessage = $"{errorTitle} Cannot find employee in the payslip report datatable.";
                    WriteToFile(validationErrorMessage);
                    paystubEmail.SetStatusToFailed(validationErrorMessage);
                    return;
                }

                string saveFolderPath = GetOrCreateDirectory(PayslipsFolderName);
                var employeeNumber = employee.EmployeeNo ?? "";
                var fileName = $"Payslip-{nextPayPeriod.PayToDate.ToString("yyyy-MM-dd")}-{employeeNumber}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
                var birthDate = employee.BirthDate;

                string password = birthDate.ToString("MMddyyyy");

                payslipCreator.GeneratePDF(saveFolderPath, fileName)
                                .AddPdfPassword(password);

                var pdfFile = payslipCreator.GetPDF();

                var cutoffDate = nextPayPeriod.PayToDate.ToString("MMMM d, yyyy");

                if (string.IsNullOrWhiteSpace(employee.EmailAddress))
                {
                    validationErrorMessage = $"{errorTitle} Email address is null or empty.";
                    WriteToFile(validationErrorMessage);
                    paystubEmail.SetStatusToFailed(validationErrorMessage);
                    return;
                }

                var subject = $"Payslip for {cutoffDate}";

                var body = $"Please see attached payslip for {cutoffDate}. " +
                    $"\n\n" +
                    $"Your payslip is password-protected to ensure the security of your account. The default password is your date of birth with the following format mmddyyyy. For example, if your birthday is February 2, 1988, your password is \"02021988\"" +
                    $"\n\n" +
                    $"Kindly contact the Human Resources Dept. at 571-2000 local 102 or e-mail at hrd@cinema2000.com.ph for any inquiries or corrections regarding your salary." +
                    $"\n\n" +
                    $"Thank you," +
                    $"\n" +
                    $"HRD";

                var attachments = new string[] { pdfFile };

                WriteToFile($"{paystubEmailLog} Sending...");

                _emailSender.SendEmail(employee.EmailAddress, subject, body, attachments);

                WriteToFile($"{paystubEmailLog} Email Sent! [Email address: {employee.EmailAddress}]");

                paystubEmail.Finish(fileName, employee.EmailAddress);
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                WriteToFile(ex.StackTrace);

                if (paystubEmail != null)
                {
                    string validationErrorMessage = $"[System Error] {ex.Message}";
                    paystubEmail.SetStatusToFailed(validationErrorMessage);
                }
            }
        }

        private string GetErrorMessage(string errorTitle,
                            Accupay.Data.Entities.PayPeriod currentPayPeriod,
                            Accupay.Data.Entities.PayPeriod nextPayPeriod,
                            int? employeeId,
                            int? organizationId)
        {
            string errorMessage = string.Empty;

            if (currentPayPeriod == null)
            {
                errorMessage = $"{errorTitle} currentPayPeriod is null.";
            }
            if (nextPayPeriod == null)
            {
                errorMessage = $"{errorTitle} nextPayPeriod is null.";
            }
            if (employeeId == null)
            {
                errorMessage = $"{errorTitle} employeeId is null.";
            }
            if (organizationId == null)
            {
                errorMessage = $"{errorTitle} organizationId is null.";
            }

            return errorMessage;
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