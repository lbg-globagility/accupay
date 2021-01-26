using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.ShiftImportParser;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftsController : ApiControllerBase
    {
        private readonly ShiftService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ShiftsController(ShiftService shiftService, IHostingEnvironment hostingEnvironment)
        {
            _service = shiftService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPut]
        [Permission(PermissionTypes.ShiftUpdate)]
        public async Task<ActionResult<ShiftDto>> Update([FromBody] ICollection<ShiftDto> dtos)
        {
            await _service.BatchApply(dtos);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.ShiftCreate)]
        public async Task<ShiftImportParserOutput> Import([FromForm] IFormFile file)
        {
            return await _service.Import(file);

            // return Ok();
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.ShiftRead)]
        public async Task<ActionResult<PaginatedList<EmployeeShiftsDto>>> ListByEmployee(
            [FromQuery] ShiftsByEmployeePageOptions options)
        {
            return await _service.ListByEmployee(options);
        }

        [HttpGet("accupay-shiftschedule-template")]
        [Permission(PermissionTypes.ShiftCreate)]
        public ActionResult GetEmployeeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-shiftschedule-template.xlsx");
        }
    }
}
