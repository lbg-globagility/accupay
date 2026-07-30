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

namespace AccuPay.Web.Leaves
{
    public class LeaveEmailService
    {
        private const string DefaultSubject = "[AccuPay] Leave filing approval request";

        private const string DefaultHtmlBody =
            "<div style=\"font-family:Segoe UI, Arial, sans-serif;\">" +
            "<p>Hi {approver},</p>" +
            "<p>{employee} requested {leavetype} leave ({date} {time}).</p>" +
            "<p>Reason: {reason}</p>" +
            "<p>{approveButton} {rejectButton}</p>" +
            "</div>";

        private const string DefaultTextBody =
            "Hi {approver},\n\n" +
            "{employee} requested {leavetype} leave ({date} {time}).\n" +
            "Reason: {reason}\n\n" +
            "Approve: {approveButton}\n" +
            "Reject: {rejectButton}";

        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LeaveEmailService> _logger;

        public LeaveEmailService(
            ILeaveRepository leaveRepository,
            IEmployeeApproverRepository employeeApproverRepository,
            IEmailTemplateRepository emailTemplateRepository,
            EmailService emailService,
            IConfiguration configuration,
            ILogger<LeaveEmailService> logger)
        {
            _leaveRepository = leaveRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendFilingForApprovalEmailAsync(int filingId)
        {
            var filing = await _leaveRepository.GetByIdWithEmployeeAsync(filingId);
            if (filing?.EmployeeID == null)
            {
                throw new BusinessLogicException("Leave filing {FilingId} was not found or has no employee.");
            }
            if (filing.IsNotifyEmail)
            {
                throw new BusinessLogicException("Already emailed a leave filing to approvers");
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
                throw new BusinessLogicException("No approver emails found for leave filing {FilingId}.");
            }

            var template = await _emailTemplateRepository.GetByCodeAsync(
                EmailTemplate.LeaveFilingApprovalCode, filing.OrganizationID);

            var subject = string.IsNullOrWhiteSpace(template?.Subject) ? DefaultSubject : template.Subject;
            var htmlBody = string.IsNullOrWhiteSpace(template?.HtmlBody) ? DefaultHtmlBody : template.HtmlBody;
            var textBody = string.IsNullOrWhiteSpace(template?.TextBody) ? DefaultTextBody : template.TextBody;

            var employeeName = filing.Employee?.FullName ?? "An employee";
            filing.IsNotifyEmail = true;
            await _leaveRepository.UpdateAsync(filing);
            foreach (var approver in approvers)
            {
                var approverName = $"{approver.FirstName} {approver.LastName}".Trim();

                var token = CreateToken(filingId, approver.EmailAddress);
                var approveUrl = CreateUrl($"/api/leaves/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}");
                var rejectUrl = CreateUrl($"/api/leaves/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}");

                var approveButtonHtml = $"<a href=\"{approveUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;margin-right:8px;\">Approve</a>";
                var rejectButtonHtml = $"<a href=\"{rejectUrl}\" style=\"display:inline-block;padding:10px 16px;background:#a80000;color:white;text-decoration:none;border-radius:4px;\">Reject</a>";

                var email = new Email(subject, approver.EmailAddress)
                {
                    Text = ApplyPlaceholders(textBody, filing, approverName, employeeName, approveUrl, rejectUrl, encodeValues: false),
                    Html = ApplyPlaceholders(htmlBody, filing, approverName, employeeName, approveButtonHtml, rejectButtonHtml, encodeValues: true)
                };

                await _emailService.Send(email);
            }

            _logger.LogInformation("Approval email for leave filing {FilingId} sent to {Count} recipients.", filingId, approvers.Count);
            return true;
        }

        private string CreateToken(int id, string approverEmail)
        {
            var hours = 24;
            if (int.TryParse(_configuration["App:ApprovalTokenHours"], out var configuredHours) && configuredHours > 0)
                hours = configuredHours;
            return ApprovalTokenHelper.GenerateToken(id, _configuration["App:ApprovalTokenSecret"] ?? string.Empty, TimeSpan.FromHours(hours), approverEmail);
        }

        private string CreateUrl(string path)
        {
            var domain = (_configuration["App:Domain"] ?? string.Empty).TrimEnd('/');
            return string.IsNullOrWhiteSpace(domain) ? path : domain + path;
        }

        private static string ApplyPlaceholders(
            string template,
            Leave filing,
            string approverName,
            string employeeName,
            string approveButtonOrUrl,
            string rejectButtonOrUrl,
            bool encodeValues)
        {
            string E(string value) => encodeValues ? WebUtility.HtmlEncode(value) : value;

            var time = filing.IsWholeDay
                ? "whole day"
                : $"{filing.StartTime.Value.ToString(@"hh\:mm")} - {filing.EndTime.Value.ToString(@"hh\:mm")}";

            return template
                .Replace("{approver}", E(approverName))
                .Replace("{employee}", E(employeeName))
                .Replace("{leavetype}", E(filing.LeaveType))
                .Replace("{date}", filing.StartDate.ToString("yyyy-MM-dd"))
                .Replace("{time}", time)
                .Replace("{reason}", E(string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason))
                .Replace("{approveButton}", approveButtonOrUrl)
                .Replace("{rejectButton}", rejectButtonOrUrl);
        }
    }
}
