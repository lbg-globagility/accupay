using AccuPay.Data.Helpers;
using AccuPay.Web.OfficialBusinesses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficialBusinessesController : ControllerBase
    {
        private readonly OfficialBusinessService _service;

        public OfficialBusinessesController(OfficialBusinessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<OfficialBusinessDto>>> List([FromQuery] PageOptions options, [FromQuery] OfficialBusinessFilter filter)
        {
            return await _service.PaginatedList(options, filter);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfficialBusinessDto>> GetById(int id)
        {
            var officalBusiness = await _service.GetById(id);

            if (officalBusiness == null)
                return NotFound();
            else
                return officalBusiness;
        }

        [HttpPost]
        public async Task<ActionResult<OfficialBusinessDto>> Create([FromBody] CreateOfficialBusinessDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OfficialBusinessDto>> Update(int id, [FromBody] UpdateOfficialBusinessDto dto)
        {
            var officalBusiness = await _service.Update(id, dto);

            if (officalBusiness == null)
                return NotFound();
            else
                return officalBusiness;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var officalBusiness = await _service.GetById(id);

            if (officalBusiness == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("statuslist")]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }
    }
}
