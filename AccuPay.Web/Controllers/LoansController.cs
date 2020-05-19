using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Loans;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _service;
        private readonly LoanScheduleRepository _repository;

        public LoansController(LoanService service, LoanScheduleRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<LoanDto>>> List([FromForm] PageOptions options, string searchTerm)
        {
            return await _service.PaginatedList(options, searchTerm);
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
            var officalBusiness = await _repository.GetByIdAsync(id);

            if (officalBusiness == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
