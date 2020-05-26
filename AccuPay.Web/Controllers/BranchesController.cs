using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        public BranchesController()
        {
        }

        [HttpGet("{id}"]
        public async Task<ActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            throw new NotImplementedException();
        }
    }
}
