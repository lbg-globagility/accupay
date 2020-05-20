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
        public async Task<ActionResult<PaginatedList<OrganizationDto>>> List()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<OrganizationDto>> Update([FromBody] UpdateOrganizationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
