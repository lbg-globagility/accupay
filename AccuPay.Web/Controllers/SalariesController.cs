using AccuPay.Web.Salaries.Models;
using AccuPay.Web.Salaries.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    class SalariesController : ControllerBase
    {
        private readonly SalaryService _salaryService;

        public SalariesController(SalaryService branchRepository)
        {
            _salaryService = branchRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SalaryDto dto)
        {
            try
            {
                var salary = await _salaryService.Create(dto);
                return Ok(salary.RowID);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] SalaryDto dto)
        {
            try
            {
                await _salaryService.Update(id, dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] SalaryDto dto)
        {
            try
            {
                await _salaryService.Delete(id, dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryDto>> GeyById(int id)
        {
            var dto = await _salaryService.GeyByIdAsync(id);

            return dto;
        }
    }
}
