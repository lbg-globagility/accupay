using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeavesController : ApiControllerBase
    {
        private readonly LeaveService _service;
        private readonly LeaveRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LeavesController(LeaveService leaveService, LeaveRepository repository, IHostingEnvironment hostingEnvironment)
        {
            _service = leaveService;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveDto>>> List([FromQuery] PageOptions options, [FromQuery] LeaveFilter filter)
        {
            return await _service.PaginatedList(options, filter);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<LeaveDto>> GetById(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpPost]
        [Permission(PermissionTypes.LeaveCreate)]
        public async Task<ActionResult<LeaveDto>> Create([FromBody] CreateLeaveDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.LeaveUpdate)]
        public async Task<ActionResult<LeaveDto>> Update(int id, [FromBody] UpdateLeaveDto dto)
        {
            var leave = await _service.Update(id, dto);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.LeaveDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<ICollection<string>>> GetLeaveTypes()
        {
            return await _service.GetLeaveTypes();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.LeaveRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        [HttpGet("ledger")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveBalanceDto>>> GetLeaveBalance([FromQuery] PageOptions options, string term)
        {
            return await _service.GetLeaveBalance(options, term);
        }

        [HttpGet("ledger/{id}")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveTransactionDto>>> GetLedger([FromQuery] PageOptions options, string type, int id)
        {
            return await _service.ListTransactions(options, id, type);
        }

        [HttpGet("accupay-leave-template")]
        [Permission(PermissionTypes.LeaveRead)]
        public ActionResult GetLeaveTemplate()
        {
            return GetTemplate(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-leave-template.xlsx");
        }
    }
}
