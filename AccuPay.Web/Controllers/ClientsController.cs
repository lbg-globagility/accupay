using AccuPay.Data.Helpers;
using AccuPay.Web.Clients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientsController(ClientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ClientDto>>> List([FromQuery] PageOptions options)
        {
            return await _service.List(options);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDto>> Update(int id, [FromBody] UpdateClientDto dto)
        {
            return await _service.Update(id, dto);
        }
    }
}
