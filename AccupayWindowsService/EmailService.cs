using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.CrystalReports;
using GlobagilityShared.EmailSender;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;
using AccuPay.Data.Services;

namespace AccupayWindowsService
{
    // Steps to install the windows service to client
    // 1. Build the project
    // 2. Compress the files in the AccupayWindowsService\bin\Debug (except for the Payslip and Logs folders)
    // 3. Paste the zip file to clients server
    // 4. Open command prompt in server as Administrator and type `cd C:\Windows\Microsoft.NET\Framework\v4.0.30319`
    // 5. Before installing the service, make sure to stop the service if it is running and uninstall it by typing `installUtil /u {path to .exe}\AccupayWindowsService.exe` ex. installUtil /u C:\AccupayWindowsService\AccupayWindowsService.exe
    // 6. Then install the service installUtil {path to .exe}\AccupayWindowsService.exe` ex. installUtil C:\AccupayWindowsService\AccupayWindowsService.exe
    // 7. Make sure the service is started.

    //  TYPE THE COMMANDS BELOW ON COMMAND PROMPT AS ADMINISTRATOR TO INSTALL (Only use the command if there is already an existing AccupayWindowsService installed.)
    //  cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
    //  installUtil /u E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    //  installUtil E:\Programs\_accupay_from_origin\AccupayWindowsService\bin\Debug\AccupayWindowsService.exe
    //
    // Dont forget to start the service!

    public partial class EmailService : ServiceBase
    {
        private Timer _emailTimer;
        private EmailSender _emailSender;

        private const string PayslipsFolderName = "Payslips";
        private const string LogsFolderName = "Logs";

        private readonly PaystubEmailRepository _paystubEmailRepository;
        private readonly PayslipBuilder _payslipBuilder;
        private readonly SystemOwnerService _systemOwnerService;

        public EmailService(
            PaystubEmailRepository paystubEmailRepository,
            PayslipBuilder payslipBuilder,
            SystemOwnerService systemOwnerService)
        {
            InitializeComponent();

            _emailTimer = new Timer();
            _emailSender = new EmailSender(new EmailConfig());
            _paystubEmailRepository = paystubEmailRepository;
            _payslipBuilder = payslipBuilder;
            _systemOwnerService = systemOwnerService;
        }

        protected override void OnStart(string[] args)
        {
            var appVersion = ConfigurationManager.AppSettings["appVersion"];
            WriteToFile($"Service has started. [version {appVersion}]");
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
            PaystubEmail paystubEmail = null;

            try
            {
                WriteToFile("--OnElapsedTimeEmail--");

                paystubEmail = _paystubEmailRepository.FirstOnQueueWithPaystubDetails();
                if (paystubEmail == null)
                {
                    WriteToFile("No queued email.");
                    return;
                }
                else
                {
                    _paystubEmailRepository.SetStatusToProcessing(paystubEmail.RowID);
                }

                var paystubEmailLog = $"[paystubId: {paystubEmail.Paystub?.RowID}]";
                var errorTitle = $"[Error] {paystubEmailLog}";

                var currentPayPeriod = paystubEmail.Paystub?.PayPeriod;
                var employeeId = paystubEmail.Paystub?.Employee?.RowID;
                var employee = paystubEmail.Paystub?.Employee;
                var organizationId = paystubEmail.Paystub?.OrganizationID;

                string validationErrorMessage;
                if (!Validate(paystubEmail, errorTitle, currentPayPeriod, employeeId, employee, organizationId))
                {
                    return;
                }

                DateTime payDate = GetPayDate(currentPayPeriod);
                var employeeIds = new int[] { employeeId.Value };

                var payslipBuilder = _payslipBuilder.CreateReportDocument(
                    payPeriodId: currentPayPeriod.RowID.Value,
                    isActual: 0,
                    employeeIds: employeeIds);

                if (payslipBuilder.CheckIfEmployeeExists(employeeId.Value) == false)
                {
                    validationErrorMessage = $"{errorTitle} Cannot find employee in the payslip report datatable.";
                    WriteToFile(validationErrorMessage);
                    _paystubEmailRepository.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                    return;
                }

                var employeeNumber = employee.EmployeeNo ?? "";
                var fileName = $"Payslip-{payDate:yyyy-MM-dd}-{employeeNumber}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
                var pdfFile = CreatePDF(payslipBuilder, employee.BirthDate, fileName);

                SendEmail(paystubEmail, paystubEmailLog, payDate, employee.EmailAddress, pdfFile, fileName);
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                WriteToFile(ex.StackTrace);

                if (paystubEmail != null)
                {
                    string validationErrorMessage = $"[System Error] {ex.Message}";
                    _paystubEmailRepository.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                }
            }
        }

        private bool Validate(PaystubEmail paystubEmail, string errorTitle, PayPeriod currentPayPeriod, int? employeeId, Employee employee, int? organizationId)
        {
            string validationErrorMessage = GetErrorMessage(
                errorTitle,
                currentPayPeriod,
                employeeId,
                organizationId);

            if (string.IsNullOrWhiteSpace(validationErrorMessage) == false)
            {
                WriteToFile(validationErrorMessage);
                _paystubEmailRepository.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.EmailAddress))
            {
                validationErrorMessage = $"{errorTitle} Email address is null or empty.";
                WriteToFile(validationErrorMessage);
                _paystubEmailRepository.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                return false;
            }

            return true;
        }

        private DateTime GetPayDate(PayPeriod currentPayPeriod)
        {
            // only supports semi-monthly. no weekly
            var currentMonth = new DateTime(currentPayPeriod.Year, currentPayPeriod.Month, 1);
            var day = currentPayPeriod.IsFirstHalf ? 15 : currentMonth.AddMonths(1).AddDays(-1).Day;

            return new DateTime(currentMonth.Year, currentMonth.Month, day);
        }

        private string CreatePDF(
            PayslipBuilder payslipBuilder,
            DateTime birthDate,
            string fileName)
        {
            string saveFolderPath = GetOrCreateDirectory(PayslipsFolderName);

            string password = birthDate.ToString("MMddyyyy");

            PayslipBuilder builder = (PayslipBuilder)payslipBuilder.GeneratePDF(saveFolderPath, fileName);
            builder.AddPdfPassword(password);

            return builder.GetPDF();
        }

        private void SendEmail(
            PaystubEmail paystubEmail,
            string paystubEmailLog,
            DateTime payDate,
            string emailAddress,
            string pdfFile,
            string fileName)
        {
            var cutoffDate = payDate.ToString("MMMM d, yyyy");

            var subject = $"Payslip for {cutoffDate}";

            var body = $"Please see attached payslip for {cutoffDate}. " +
                $"\n\n" +
                $"Your payslip is password-protected to ensure the security of your account. The default password is your date of birth with the following format mmddyyyy. For example, if your birthday is February 2, 1988, your password is \"02021988\"";

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Cinema2000)
            {
                body += $"\n\n" +
                $"Kindly contact the Human Resources Dept. at 571-2000 local 102 or e-mail at hrd@cinema2000.com.ph for any inquiries or corrections regarding your salary.";
            }

            body += $"\n\n" +
            $"Thank you," +
            $"\n" +
            $"HRD";

            var attachments = new string[] { pdfFile };

            WriteToFile($"{paystubEmailLog} Sending...");

            _emailSender.SendEmail(emailAddress, subject, body, attachments);

            WriteToFile($"{paystubEmailLog} Email Sent! [Email address: {emailAddress}]");

            _paystubEmailRepository.Finish(paystubEmail.RowID, fileName, emailAddress);
        }

        private string GetErrorMessage(
            string errorTitle,
            PayPeriod currentPayPeriod,
            int? employeeId,
            int? organizationId)
        {
            string errorMessage = string.Empty;

            if (currentPayPeriod == null)
            {
                errorMessage = $"{errorTitle} currentPayPeriod is null.";
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
            if (!System.IO.File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(filepath))
                {
                    sw.WriteLine($"[{DateTime.Now}] {Message}");
                }
            }
            else
            {
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
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