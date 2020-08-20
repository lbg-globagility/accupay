using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Overtimes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OvertimesController : ApiControllerBase
    {
        private readonly OvertimeService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OvertimesController(OvertimeService service, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.OvertimeRead)]
        public async Task<ActionResult<PaginatedList<OvertimeDto>>> List([FromQuery] PageOptions options, [FromQuery] OvertimeFilter filter)
        {
            return await _service.PaginatedList(options, filter);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.OvertimeRead)]
        public async Task<ActionResult<OvertimeDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        [Permission(PermissionTypes.OvertimeCreate)]
        public async Task<ActionResult<OvertimeDto>> Create([FromBody] CreateOvertimeDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.OvertimeUpdate)]
        public async Task<ActionResult<OvertimeDto>> Update(int id, [FromBody] UpdateOvertimeDto dto)
        {
            var overtime = await _service.Update(id, dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.OvertimeDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.OvertimeRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }

        [HttpGet("accupay-overtime-template")]
        [Permission(PermissionTypes.OvertimeRead)]
        public ActionResult GetOvertimeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-overtime-template.xlsx");
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.OvertimeCreate)]
        public async Task<Data.Services.Imports.Overtimes.OvertimeImportParserOutput> Import([FromForm] IFormFile file)
        {
            return await _service.Import(file);
        }
    }
}
