using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/users")]
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
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var userDto = await _service.Update(id, dto);

            return userDto;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserDto>>> List([FromForm] PageOptions options, string term)
        {
            var (users, count) = await _repository.List(options, term);

            var dtos = users.Select(t =>
                new UserDto()
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email
                }
            );

            return new PaginatedList<UserDto>(dtos, count, 1, 1);
        }
    }
}
