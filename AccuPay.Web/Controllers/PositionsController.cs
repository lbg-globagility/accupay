using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Positions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PositionsController : ControllerBase
    {
        private readonly PositionService _service;

        public PositionsController(PositionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.PositionRead)]
        public async Task<ActionResult<PaginatedList<PositionDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpPost]
        [Permission(PermissionTypes.PositionCreate)]
        public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.PositionUpdate)]
        public async Task<ActionResult<PositionDto>> Update(int id, [FromBody] UpdatePositionDto dto)
        {
            var overtime = await _service.Update(id, dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.PositionRead)]
        public async Task<ActionResult<PositionDto>> GetById(int id)
        {
            var position = await _service.GetById(id);

            if (position == null)
                return NotFound();
            else
                return position;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.PositionDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var position = await _service.GetById(id);

            if (position == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }
    }
}
