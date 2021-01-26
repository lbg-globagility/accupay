using AccuPay.Web.Core.Auth;
using AccuPay.Web.Users;
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
    public class PermissionsController : ControllerBase
    {
        private readonly PermissionService _service;

        public PermissionsController(PermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.RoleRead)]
        public async Task<ActionResult<ICollection<PermissionDto>>> GetAll()
        {
            var dtos = await _service.GetAll();
            return dtos.ToList();
        }
    }
}
