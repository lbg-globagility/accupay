using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Positions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController
    {
        private readonly PositionService _service;
        private readonly PositionRepository _repository;

        public PositionsController(PositionService service, PositionRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<PositionDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }
    }
}
