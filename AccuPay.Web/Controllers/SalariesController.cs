using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Salaries.Models;
using AccuPay.Web.Salaries.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly SalaryService _service;
        private readonly SalaryRepository _repository;

        public SalariesController(SalaryService salaryService, SalaryRepository repository)
        {
            _service = salaryService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<SalaryDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryDto>> GetById(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpPost]
        public async Task<ActionResult<SalaryDto>> Create([FromBody] CreateSalaryDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalaryDto>> Update(int id, [FromBody] UpdateSalaryDto dto)
        {
            var allowance = await _service.Update(id, dto);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var allowance = await _repository.GetByIdAsync(id);

            if (allowance == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
