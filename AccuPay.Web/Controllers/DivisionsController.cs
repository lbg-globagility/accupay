using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Divisions;
using AccuPay.Web.Divisions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DivisionsController : ControllerBase
    {
        private readonly DivisionService _service;

        public DivisionsController(DivisionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.DivisionRead)]
        public async Task<ActionResult<PaginatedList<DivisionDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.DivisionRead)]
        public async Task<ActionResult<DivisionDto>> GetById(int id)
        {
            var division = await _service.GetById(id);

            if (division == null)
                return NotFound();
            else
                return division;
        }

        [HttpGet("parents")]
        [Permission(PermissionTypes.DivisionRead)]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetAllParents()
        {
            var parents = await _service.GetAllParents();

            if (parents == null)
                return NotFound();
            else
                return Ok(parents);
        }

        [HttpGet("types")]
        [Permission(PermissionTypes.DivisionRead)]
        public ActionResult<IEnumerable<string>> GetTypes()
        {
            var types = _service.GetTypes();

            if (types == null)
                return NotFound();
            else
                return Ok(types);
        }

        [HttpGet("schedules")]
        [Permission(PermissionTypes.DivisionRead)]
        public async Task<ActionResult<IEnumerable<string>>> GetSchedules()
        {
            var schedules = await _service.GetSchedules();

            if (schedules == null)
                return NotFound();
            else
                return Ok(schedules);
        }

        [HttpPost]
        [Permission(PermissionTypes.DivisionCreate)]
        public async Task<ActionResult<DivisionDto>> Create([FromBody] CreateDivisionDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.DivisionUpdate)]
        public async Task<ActionResult<DivisionDto>> Update(int id, [FromBody] UpdateDivisionDto dto)
        {
            var division = await _service.Update(id, dto);

            if (division == null)
                return NotFound();
            else
                return division;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.DivisionDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }
    }
}
