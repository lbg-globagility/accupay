using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.TimeLogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            if (filing.Status != Leave.StatusPending)
            {
                throw new BusinessLogicException("Only pending leave filings can be emailed for approval.");
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

            var filingGroup = await GetFilingGroupAsync(filing);
            var firstDate = filingGroup.Min(l => l.StartDate);
            var lastDate = filingGroup.Max(l => l.StartDate);

            var employeeName = filing.Employee?.FullName ?? "An employee";
            filingGroup.ForEach(l => l.IsNotifyEmail = true);
            await _leaveRepository.SaveManyAsync(filingGroup);
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
                    Text = ApplyPlaceholders(textBody, filing, firstDate, lastDate, approverName, employeeName, approveUrl, rejectUrl, encodeValues: false),
                    Html = ApplyPlaceholders(htmlBody, filing, firstDate, lastDate, approverName, employeeName, approveButtonHtml, rejectButtonHtml, encodeValues: true)
                };

                await _emailService.Send(email);
            }

            _logger.LogInformation("Approval email for leave filing {FilingId} sent to {Count} recipients.", filingId, approvers.Count);
            return true;
        }

        // Multi-day self-service filings are saved as one Leave row per day, all sharing the
        // same FilingGroupDate. The approval email is sent for a single row, but the date
        // range shown and the IsNotifyEmail flag should cover every row in the request.
        private async Task<List<Leave>> GetFilingGroupAsync(Leave filing)
        {
            if (!filing.FilingGroupDate.HasValue || filing.EmployeeID == null)
            {
                return new List<Leave> { filing };
            }

            var groupLeaves = await _leaveRepository.GetByFilingGroupDateAsync(
                filing.FilingGroupDate.Value, filing.EmployeeID.Value);

            return groupLeaves.Any() ? groupLeaves.ToList() : new List<Leave> { filing };
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
            DateTime firstDate,
            DateTime lastDate,
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

            var date = firstDate.Date == lastDate.Date
                ? firstDate.ToString("yyyy-MM-dd")
                : $"{firstDate:yyyy-MM-dd} - {lastDate:yyyy-MM-dd}";

            return template
                .Replace("{approver}", E(approverName))
                .Replace("{employee}", E(employeeName))
                .Replace("{leavetype}", E(filing.LeaveType))
                .Replace("{date}", date)
                .Replace("{time}", time)
                .Replace("{reason}", E(string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason))
                .Replace("{approveButton}", approveButtonOrUrl)
                .Replace("{rejectButton}", rejectButtonOrUrl);
        }
    }
}
