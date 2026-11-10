using GlobagilityShared.EmailSender;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AccupayWindowsService
{
    public class EmailConfig : IEmailConfig
    {
        public string SmtpHost => ConfigurationManager.AppSettings["SmtpHost"];

        public int SmtpPort
        {
            get
            {
                var port = ConfigurationManager.AppSettings["SmtpPort"];

                int num = 0;

                if (int.TryParse(port, out num))
                {
                    return num;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string SmtpSender => ConfigurationManager.AppSettings["SmtpSender"];
        public string SmtpDisplayName => ConfigurationManager.AppSettings["SmtpDisplayName"];
        public string SmtpPassword => ConfigurationManager.AppSettings["SmtpPassword"];

        /// <summary>
        /// CC addresses for payslip emails (regular and Access Offshoring payslips).
        /// App.config key "PayslipCcEmails", separated by comma or semicolon.
        /// </summary>
        public IReadOnlyList<string> PayslipCcEmails => GetEmailList("PayslipCcEmails");

        /// <summary>
        /// CC addresses for daily attendance report emails.
        /// App.config key "DailyAttendanceReportCcEmails", separated by comma or semicolon.
        /// </summary>
        public IReadOnlyList<string> DailyAttendanceReportCcEmails => GetEmailList("DailyAttendanceReportCcEmails");

        private static IReadOnlyList<string> GetEmailList(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<string>();
            }

            return value
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}