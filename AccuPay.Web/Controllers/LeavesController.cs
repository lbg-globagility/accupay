using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<PaginatedList<LeaveDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
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

        [HttpGet("types")]
        public async Task<ActionResult<ICollection<string>>> GetLeaveTypes()
        {
            return await _service.GetLeaveTypes();
        }

        [HttpGet("statuslist")]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        [HttpGet("ledger")]
        public async Task<ActionResult<PaginatedList<LeaveBalanceDto>>> GetLeaveBalance([FromQuery] PageOptions options, string term)
        {
            return await _service.GetLeaveBalance(options, term);
        }


        [HttpGet("ledger/{id}")]
        public async Task<ActionResult<PaginatedList<LeaveLedgerDto>>> GetLedger([FromQuery] PageOptions options,  string type, int id)
        {
            return await _service.PaginatedListLedger(options, id, type);
        }


    }
}
