using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogEmailService
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TimeLogEmailService> _logger;

        public TimeLogEmailService(
            ITimeLogRepository timeLogRepository,
            IEmployeeApproverRepository employeeApproverRepository,
            EmailService emailService,
            IConfiguration configuration,
            ILogger<TimeLogEmailService> logger)
        {
            _timeLogRepository = timeLogRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendFilingForApprovalEmailAsync(int filingId)
        {
            var filing = await _timeLogRepository.GetFilingByIdAsync(filingId);
            if (filing == null)
            {
                _logger.LogWarning("Filing {FilingId} not found when attempting to send approval email.", filingId);
                return false;
            }

            if (!filing.EmployeeID.HasValue)
            {
                _logger.LogWarning("Filing {FilingId} has no EmployeeID.", filingId);
                return false;
            }

            var employeeApprovers = await _employeeApproverRepository.GetByEmployeeIdAsync(filing.EmployeeID.Value);
            var recipients = employeeApprovers
                .Select(ea => ea.Approver?.EmailAddress)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!recipients.Any())
            {
                _logger.LogWarning("No approver emails found for employee {EmployeeId} when sending filing {FilingId}.",
                    filing.EmployeeID, filingId);
                return false;
            }

            var domain = _configuration["App:Domain"] ?? string.Empty;
            var filingUrl = string.IsNullOrWhiteSpace(domain)
                ? $"/timelogs/filings/{filingId}"
                : $"{domain.TrimEnd('/')}/timelogs/filings/{filingId}";

            var subject = "[AccuPay] Timelog filing approval request";
            var html = BuildFilingHtml(filing, filingUrl);

            var email = new Email(subject, recipients);
            email.Html = html;

            await _emailService.Send(email);

            _logger.LogInformation("Approval email for filing {FilingId} sent to {Count} recipients.", filingId, recipients.Count);
            return true;
        }

        private static string BuildFilingHtml(EmployeeTimelogFiling filing, string filingUrl)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div style=\"font-family:Segoe UI, Arial, sans-serif;\">");
            sb.AppendLine($"<h2>Timelog Filing Approval Request</h2>");
            sb.AppendLine("<table style=\"border-collapse:collapse;\">");
            sb.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Employee:</td><td style=\"padding:4px;\">{filing.Employee?.FullName ?? string.Empty}</td></tr>");
            sb.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Date:</td><td style=\"padding:4px;\">{filing.LogDate:yyyy-MM-dd}</td></tr>");
            sb.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Time:</td><td style=\"padding:4px;\">{filing.Time}</td></tr>");
            if (!string.IsNullOrWhiteSpace(filing.Reason))
            {
                sb.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Reason:</td><td style=\"padding:4px;\">{filing.Reason}</td></tr>");
            }
            sb.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Status:</td><td style=\"padding:4px;\">{filing.Status}</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"<p>Please review the filing and approve or reject it in the application:</p>");
            sb.AppendLine($"<p><a href=\"{filingUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;\">Open Filing</a></p>");
            sb.AppendLine("<p>If you prefer to use the API directly, use the existing endpoints to approve or reject the filing.</p>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}
