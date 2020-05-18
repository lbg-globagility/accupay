using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Overtimes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimesController : ControllerBase
    {
        private readonly OvertimeService _service;
        private readonly OvertimeRepository _repository;

        public OvertimesController(OvertimeService service, OvertimeRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<OvertimeDto>>> List([FromForm] PageOptions options, string searchTerm)
        {
            return await _service.PaginatedList(options, searchTerm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OvertimeDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        public async Task<ActionResult<OvertimeDto>> Create([FromBody] CreateOvertimeDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OvertimeDto>> Update(int id, [FromBody] UpdateOvertimeDto dto)
        {
            var overtime = await _service.Update(id, dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
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
