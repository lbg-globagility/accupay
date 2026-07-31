using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogEmailService
    {
        private const string DefaultSubject = "[AccuPay] Timelog filing correction request";

        private const string DefaultHtmlBody =
            "<div style=\"font-family:Segoe UI, Arial, sans-serif;\">" +
            "<p>Hi {approver},</p>" +
            "<p>{employee} filed the following time log correction(s):</p>" +
            "{filings}" +
            "<p>{reviewButton}</p>" +
            "</div>";

        private const string DefaultTextBody =
            "Hi {approver},\n\n" +
            "{employee} filed the following time log correction(s):\n" +
            "{filings}\n" +
            "Review: {reviewButton}";

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

        public async Task<bool> SendFilingForApprovalEmailAsync(ICollection<int> filingIds)
        {
            if (filingIds == null || !filingIds.Any())
            {
                throw new BusinessLogicException("At least one time log filing id is required.");
            }

            var distinctFilingIds = filingIds.Distinct().ToList();
            var filings = await _timeLogRepository.GetFilingsByIdsAsync(distinctFilingIds);

            if (filings.Count != distinctFilingIds.Count)
            {
                throw new BusinessLogicException("One or more time log filings were not found.");
            }

            if (filings.Any(f => f.EmployeeID == null) || filings.Select(f => f.EmployeeID).Distinct().Count() > 1)
            {
                throw new BusinessLogicException("Time log filings must all belong to the same employee.");
            }

            if (filings.Any(f => f.IsNotifyEmail))
            {
                throw new BusinessLogicException("Already emailed a time log filing to approvers");
            }

            var employeeId = filings.First().EmployeeID.Value;

            var employeeApprovers = await _employeeApproverRepository.GetByEmployeeIdAsync(employeeId);
            var approvers = employeeApprovers
                .Where(ea => ea.Approver != null && !string.IsNullOrWhiteSpace(ea.Approver.EmailAddress))
                .GroupBy(ea => ea.Approver.EmailAddress, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            if (!approvers.Any())
            {
                throw new BusinessLogicException("No approver emails found for the time log filing(s).");
            }

            var organizationId = filings.First().OrganizationID;
            var template = await _emailTemplateRepository.GetByCodeAsync(
                EmailTemplate.TimeLogFilingApprovalCode, organizationId);

            var subject = string.IsNullOrWhiteSpace(template?.Subject) ? DefaultSubject : template.Subject;
            var htmlBody = string.IsNullOrWhiteSpace(template?.HtmlBody) ? DefaultHtmlBody : template.HtmlBody;
            var textBody = string.IsNullOrWhiteSpace(template?.TextBody) ? DefaultTextBody : template.TextBody;

            var employeeName = filings.First().Employee?.FullName ?? "An employee";

            foreach (var filing in filings)
            {
                filing.IsNotifyEmail = true;
                await _timeLogRepository.UpdateFilingAsync(filing);
            }

            foreach (var employeeApprover in approvers)
            {
                var approver = employeeApprover.Approver;
                var approverName = $"{approver.FirstName} {approver.LastName}".Trim();

                var token = CreateToken(employeeApprover.RowID.Value, approver.EmailAddress);
                var reviewUrl = CreateUrl(
                    $"/timelog-approvals?employeeApproverId={employeeApprover.RowID.Value}&token={Uri.EscapeDataString(token)}");

                var reviewButtonHtml = $"<a href=\"{reviewUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;\">Review Requests</a>";

                var email = new Email(subject, approver.EmailAddress)
                {
                    Text = ApplyPlaceholders(textBody, filings, approverName, employeeName, reviewUrl, encodeValues: false),
                    Html = ApplyPlaceholders(htmlBody, filings, approverName, employeeName, reviewButtonHtml, encodeValues: true)
                };

                await _emailService.Send(email);
            }

            _logger.LogInformation(
                "Approval email for {Count} time log filing(s) sent to {ApproverCount} recipients.",
                filings.Count, approvers.Count);
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
            IEnumerable<EmployeeTimelogFiling> filings,
            string approverName,
            string employeeName,
            string reviewButtonOrUrl,
            bool encodeValues)
        {
            string E(string value) => encodeValues ? WebUtility.HtmlEncode(value) : value;

            return template
                .Replace("{approver}", E(approverName))
                .Replace("{employee}", E(employeeName))
                .Replace("{filings}", encodeValues ? BuildFilingsHtml(filings) : BuildFilingsText(filings))
                .Replace("{reviewButton}", reviewButtonOrUrl);
        }

        private static string BuildFilingsHtml(IEnumerable<EmployeeTimelogFiling> filings)
        {
            var sb = new StringBuilder("<ul>");
            foreach (var filing in filings)
            {
                sb.Append("<li>")
                  .Append(WebUtility.HtmlEncode(DescribeFiling(filing)))
                  .Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private static string BuildFilingsText(IEnumerable<EmployeeTimelogFiling> filings)
        {
            return string.Join(Environment.NewLine, filings.Select(f => $"- {DescribeFiling(f)}"));
        }

        private static string DescribeFiling(EmployeeTimelogFiling filing)
        {
            var reason = string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason;

            return $"{filing.EntryType} on {filing.LogDate:yyyy-MM-dd} ({filing.TimeStamp}) - Reason: {reason}";
        }
    }
}
