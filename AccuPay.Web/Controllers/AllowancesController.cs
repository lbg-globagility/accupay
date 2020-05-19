using AccuPay.Data.Repositories;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Allowances.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowancesController : ControllerBase
    {
        private readonly AllowanceService _allowanceService;
        private readonly AllowanceRepository _repository;

        public AllowancesController(AllowanceService allowanceService, AllowanceRepository repository)
        {
            _allowanceService = allowanceService;
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AllowanceDto>> GetById(int id)
        {
            var allowance = await _allowanceService.GetByIdAsync(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpPost]
        public async Task<ActionResult<AllowanceDto>> Create([FromBody] CreateAllowanceDto dto)
        {
            try
            {
                return await _allowanceService.Create(dto);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AllowanceDto>> Update(int id, [FromBody] UpdateAllowanceDto dto)
        {
            try
            {
                var allowance = await _allowanceService.Update(id, dto);

                if (allowance == null)
                    return NotFound();
                else
                    return allowance;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var allowance = await _repository.GetByIdAsync(id);

                if (allowance == null) return NotFound();

                await _repository.DeleteAsync(id);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
