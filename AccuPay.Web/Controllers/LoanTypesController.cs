using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Loans.LoanType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoanTypesController : ControllerBase
    {
        private readonly LoanTypeService _service;

        public LoanTypesController(LoanTypeService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult<LoanTypeDto>> GetById(int id)
        {
            var loanType = await _service.GetByIdAsync(id);

            if (loanType == null)
                return NotFound();

            return LoanTypeDto.Convert(loanType);
        }

        [HttpPost]
        [Permission(PermissionTypes.LoanCreate)]
        public async Task<ActionResult<LoanTypeDto>> Create([FromBody] LoanTypeDto dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.LoanUpdate)]
        public async Task<ActionResult<LoanTypeDto>> Update([FromBody] LoanTypeDto dto, int id)
        {
            if (id != dto.Id)
                return BadRequest();

            await _service.UpdateAsync(id, dto);

            return dto;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.LoanDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var loanType = await _service.GetByIdAsync(id);

            if (loanType == null)
                return NotFound();

            await _service.DeleteAsync(id);

            return Ok();
        }

        [HttpGet]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<PaginatedList<LoanTypeDto>> GetPaginatedList([FromQuery] PageOptions options, [FromQuery] string term = "")
        {
            var result = await _service.GetPaginatedListAsync(options, term);

            return result;
        }
    }
}
