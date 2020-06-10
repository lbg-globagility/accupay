using AccuPay.Data.Helpers;
using AccuPay.Web.Loans;
using AccuPay.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _service;

        public LoansController(LoanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<LoanDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LoanDto>> Update(int id, [FromBody] UpdateLoanDto dto)
        {
            var leave = await _service.Update(id, dto);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var loan = await _service.GetById(id);

            if (loan == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<ICollection<DropDownItem>>> GetLoanTypes()
        {
            return await _service.GetLoanTypes();
        }

        [HttpGet("statuslist")]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }

        [HttpGet("deductionsechedules")]
        public async Task<ActionResult<ICollection<string>>> GetDeductionSchedules()
        {
            return await _service.GetDeductionSchedules();
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult<PaginatedList<LoanHistoryDto>>> GetLoanHistory([FromQuery] PageOptions options, int id)
        {
            return await _service.GetLoanHistory(options, id);
        }
    }
}
