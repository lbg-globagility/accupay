using AccuPay.Data.Repositories;
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
        private readonly AspNetUserRepository _repository;

        public UsersController(UserService service, AspNetUserRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var userDto = await _service.Create(dto);

            return userDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _repository.GetById(id);
            var dto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return dto;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Update()
        {
            throw new NotImplementedException();
        }
    }
}
