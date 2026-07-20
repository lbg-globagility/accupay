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

namespace AccuPay.Web.Overtimes
{
    public class OvertimeEmailService
    {
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OvertimeEmailService> _logger;

        public OvertimeEmailService(IOvertimeRepository overtimeRepository,
            IEmployeeApproverRepository employeeApproverRepository, EmailService emailService,
            IConfiguration configuration, ILogger<OvertimeEmailService> logger)
        {
            _overtimeRepository = overtimeRepository;
            _employeeApproverRepository = employeeApproverRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendFilingForApprovalEmailAsync(int filingId)
        {
            var filing = await _overtimeRepository.GetByIdWithEmployeeAsync(filingId);
            if (filing?.EmployeeID == null)
            {
                _logger.LogWarning("Overtime filing {FilingId} was not found or has no employee.", filingId);
                return false;
            }

            var approvers = await _employeeApproverRepository.GetByEmployeeIdAsync(filing.EmployeeID.Value);
            var recipients = approvers.Select(x => x.Approver?.EmailAddress)
                .Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (!recipients.Any())
            {
                _logger.LogWarning("No approver emails found for overtime filing {FilingId}.", filingId);
                return false;
            }

            var hours = 24;
            if (int.TryParse(_configuration["App:ApprovalTokenHours"], out var configuredHours) && configuredHours > 0)
                hours = configuredHours;
            var token = ApprovalTokenHelper.GenerateToken(filingId,
                _configuration["App:ApprovalTokenSecret"] ?? string.Empty, TimeSpan.FromHours(hours));
            var domain = (_configuration["App:Domain"] ?? string.Empty).TrimEnd('/');
            var approvePath = $"/api/overtimes/filings/{filingId}/approve?token={Uri.EscapeDataString(token)}";
            var rejectPath = $"/api/overtimes/filings/{filingId}/reject?token={Uri.EscapeDataString(token)}";
            var email = new Email("[AccuPay] Overtime filing approval request", recipients)
            {
                Html = BuildHtml(filing, domain + approvePath, domain + rejectPath)
            };
            await _emailService.Send(email);
            _logger.LogInformation("Approval email for overtime filing {FilingId} sent to {Count} recipients.", filingId, recipients.Count);
            return true;
        }

        private static string BuildHtml(Overtime filing, string approveUrl, string rejectUrl)
        {
            string E(object value) => WebUtility.HtmlEncode(value?.ToString() ?? string.Empty);
            var html = new StringBuilder();
            html.AppendLine("<div style=\"font-family:Segoe UI, Arial, sans-serif;\">");
            html.AppendLine("<h2>Overtime Filing Approval Request</h2><table style=\"border-collapse:collapse;\">");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Employee:</td><td style=\"padding:4px;\">{E(filing.Employee?.FullName)}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Date:</td><td style=\"padding:4px;\">{filing.OTStartDate:yyyy-MM-dd}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Time:</td><td style=\"padding:4px;\">{E(filing.OTStartTime)} to {E(filing.OTEndTime)}</td></tr>");
            html.AppendLine($"<tr><td style=\"padding:4px;font-weight:bold;\">Reason:</td><td style=\"padding:4px;\">{E(filing.Reason)}</td></tr></table>");
            html.AppendLine("<p>Please review the filing and approve or reject it:</p>");
            html.AppendLine($"<p><a href=\"{E(approveUrl)}\" style=\"display:inline-block;padding:10px 16px;background:#0078d4;color:white;text-decoration:none;border-radius:4px;margin-right:8px;\">Approve</a>");
            html.AppendLine($"<a href=\"{E(rejectUrl)}\" style=\"display:inline-block;padding:10px 16px;background:#a80000;color:white;text-decoration:none;border-radius:4px;\">Reject</a></p></div>");
            return html.ToString();
        }
    }
}
