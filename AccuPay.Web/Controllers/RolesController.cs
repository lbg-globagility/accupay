using AccuPay.Data.Helpers;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto dto)
        {
            return await _roleService.Create(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(Guid id)
        {
            return await _roleService.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            return await _roleService.Update(id, dto);
        }

        [HttpGet("user-roles")]
        public async Task<ActionResult<ICollection<UserRoleDto>>> GetUserRoles()
        {
            var dtos = await _roleService.GetUserRoles();
            return dtos.ToList();
        }

        [HttpPut("user-roles")]
        public async Task<ActionResult> UpdateUserRoles([FromBody] ICollection<UpdateUserRoleDto> dtos)
        {
            await _roleService.UpdateUserRoles(dtos);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<RoleDto>>> List([FromQuery] PageOptions options)
        {
            return await _roleService.List(options);
        }
    }
}
