using AccuPay.Data.Helpers;
using AccuPay.Web.Branches;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly BranchService _service;

        public BranchesController(BranchService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<BranchDto>> Create([FromBody] CreateBranchDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BranchDto>> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            return await _service.Update(id, dto);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<BranchDto>>> List([FromQuery] PageOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
