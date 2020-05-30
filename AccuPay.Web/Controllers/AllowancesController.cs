using AccuPay.Data.Helpers;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Allowances.Services;
using AccuPay.Web.Products;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowancesController : ControllerBase
    {
        private readonly AllowanceService _service;

        public AllowancesController(AllowanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<AllowanceDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AllowanceDto>> GetById(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpPost]
        public async Task<ActionResult<AllowanceDto>> Create([FromBody] CreateAllowanceDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AllowanceDto>> Update(int id, [FromBody] UpdateAllowanceDto dto)
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
            var allowance = await _service.GetById(id);

            if (allowance == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<ICollection<ProductDto>>> GetAllowanceTypes()
        {
            return await _service.GetAllowanceTypes();
        }

        [HttpGet("frequencylist")]
        public ActionResult<ICollection<string>> GetFrequencyList()
        {
            return _service.GetFrequencyList();
        }
    }
}
