using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogEmailService
    {
        private const string DefaultSubject = "[AccuPay] Timelog filing correction request";

        private const string DefaultHtmlBody =
            "<div style=\"font-family:Segoe UI, Arial, sans-serif;\">" +
            "<p>Hi {approver},</p>" +
            "<p>{employee} filed a time log correction for {date} ({time}).</p>" +
            "<p>Reason: {reason}</p>" +
            "<p>{approveButton} {rejectButton}</p>" +
            "</div>";

        private const string DefaultTextBody =
            "Hi {approver},\n\n" +
            "{employee} filed a time log correction for {date} ({time}).\n" +
            "Reason: {reason}\n\n" +
            "Approve: {approveButton}\n" +
            "Reject: {rejectButton}";

        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TimeLogEmailService> _logger;

        public TimeLogEmailService(
            ITimeLogRepository timeLogRepository,
            IEmployeeApproverRepository employeeApproverRepository,
            IEmailTemplateRepository emailTemplateRepository,
            EmailService emailService,
            IConfiguration configuration,
            ILogger<TimeLogEmailService> logger)
        {
            _timeLogRepository = timeLogRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailTemplateRepository = emailTemplateRepository;
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
            var approvers = employeeApprovers
                .Select(ea => ea.Approver)
                .Where(a => a != null && !string.IsNullOrWhiteSpace(a.EmailAddress))
                .GroupBy(a => a.EmailAddress, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            if (!approvers.Any())
            {
                _logger.LogWarning("No approver emails found for employee {EmployeeId} when sending filing {FilingId}.",
                    filing.EmployeeID, filingId);
                return false;
            }

            var domain = _configuration["App:Domain"] ?? string.Empty;
            var baseDomain = string.IsNullOrWhiteSpace(domain)
                ? string.Empty
                : domain.TrimEnd('/');

            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            // token TTL (hours) default 24
            var ttlHours = 24;
            int.TryParse(_configuration["App:ApprovalTokenHours"], out var configuredHours);
            if (configuredHours > 0) ttlHours = configuredHours;

            var token = ApprovalTokenHelper.GenerateToken(filingId, secret, TimeSpan.FromHours(ttlHours));

            var approveUrl = string.IsNullOrWhiteSpace(baseDomain)
                ? $"/api/timelogs/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}"
                : $"{baseDomain}/api/timelogs/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}";

            var rejectUrl = string.IsNullOrWhiteSpace(baseDomain)
                ? $"/api/timelogs/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}"
                : $"{baseDomain}/api/timelogs/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}";

            var employeeName = filing.Employee?.FullName ?? "An employee";

            var template = await _emailTemplateRepository.GetByCodeAsync(
                EmailTemplate.TimeLogFilingApprovalCode, filing.OrganizationID);

            var subject = string.IsNullOrWhiteSpace(template?.Subject) ? DefaultSubject : template.Subject;
            var htmlBody = string.IsNullOrWhiteSpace(template?.HtmlBody) ? DefaultHtmlBody : template.HtmlBody;
            var textBody = string.IsNullOrWhiteSpace(template?.TextBody) ? DefaultTextBody : template.TextBody;

            var approveButtonHtml = $"<a href=\"{approveUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;margin-right:8px;\">Approve</a>";
            var rejectButtonHtml = $"<a href=\"{rejectUrl}\" style=\"display:inline-block;padding:10px 16px;background:#a80000;color:white;text-decoration:none;border-radius:4px;\">Reject</a>";

            foreach (var approver in approvers)
            {
                var approverName = $"{approver.FirstName} {approver.LastName}".Trim();

                var email = new Email(subject, approver.EmailAddress);
                email.Text = ApplyPlaceholders(textBody, filing, approverName, employeeName, approveUrl, rejectUrl);
                email.Html = ApplyPlaceholders(htmlBody, filing, approverName, employeeName, approveButtonHtml, rejectButtonHtml);

                await _emailService.Send(email);
            }

            _logger.LogInformation("Approval email for filing {FilingId} sent to {Count} recipients.", filingId, approvers.Count);
            return true;
        }

        private static string ApplyPlaceholders(
            string template,
            EmployeeTimelogFiling filing,
            string approverName,
            string employeeName,
            string approveButtonOrUrl,
            string rejectButtonOrUrl)
        {
            return template
                .Replace("{approver}", approverName)
                .Replace("{employee}", employeeName)
                .Replace("{date}", filing.LogDate.ToString("yyyy-MM-dd"))
                .Replace("{time}", filing.TimeStamp)
                .Replace("{reason}", string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason)
                .Replace("{approveButton}", approveButtonOrUrl)
                .Replace("{rejectButton}", rejectButtonOrUrl);
        }
    }
}
