using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Salaries.Models;
using AccuPay.Web.Salaries.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Permission(PermissionTypes.SalaryRead)]
        public async Task<ActionResult<PaginatedList<SalaryDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.SalaryRead)]
        public async Task<ActionResult<SalaryDto>> GetById(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpPost]
        [Permission(PermissionTypes.SalaryCreate)]
        public async Task<ActionResult<SalaryDto>> Create([FromBody] CreateSalaryDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.SalaryUpdate)]
        public async Task<ActionResult<SalaryDto>> Update(int id, [FromBody] UpdateSalaryDto dto)
        {
            var allowance = await _service.Update(id, dto);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.SalaryDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var allowance = await _repository.GetByIdAsync(id);

            if (allowance == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
