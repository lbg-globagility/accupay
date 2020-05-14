using AccuPay.Web.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var userDto = await _service.Create(dto);

            return userDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Update()
        {
            throw new NotImplementedException();
        }
    }
}
