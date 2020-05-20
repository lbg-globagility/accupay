using AccuPay.Data.Helpers;
using AccuPay.Web.Organizations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _service;

        public OrganizationsController(OrganizationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<OrganizationDto>>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<OrganizationDto>> Update(int id, [FromBody] UpdateOrganizationDto dto)
        {
            return await _service.Update(id, dto);
        }
    }
}
