using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Allowances.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    class AllowancesController : ControllerBase
    {
        private readonly AllowanceService _allowanceService;

        public AllowancesController(AllowanceService allowanceService)
        {
            _allowanceService = allowanceService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AllowanceDto dto)
        {
            try
            {
                var allowance = await _allowanceService.Create(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AllowanceDto dto)
        {
            try
            {
                await _allowanceService.Update(id, dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] AllowanceDto dto)
        {
            try
            {
                await _allowanceService.Delete(id, dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AllowanceDto>> GeyById(int id)
        {
            var dto = await _allowanceService.GeyByIdAsync(id);

            return dto;
        }
    }
}
