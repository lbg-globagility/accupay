using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Divisions;
using AccuPay.Web.Divisions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly DivisionService _service;
        private readonly DivisionRepository _repository;

        public DivisionsController(DivisionService divisionService, DivisionRepository repository)
        {
            _service = divisionService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<DivisionDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDto>> GetById(int id)
        {
            var division = await _service.GetById(id);

            if (division == null)
                return NotFound();
            else
                return division;
        }

        [HttpGet("parents")]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetAllParents()
        {
            var parents = await _service.GetAllParents();

            if (parents == null)
                return NotFound();
            else
                return Ok(parents);
        }

        [HttpGet("types")]
        public  ActionResult<IEnumerable<string>> GetTypes()
        {
            var types =  _service.GetTypes();

            if (types == null)
                return NotFound();
            else
                return Ok(types);
        }

        [HttpGet("schedules")]
        public async Task<ActionResult<IEnumerable<string>>> GetSchedules()
        {
            var schedules = await _service.GetSchedules();

            if (schedules == null)
                return NotFound();
            else
                return Ok(schedules);
        }



        [HttpPost]
        public async Task<ActionResult<DivisionDto>> Create([FromBody] CreateDivisionDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DivisionDto>> Update(int id, [FromBody] UpdateDivisionDto dto)
        {
            var division = await _service.Update(id, dto);

            if (division == null)
                return NotFound();
            else
                return division;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _repository.GetByIdAsync(id);

            if (leave == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
