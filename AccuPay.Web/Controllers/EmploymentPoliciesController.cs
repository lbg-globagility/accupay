using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.EmploymentPolicies.Models;
using AccuPay.Web.EmploymentPolicies.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/employment-policies")]
    [ApiController]
    [Authorize]
    public class EmploymentPoliciesController : ControllerBase
    {
        private readonly EmploymentPolicyService _service;

        public EmploymentPoliciesController(EmploymentPolicyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Permission(PermissionTypes.EmploymentPolicyRead)]
        public async Task<EmploymentPolicyDto> GetById(int id)
        {
            return await _service.GetById(id);
        }

        /// <summary>
        /// Create employment policy
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermissionTypes.EmploymentPolicyCreate)]
        public async Task<EmploymentPolicyDto> Create([FromBody] CreateEmploymentPolicyDto dto)
        {
            return await _service.Create(dto);
        }

        /// <summary>
        /// Update employment policy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmploymentPolicyUpdate)]
        public async Task<EmploymentPolicyDto> Update(int id, [FromBody] UpdateEmploymentPolicyDto dto)
        {
            return await _service.Update(id, dto);
        }

        /// <summary>
        /// List all employment policies
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        [Permission(PermissionTypes.EmploymentPolicyRead)]
        public async Task<PaginatedList<EmploymentPolicyDto>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }
    }
}
