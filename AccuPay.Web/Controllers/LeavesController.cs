using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _service;
        private readonly LeaveRepository _repository;

        public LeavesController(LeaveService leaveService, LeaveRepository repository)
        {
            _service = leaveService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<LeaveDto>>> List([FromForm] PageOptions options, string searchTerm)
        {
            return await _service.PaginatedList(options, searchTerm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveDto>> GetById(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveDto>> Create([FromBody] CreateLeaveDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LeaveDto>> Update(int id, [FromBody] UpdateLeaveDto dto)
        {
            var leave = await _service.Update(id, dto);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _repository.GetByIdAsync(id);

            if (leave == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
