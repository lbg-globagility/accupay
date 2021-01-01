using AccuPay.Core.Helpers;
using AccuPay.Web.Clients;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientsController(ClientService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.ClientRead)]
        public async Task<ActionResult<PaginatedList<ClientDto>>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.ClientRead)]
        public async Task<ActionResult<ClientDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        [Permission(PermissionTypes.ClientCreate)]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.ClientUpdate)]
        public async Task<ActionResult<ClientDto>> Update(int id, [FromBody] UpdateClientDto dto)
        {
            return await _service.Update(id, dto);
        }
    }
}
