using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.TimeLogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeEmailService
    {
        private const string DefaultSubject = "[AccuPay] Overtime filing approval request";

        private const string DefaultHtmlBody =
            "<div style=\"font-family:Segoe UI, Arial, sans-serif;\">" +
            "<p>Hi {approver},</p>" +
            "<p>{employee} filed {hours} h of overtime on {date}.</p>" +
            "<p>Reason: {reason}</p>" +
            "<p>{approveButton} {rejectButton}</p>" +
            "</div>";

        private const string DefaultTextBody =
            "Hi {approver},\n\n" +
            "{employee} filed {hours} h of overtime on {date}.\n" +
            "Reason: {reason}\n\n" +
            "Approve: {approveButton}\n" +
            "Reject: {rejectButton}";

        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OvertimeEmailService> _logger;

        public OvertimeEmailService(IOvertimeRepository overtimeRepository,
            IEmployeeApproverRepository employeeApproverRepository,
            IEmailTemplateRepository emailTemplateRepository,
            EmailService emailService,
            IConfiguration configuration, ILogger<OvertimeEmailService> logger)
        {
            _overtimeRepository = overtimeRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendFilingForApprovalEmailAsync(int filingId)
        {
            var filing = await _overtimeRepository.GetByIdWithEmployeeAsync(filingId);
            if (filing == null)
            {
                throw new BusinessLogicException("Filing {FilingId} not found when attempting to send approval email.");

            }
            if (!filing.EmployeeID.HasValue)
            {
                throw new BusinessLogicException("Filing {FilingId} has no EmployeeID.");
            }
            if (filing.Status != Overtime.StatusPending)
            {
                throw new BusinessLogicException("Only pending overtime filings can be emailed for approval.");
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
                throw new BusinessLogicException("No approver emails found for overtime filing {FilingId}.");
               
            }

            var template = await _emailTemplateRepository.GetByCodeAsync(
                EmailTemplate.OvertimeFilingApprovalCode, filing.OrganizationID);

            var subject = string.IsNullOrWhiteSpace(template?.Subject) ? DefaultSubject : template.Subject;
            var htmlBody = string.IsNullOrWhiteSpace(template?.HtmlBody) ? DefaultHtmlBody : template.HtmlBody;
            var textBody = string.IsNullOrWhiteSpace(template?.TextBody) ? DefaultTextBody : template.TextBody;

            var employeeName = filing.Employee?.FullName ?? "An employee";

            var hours = 24;
            if (int.TryParse(_configuration["App:ApprovalTokenHours"], out var configuredHours) && configuredHours > 0)
                hours = configuredHours;
            var domain = (_configuration["App:Domain"] ?? string.Empty).TrimEnd('/');
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            filing.IsNotifyEmail = true;
            await _overtimeRepository.UpdateAsync(filing);
            foreach (var approver in approvers)
            {
                var approverName = $"{approver.FirstName} {approver.LastName}".Trim();

                var token = ApprovalTokenHelper.GenerateToken(filingId, secret, TimeSpan.FromHours(hours), approver.EmailAddress);
                var approveUrl = domain + $"/api/overtimes/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}";
                var rejectUrl = domain + $"/api/overtimes/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}";

                var approveButtonHtml = $"<a href=\"{approveUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;margin-right:8px;\">Approve</a>";
                var rejectButtonHtml = $"<a href=\"{rejectUrl}\" style=\"display:inline-block;padding:10px 16px;background:#a80000;color:white;text-decoration:none;border-radius:4px;\">Reject</a>";

                var email = new Email(subject, approver.EmailAddress)
                {
                    Text = ApplyPlaceholders(textBody, filing, approverName, employeeName, approveUrl, rejectUrl, encodeValues: false),
                    Html = ApplyPlaceholders(htmlBody, filing, approverName, employeeName, approveButtonHtml, rejectButtonHtml, encodeValues: true)
                };

                await _emailService.Send(email);
            }

            _logger.LogInformation("Approval email for overtime filing {FilingId} sent to {Count} recipients.", filingId, approvers.Count);
            return true;
        }

        private static string ApplyPlaceholders(
            string template,
            Overtime filing,
            string approverName,
            string employeeName,
            string approveButtonOrUrl,
            string rejectButtonOrUrl,
            bool encodeValues)
        {
            string E(string value) => encodeValues ? WebUtility.HtmlEncode(value) : value;

            var hours = filing.OTStartTimeFull.HasValue && filing.OTEndTimeFull.HasValue
                ? (filing.OTEndTimeFull.Value - filing.OTStartTimeFull.Value).TotalHours
                : 0;

            return template
                .Replace("{approver}", E(approverName))
                .Replace("{employee}", E(employeeName))
                .Replace("{hours}", hours.ToString("0.##"))
                .Replace("{date}", filing.OTStartDate.ToString("yyyy-MM-dd"))
                .Replace("{reason}", E(string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason))
                .Replace("{approveButton}", approveButtonOrUrl)
                .Replace("{rejectButton}", rejectButtonOrUrl);
        }
    }
}
