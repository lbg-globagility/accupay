using AccuPay.Web.Core.Emails;
using AccuPay.Web.EmailTemplates;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplatesController : ControllerBase
    {
        private readonly EmailTemplateService _emailTemplateService;
        public EmailTemplatesController(EmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }
       
        [HttpGet("{code}")]
        public async Task<EmailTemplateDto> Get(string code)
        {
            return await _emailTemplateService.GetByCode(code);
        }
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
