using AccuPay.Web.Branches;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchesController : ControllerBase
    {
        private readonly BranchService _service;

        public BranchesController(BranchService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.BranchRead)]
        public async Task<ActionResult<BranchDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        [Permission(PermissionTypes.BranchCreate)]
        public async Task<ActionResult<BranchDto>> Create([FromBody] CreateBranchDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.BranchUpdate)]
        public async Task<ActionResult<BranchDto>> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            return await _service.Update(id, dto);
        }

        [HttpGet]
        [Permission(PermissionTypes.BranchRead)]
        public async Task<ActionResult<ICollection<BranchDto>>> List()
        {
            var dtos = await _service.List();

            return dtos.ToList();
        }
    }
}
