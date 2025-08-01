using AccuPay.Core.Helpers;
using AccuPay.Core.Services.Imports.Allowances;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Allowances.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllowancesController : ApiControllerBase
    {
        private readonly AllowanceService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AllowancesController(AllowanceService service, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.AllowanceRead)]
        public async Task<ActionResult<PaginatedList<AllowanceDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.AllowanceRead)]
        public async Task<ActionResult<AllowanceDto>> GetById(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpPost]
        [Permission(PermissionTypes.AllowanceCreate)]
        public async Task<ActionResult<AllowanceDto>> Create([FromBody] CreateAllowanceDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.AllowanceUpdate)]
        public async Task<ActionResult<AllowanceDto>> Update(int id, [FromBody] UpdateAllowanceDto dto)
        {
            var allowance = await _service.Update(id, dto);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.AllowanceDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        [Permission(PermissionTypes.AllowanceRead)]
        public async Task<ActionResult<ICollection<ProductDto>>> GetAllowanceTypes()
        {
            return await _service.GetAllowanceTypes();
        }

        [HttpGet("frequencylist")]
        [Permission(PermissionTypes.AllowanceRead)]
        public ActionResult<ICollection<string>> GetFrequencyList()
        {
            return _service.GetFrequencyList();
        }

        [HttpGet("accupay-allowance-template")]
        [Permission(PermissionTypes.AllowanceRead)]
        public ActionResult GetAllowanceTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-allowance-template.xlsx");
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.AllowanceCreate)]
        public async Task<AllowanceImportParserOutput> Import([FromForm] IFormFile file)
        {
            return await _service.Import(file);
        }
    }
}
