using AccuPay.Web.Core.Auth;
using AccuPay.Web.Loans;
using AccuPay.Web.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaystubsController
    {
        private readonly PaystubService _service;

        public PaystubsController(PaystubService service)
        {
            _service = service;
        }

        [HttpGet("{id}/adjustments")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<ICollection<AdjustmentDto>>> GetAdjustments(int id)
        {
            return await _service.GetAdjustments(id);
        }

        [HttpGet("{id}/loans")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<ICollection<LoanTransactionDto>>> GetLoanTransactions(int id)
        {
            return await _service.GetLoanTransactions(id);
        }
    }
}
