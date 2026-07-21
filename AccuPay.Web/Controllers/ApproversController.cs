using AccuPay.Core.Helpers;
using AccuPay.Web.Appraisers;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApproversController : ControllerBase
    {
        private readonly ApproverService _service;

        public ApproversController(ApproverService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.ApproverRead)]
        public async Task<ActionResult<PaginatedList<ApproverDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.ApproverRead)]
        public async Task<ActionResult<ApproverDto>> GetById(int id)
        {
            var dto = await _service.GetById(id);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        [Permission(PermissionTypes.ApproverCreate)]
        public async Task<ActionResult<ApproverDto>> Create([FromBody] CreateApproverDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.ApproverUpdate)]
        public async Task<ActionResult<ApproverDto>> Update(int id, [FromBody] UpdateApproverDto dto)
        {
            var result = await _service.Update(id, dto);

            if (result == null)
                return NotFound();

            return result;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.ApproverDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.GetById(id);
            if (existing == null) return NotFound();

            await _service.SetAsInactive(id);

            return Ok();
        }
    }
}
