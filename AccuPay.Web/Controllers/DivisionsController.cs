using AccuPay.Data.Helpers;
using AccuPay.Web.Divisions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController
    {
        private readonly DivisionService _service;

        public DivisionsController(DivisionService allowanceService)
        {
            _service = allowanceService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<DivisionDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }
    }
}
