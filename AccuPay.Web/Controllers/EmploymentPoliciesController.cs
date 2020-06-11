using AccuPay.Data.Helpers;
using AccuPay.Web.EmploymentPolicies.Models;
using AccuPay.Web.EmploymentPolicies.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/employment-policies")]
    [ApiController]
    public class EmploymentPoliciesController : ControllerBase
    {
        private readonly EmploymentPolicyService _service;

        public EmploymentPoliciesController(EmploymentPolicyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<EmploymentPolicyDto> Create([FromBody] CreateEmploymentPolicyDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<EmploymentPolicyDto> Update(int id, [FromBody] UpdateEmploymentPolicyDto dto)
        {
            return await _service.Update(id, dto);
        }

        [HttpGet]
        public async Task<PaginatedList<EmploymentPolicyDto>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }
    }
}
