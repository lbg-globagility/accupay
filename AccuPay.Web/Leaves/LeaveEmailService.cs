using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.TimeLogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Leaves
{
    public class LeaveEmailService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LeaveEmailService> _logger;

        public LeaveEmailService(
            ILeaveRepository leaveRepository,
            IEmployeeApproverRepository employeeApproverRepository,
            EmailService emailService,
            IConfiguration configuration,
            ILogger<LeaveEmailService> logger)
        {
            _leaveRepository = leaveRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendFilingForApprovalEmailAsync(int filingId)
        {
            var filing = await _leaveRepository.GetByIdWithEmployeeAsync(filingId);
            if (filing?.EmployeeID == null)
            {
                _logger.LogWarning("Leave filing {FilingId} was not found or has no employee.", filingId);
                return false;
            }

            var approvers = await _employeeApproverRepository.GetByEmployeeIdAsync(filing.EmployeeID.Value);
            var recipients = approvers.Select(x => x.Approver?.EmailAddress)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (!recipients.Any())
            {
                _logger.LogWarning("No approver emails found for leave filing {FilingId}.", filingId);
                return false;
            }

            var token = CreateToken(filingId);
            var approveUrl = CreateUrl($"/api/leaves/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}");
            var rejectUrl = CreateUrl($"/api/leaves/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}");
            var email = new Email("[AccuPay] Leave filing approval request", recipients)
            {
                Html = BuildHtml(filing, approveUrl, rejectUrl)
            };

            await _emailService.Send(email);
            _logger.LogInformation("Approval email for leave filing {FilingId} sent to {Count} recipients.", filingId, recipients.Count);
            return true;
        }

        private string CreateToken(int id)
        {
            var hours = 24;
            if (int.TryParse(_configuration["App:ApprovalTokenHours"], out var configuredHours) && configuredHours > 0)
                hours = configuredHours;
            return ApprovalTokenHelper.GenerateToken(id, _configuration["App:ApprovalTokenSecret"] ?? string.Empty, TimeSpan.FromHours(hours));
        }

        private string CreateUrl(string path)
        {
            var domain = (_configuration["App:Domain"] ?? string.Empty).TrimEnd('/');
            return string.IsNullOrWhiteSpace(domain) ? path : domain + path;
        }

        private static string BuildHtml(Leave filing, string approveUrl, string rejectUrl)
        {
            string E(object value) => WebUtility.HtmlEncode(value?.ToString() ?? string.Empty);
            var html = new StringBuilder();
            html.AppendLine("<div style=\"font-family:Segoe UI, Arial, sans-serif;\">");
            html.AppendLine("<h2>Leave Filing Approval Request</h2><table style=\"border-collapse:collapse;\">");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Employee:</td><td style=\"padding:4px;\">{E(filing.Employee?.FullName)}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Type:</td><td style=\"padding:4px;\">{E(filing.LeaveType)}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Dates:</td><td style=\"padding:4px;\">{filing.StartDate:yyyy-MM-dd} to {filing.ProperEndDate:yyyy-MM-dd}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Reason:</td><td style=\"padding:4px;\">{E(filing.Reason)}</td></tr></table>");
            html.AppendLine("<p>Please review the filing and approve or reject it:</p>");
            html.AppendLine($"<p><a href=\"{E(approveUrl)}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;margin-right:8px;\">Approve</a>");
            html.AppendLine($"<a href=\"{E(rejectUrl)}\" style=\"display:inline-block;padding:10px 16px;background:#a80000;color:white;text-decoration:none;border-radius:4px;\">Reject</a></p></div>");
            return html.ToString();
        }
    }
}
