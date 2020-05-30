using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BranchRepository _branchRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly DivisionRepository _divisionRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                        BranchRepository branchRepository,
                                        EmployeeRepository employeeRepository,
                                        DivisionRepository divisionRepository)
        {
            _logger = logger;
            _branchRepository = branchRepository;
            this._employeeRepository = employeeRepository;
            this._divisionRepository = divisionRepository;
        }

        [HttpGet("employees")]
        public async Task<IEnumerable<Employee>> Employees()
        {
            int organizationId = 2;
            return await _employeeRepository.GetAllActiveAsync(organizationId);
        }

        [HttpGet("employees/{employeeNumber}")]
        public async Task<Employee> GetEmployee(string employeeNumber)
        {
            int organizationId = 2;
            return await _employeeRepository.GetByEmployeeNumberAsync(employeeNumber, organizationId);
        }

        [HttpGet]
        public IEnumerable<Division> Get()
        {
            //return _branchRepository.GetAll();
            int organizationId = 2;
            return _divisionRepository.GetAll(organizationId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, Branch branch)
        {
            await _branchRepository.DeleteAsync(branch);
            return Ok();
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
