using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports;
using GlobagilityShared.EmailSender;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

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
    //  installUtil /u C:\AccupayWindowsService\AccupayWindowsService.exe
    //  installUtil C:\AccupayWindowsService\AccupayWindowsService.exe
    //
    // Dont forget to start the service!

    // If there are errors in installing this to Windows Service, try to run this from Visual Studio. It will show the errors if there is any. If there
    // is not, it will just say 'Cannot start service from the command line or a debugger.' and that means that there is no error in trying to start the service.

    public partial class EmailService : ServiceBase
    {
        private Timer _emailTimer;
        private EmailSender _emailSender;

        private const string PayslipsFolderName = "Payslips";
        private const string LogsFolderName = "Logs";

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EmailService(IServiceScopeFactory serviceScopeFactory)
        {
            InitializeComponent();

            _emailTimer = new Timer();
            _emailSender = new EmailSender(new EmailConfig());
            _serviceScopeFactory = serviceScopeFactory;
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

        private async void OnElapsedTimeEmail(object source, ElapsedEventArgs e)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var repository = serviceScope.ServiceProvider.GetRequiredService<IPaystubEmailRepository>();
                var dataservice = serviceScope.ServiceProvider.GetRequiredService<IPaystubEmailDataService>();
                var payslipBuilder = serviceScope.ServiceProvider.GetRequiredService<IPayslipBuilder>();
                var systemOwnerService = serviceScope.ServiceProvider.GetRequiredService<ISystemOwnerService>();

                await ExecuteSendEmail(repository, dataservice, payslipBuilder, systemOwnerService);
            }
        }

        private async Task ExecuteSendEmail(
            IPaystubEmailRepository repository,
            IPaystubEmailDataService dataService,
            IPayslipBuilder payslipBuilder,
            ISystemOwnerService systemOwnerService)
        {
            PaystubEmail paystubEmail = null;

            try
            {
                WriteToFile("--OnElapsedTimeEmail--");

                paystubEmail = repository.FirstOnQueueWithPaystubDetails();
                if (paystubEmail == null)
                {
                    WriteToFile("No queued email.");
                    return;
                }
                else
                {
                    await dataService.SetStatusToProcessing(paystubEmail.RowID);
                }

                var paystubEmailLog = $"[paystubId: {paystubEmail.Paystub?.RowID}]";
                var errorTitle = $"[Error] {paystubEmailLog}";

                var currentPayPeriod = paystubEmail.Paystub?.PayPeriod;
                var employeeId = paystubEmail.Paystub?.Employee?.RowID;
                var employee = paystubEmail.Paystub?.Employee;
                var organizationId = paystubEmail.Paystub?.OrganizationID;

                string validationErrorMessage;
                if (!(await Validate(paystubEmail, errorTitle, currentPayPeriod, employeeId, employee, organizationId, dataService)))
                {
                    return;
                }

                DateTime payDate = GetPayDate(currentPayPeriod);
                var employeeIds = new int[] { employeeId.Value };

                var builder = await payslipBuilder.CreateReportDocumentAsync(
                    payPeriodId: currentPayPeriod.RowID.Value,
                    isActual: paystubEmail.IsActual,
                    employeeIds: employeeIds);

                if (builder.CheckIfEmployeeExists(employeeId.Value) == false)
                {
                    validationErrorMessage = $"{errorTitle} Cannot find employee in the payslip report datatable.";
                    WriteToFile(validationErrorMessage);
                    await dataService.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                    return;
                }

                var employeeNumber = employee.EmployeeNo ?? "";
                var fileName = $"Payslip-{payDate:yyyy-MM-dd}-{employeeNumber}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
                var pdfFile = CreatePDF(builder, employee.BirthDate, fileName);

                await SendEmail(paystubEmail, paystubEmailLog, payDate, employee.EmailAddress, pdfFile, fileName, systemOwnerService, dataService);
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                WriteToFile(ex.StackTrace);

                if (paystubEmail != null)
                {
                    string validationErrorMessage = $"[System Error] {ex.Message}";
                    await dataService.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                }
            }
        }

        private async Task<bool> Validate(
            PaystubEmail paystubEmail,
            string errorTitle,
            PayPeriod currentPayPeriod,
            int? employeeId,
            Employee employee,
            int? organizationId,
            IPaystubEmailDataService dataService)
        {
            string validationErrorMessage = GetErrorMessage(
                errorTitle,
                currentPayPeriod,
                employeeId,
                organizationId);

            if (string.IsNullOrWhiteSpace(validationErrorMessage) == false)
            {
                WriteToFile(validationErrorMessage);
                await dataService.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.EmailAddress))
            {
                validationErrorMessage = $"{errorTitle} Email address is null or empty.";
                WriteToFile(validationErrorMessage);
                await dataService.SetStatusToFailed(paystubEmail.RowID, validationErrorMessage);
                return false;
            }

            return true;
        }

        private DateTime GetPayDate(PayPeriod currentPayPeriod)
        {
            // only supports semi-monthly. no weekly
            var currentMonth = new DateTime(currentPayPeriod.Year, currentPayPeriod.Month, 1);
            //var day = currentPayPeriod.IsFirstHalf ? 15 : currentMonth.AddMonths(1).AddDays(-1).Day;
            var day = currentPayPeriod.IsFirstHalf ? 15 : DateTime.DaysInMonth(currentPayPeriod.Year, currentMonth.AddMonths(1).AddDays(-1).Month);

            return new DateTime(currentMonth.Year, currentMonth.Month, day);
        }

        private string CreatePDF(
            IPayslipBuilder payslipBuilder,
            DateTime birthDate,
            string fileName)
        {
            string saveFolderPath = GetOrCreateDirectory(PayslipsFolderName);

            string password = birthDate.ToString("MMddyyyy");

            IPayslipBuilder builder = (IPayslipBuilder)payslipBuilder.GeneratePDF(saveFolderPath, fileName);
            builder.AddPdfPassword(password);

            return builder.GetPDF();
        }

        private async Task SendEmail(
            PaystubEmail paystubEmail,
            string paystubEmailLog,
            DateTime payDate,
            string emailAddress,
            string pdfFile,
            string fileName,
            ISystemOwnerService systemOwnerService,
            IPaystubEmailDataService dataService)
        {
            var cutoffDate = payDate.ToString("MMMM d, yyyy");

            var subject = $"Payslip for {cutoffDate}";

            var body = $"Please see attached payslip for {cutoffDate}. " +
                $"\n\n" +
                $"Your payslip is password-protected to ensure the security of your account. The default password is your date of birth with the following format mmddyyyy. For example, if your birthday is February 2, 1988, your password is \"02021988\"";

            if (systemOwnerService.GetCurrentSystemOwner() == SystemOwner.Cinema2000)
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

            await dataService.Finish(paystubEmail.RowID, fileName, emailAddress);
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
