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
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeEmailService
    {
        private const string DefaultSubject = "[AccuPay] Overtime filing approval request";

        private const string DefaultHtmlBody =
            "<div style=\"font-family:Segoe UI, Arial, sans-serif;\">" +
            "<p>Hi {approver},</p>" +
            "<p>{employee} filed the following overtime(s):</p>" +
            "{filings}" +
            "<p>{reviewButton}</p>" +
            "</div>";

        private const string DefaultTextBody =
            "Hi {approver},\n\n" +
            "{employee} filed the following overtime(s):\n" +
            "{filings}\n" +
            "Review: {reviewButton}";

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

        public async Task<bool> SendFilingForApprovalEmailAsync(ICollection<int> filingIds)
        {
            if (filingIds == null || !filingIds.Any())
            {
                throw new BusinessLogicException("At least one overtime filing id is required.");
            }

            var distinctFilingIds = filingIds.Distinct().ToList();
            var filings = await _overtimeRepository.GetByIdsWithEmployeeAsync(distinctFilingIds);

            if (filings.Count != distinctFilingIds.Count)
            {
                throw new BusinessLogicException("One or more overtime filings were not found.");
            }

            if (filings.Any(f => f.EmployeeID == null) || filings.Select(f => f.EmployeeID).Distinct().Count() > 1)
            {
                throw new BusinessLogicException("Overtime filings must all belong to the same employee.");
            }

            if (filings.Any(f => f.IsNotifyEmail))
            {
                throw new BusinessLogicException("Already emailed an overtime filing to approvers");
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
                throw new BusinessLogicException("No approver emails found for the overtime filing(s).");
            }

            var organizationId = filings.First().OrganizationID;
            var template = await _emailTemplateRepository.GetByCodeAsync(
                EmailTemplate.OvertimeFilingApprovalCode, organizationId);

            var subject = string.IsNullOrWhiteSpace(template?.Subject) ? DefaultSubject : template.Subject;
            var htmlBody = string.IsNullOrWhiteSpace(template?.HtmlBody) ? DefaultHtmlBody : template.HtmlBody;
            var textBody = string.IsNullOrWhiteSpace(template?.TextBody) ? DefaultTextBody : template.TextBody;

            var employeeName = filings.First().Employee?.FullName ?? "An employee";

            foreach (var filing in filings)
            {
                filing.IsNotifyEmail = true;
                await _overtimeRepository.UpdateAsync(filing);
            }

            foreach (var employeeApprover in approvers)
            {
                var approver = employeeApprover.Approver;
                var approverName = $"{approver.FirstName} {approver.LastName}".Trim();

                var token = CreateToken(employeeApprover.RowID.Value, approver.EmailAddress);
                var reviewUrl = CreateUrl(
                    $"/overtime-approvals?employeeApproverId={employeeApprover.RowID.Value}&token={Uri.EscapeDataString(token)}");

                var reviewButtonHtml = $"<a href=\"{reviewUrl}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;\">Review Requests</a>";

                var email = new Email(subject, approver.EmailAddress)
                {
                    Text = ApplyPlaceholders(textBody, filings, approverName, employeeName, reviewUrl, encodeValues: false),
                    Html = ApplyPlaceholders(htmlBody, filings, approverName, employeeName, reviewButtonHtml, encodeValues: true)
                };

                await _emailService.Send(email);
            }

            _logger.LogInformation(
                "Approval email for {Count} overtime filing(s) sent to {ApproverCount} recipients.",
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
            IEnumerable<Overtime> filings,
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

        private static string BuildFilingsHtml(IEnumerable<Overtime> filings)
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

        private static string BuildFilingsText(IEnumerable<Overtime> filings)
        {
            return string.Join(Environment.NewLine, filings.Select(f => $"- {DescribeFiling(f)}"));
        }

        private static string DescribeFiling(Overtime filing)
        {
            var hours = filing.OTStartTimeFull.HasValue && filing.OTEndTimeFull.HasValue
                ? (filing.OTEndTimeFull.Value - filing.OTStartTimeFull.Value).TotalHours
                : 0;

            var reason = string.IsNullOrWhiteSpace(filing.Reason) ? "N/A" : filing.Reason;

            return $"{hours.ToString("0.##")} h on {filing.OTStartDate:yyyy-MM-dd} - Reason: {reason}";
        }
    }
}
