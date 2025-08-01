using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Permission(PermissionTypes.UserCreate)]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var userDto = await _service.Create(dto);

            return userDto;
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.UserRead)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var userDto = await _service.GetById(id);

            return userDto;
        }

        [HttpPost("{id}")]
        [Permission(PermissionTypes.UserUpdate)]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var userDto = await _service.Update(id, dto);

            return userDto;
        }

        [HttpGet]
        [Permission(PermissionTypes.UserRead)]
        public async Task<ActionResult<PaginatedList<UserDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.List(options, term);
        }

        [HttpGet("user-image")]
        [Permission(PermissionTypes.UserRead)]
        public async Task<ActionResult> GenerateUsersImage()
        {
            await _service.GenerateUserImages();

            return Ok();
        }
    }
}
