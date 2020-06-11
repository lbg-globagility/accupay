using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Overtimes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimesController : ControllerBase
    {
        private readonly OvertimeService _service;

        public OvertimesController(OvertimeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.EmployeeOvertimeRead)]
        public async Task<ActionResult<PaginatedList<OvertimeDto>>> List([FromQuery] PageOptions options, [FromQuery] OvertimeFilter filter)
        {
            return await _service.PaginatedList(options, filter);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.EmployeeOvertimeRead)]
        public async Task<ActionResult<OvertimeDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        [Permission(PermissionTypes.EmployeeOvertimeCreate)]
        public async Task<ActionResult<OvertimeDto>> Create([FromBody] CreateOvertimeDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmployeeOvertimeUpdate)]
        public async Task<ActionResult<OvertimeDto>> Update(int id, [FromBody] UpdateOvertimeDto dto)
        {
            var overtime = await _service.Update(id, dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.EmployeeOvertimeDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.EmployeeOvertimeRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }
    }
}
