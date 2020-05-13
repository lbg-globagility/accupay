using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                        BranchRepository branchRepository)
        {
            _logger = logger;
            _branchRepository = branchRepository;
        }

        [HttpGet]
        public IEnumerable<Branch> Get()
        {
            return _branchRepository.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Branch branch)
        {
            await _branchRepository.CreateAsync(branch);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Branch branch)
        {
            await _branchRepository.UpdateAsync(branch);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Branch branch)
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
