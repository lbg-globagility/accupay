using AccuPay.Data.Helpers;
using AccuPay.Web.AllowanceType;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllowanceTypesController : ControllerBase
    {
        private readonly AllowanceTypeService _service;

        public AllowanceTypesController(AllowanceTypeService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.AllowanceTypeRead)]
        public async Task<ActionResult<AllowanceTypeDto>> GetById(int id)
        {
            var allowanceType = await _service.GetByIdAsync(id);

            if (allowanceType == null)
                return NotFound();

            return AllowanceTypeDto.Convert(allowanceType);
        }

        [HttpPost]
        [Permission(PermissionTypes.AllowanceTypeCreate)]
        public async Task<ActionResult<AllowanceTypeDto>> CreateAsync([FromBody] AllowanceTypeDto dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.AllowanceTypeUpdate)]
        public async Task<ActionResult<AllowanceTypeDto>> Update([FromBody] AllowanceTypeDto dto, int id)
        {
            if (id != dto.Id)
                return BadRequest();

            var allowanceTypeDto = await _service.UpdateAsync(id, dto);

            if (allowanceTypeDto == null)
                return NotFound();

            return allowanceTypeDto;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.AllowanceTypeDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var allowanceType = await _service.GetByIdAsync(id);

            if (allowanceType == null)
                return NotFound();

            await _service.DeleteAsync(id);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<AllowanceTypeDto>>> GetPaginatedList([FromQuery] PageOptions options, [FromQuery] string term = "")
        {
            return await _service.GetPaginatedListAsync(options, term);
        }
    }
}
