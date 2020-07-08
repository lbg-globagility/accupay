using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Loans;
using AccuPay.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoansController : ApiControllerBase
    {
        private readonly LoanService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LoansController(LoanService service, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult<PaginatedList<LoanDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult<LoanDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        [Permission(PermissionTypes.LoanCreate)]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.LoanUpdate)]
        public async Task<ActionResult<LoanDto>> Update(int id, [FromBody] UpdateLoanDto dto)
        {
            var leave = await _service.Update(id, dto);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.LoanDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var loan = await _service.GetById(id);

            if (loan == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult<ICollection<DropDownItem>>> GetLoanTypes()
        {
            return await _service.GetLoanTypes();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.LoanRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }

        [HttpGet("deductionsechedules")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult<ICollection<string>>> GetDeductionSchedules()
        {
            return await _service.GetDeductionSchedules();
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult<PaginatedList<LoanHistoryDto>>> GetLoanHistory([FromQuery] PageOptions options, int id)
        {
            return await _service.GetLoanHistory(options, id);
        }

        [HttpGet("accupay-loan-template")]
        [Permission(PermissionTypes.LoanRead)]
        public ActionResult GetLoanTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-loan-template.xlsx");
        }
    }
}
