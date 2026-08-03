using AccuPay.Core.Interfaces;
using AccuPay.Web.Approvals.Models;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ApprovalsController : ControllerBase
    {
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        private readonly IConfiguration _configuration;

        public ApprovalsController(
            IEmployeeApproverRepository employeeApproverRepository,
            IConfiguration configuration)
        {
            _employeeApproverRepository = employeeApproverRepository;
            _configuration = configuration;
        }

        [HttpGet("verify-token")]
        public async Task<ActionResult<EmployeeApproverTokenDto>> VerifyToken(int employeeApproverId, string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;

            if (!ApprovalTokenHelper.ValidateToken(token, employeeApproverId, secret, out var error, out var approverEmail))
            {
                return BadRequest(error);
            }

            var employeeApprover = await _employeeApproverRepository.GetByIdAsync(employeeApproverId);

            if (employeeApprover?.Employee == null || employeeApprover.Approver == null)
            {
                return NotFound();
            }

            if (!string.Equals(employeeApprover.Approver.EmailAddress, approverEmail, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Token does not match approver.");
            }

            return new EmployeeApproverTokenDto
            {
                EmployeeApproverId = employeeApprover.RowID.Value,
                EmployeeId = employeeApprover.EmployeeID,
                EmployeeName = employeeApprover.Employee.FullName,
                EmployeeNo = employeeApprover.Employee.EmployeeNo,
                ApproverId = employeeApprover.ApproverID,
                ApproverName = $"{employeeApprover.Approver.FirstName} {employeeApprover.Approver.LastName}".Trim(),
                ApproverEmail = employeeApprover.Approver.EmailAddress
            };
        }
    }
}
