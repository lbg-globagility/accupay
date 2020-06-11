using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
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
        [Permission(PermissionTypes.RoleCreate)]
        public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto dto)
        {
            return await _roleService.Create(dto);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.RoleRead)]
        public async Task<ActionResult<RoleDto>> GetById(Guid id)
        {
            return await _roleService.GetById(id);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.RoleUpdate)]
        public async Task<ActionResult<RoleDto>> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            return await _roleService.Update(id, dto);
        }

        [HttpGet("user-roles")]
        [Permission(PermissionTypes.RoleRead)]
        public async Task<ActionResult<ICollection<UserRoleDto>>> GetUserRoles()
        {
            var dtos = await _roleService.GetUserRoles();
            return dtos.ToList();
        }

        [HttpPut("user-roles")]
        [Permission(PermissionTypes.RoleUpdate)]
        public async Task<ActionResult> UpdateUserRoles([FromBody] ICollection<UpdateUserRoleDto> dtos)
        {
            await _roleService.UpdateUserRoles(dtos);

            return Ok();
        }

        [HttpGet]
        [Permission(PermissionTypes.RoleRead)]
        public async Task<ActionResult<PaginatedList<RoleDto>>> List([FromQuery] PageOptions options)
        {
            return await _roleService.List(options);
        }
    }
}
