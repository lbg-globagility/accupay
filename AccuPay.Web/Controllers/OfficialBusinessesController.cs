using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.OfficialBusinesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfficialBusinessesController : ApiControllerBase
    {
        private readonly OfficialBusinessService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OfficialBusinessesController(OfficialBusinessService service, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.OfficialBusinessRead)]
        public async Task<ActionResult<PaginatedList<OfficialBusinessDto>>> List([FromQuery] PageOptions options, [FromQuery] OfficialBusinessFilter filter)
        {
            return await _service.PaginatedList(options, filter);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.OfficialBusinessRead)]
        public async Task<ActionResult<OfficialBusinessDto>> GetById(int id)
        {
            var officalBusiness = await _service.GetById(id);

            if (officalBusiness == null)
                return NotFound();
            else
                return officalBusiness;
        }

        [HttpPost]
        [Permission(PermissionTypes.OfficialBusinessCreate)]
        public async Task<ActionResult<OfficialBusinessDto>> Create([FromBody] CreateOfficialBusinessDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.OfficialBusinessUpdate)]
        public async Task<ActionResult<OfficialBusinessDto>> Update(int id, [FromBody] UpdateOfficialBusinessDto dto)
        {
            var officalBusiness = await _service.Update(id, dto);

            if (officalBusiness == null)
                return NotFound();
            else
                return officalBusiness;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.OfficialBusinessDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var officalBusiness = await _service.GetById(id);

            if (officalBusiness == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.OfficialBusinessRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }

        [HttpGet("accupay-official-business-template")]
        [Permission(PermissionTypes.OfficialBusinessRead)]
        public ActionResult GetOfficialBusTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-officialbus-template.xlsx");
        }
    }
}
