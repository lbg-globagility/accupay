using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.OfficialBusinesses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficialBusinessesController : ControllerBase
    {
        private readonly OfficialBusinessService _service;
        private readonly OfficialBusinessRepository _repository;

        public OfficialBusinessesController(OfficialBusinessService service, OfficialBusinessRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<OfficialBusinessDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
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
            var officalBusiness = await _repository.GetByIdAsync(id);

            if (officalBusiness == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
