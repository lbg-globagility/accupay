using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.EmailTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailTemplatesController : ControllerBase
    {
        private readonly EmailTemplateService _emailTemplateService;
        public EmailTemplatesController(EmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }
       
        [HttpGet("{code}")]
        [Permission(PermissionTypes.EmailTemplateRead)]
        public async Task<EmailTemplateDto> Get(string code)
        {
            return await _emailTemplateService.GetByCode(code);
        }
        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmailTemplateUpdate)]
        public async Task<IActionResult> Put(int id, [FromBody] EmailTemplateDto value)
        {
            var updated = await _emailTemplateService.Update(id, value);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
    }
}
