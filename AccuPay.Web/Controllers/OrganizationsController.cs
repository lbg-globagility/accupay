using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Organizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _service;

        public OrganizationsController(OrganizationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.OrganizationRead)]
        public async Task<ActionResult<PaginatedList<OrganizationDto>>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.OrganizationRead)]
        public async Task<ActionResult<OrganizationDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        [Permission(PermissionTypes.OrganizationCreate)]
        public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPost("{id}")]
        [Permission(PermissionTypes.OrganizationUpdate)]
        public async Task<ActionResult<OrganizationDto>> Update(int id, [FromBody] UpdateOrganizationDto dto)
        {
            return await _service.Update(id, dto);
        }
    }
}
