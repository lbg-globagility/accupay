using AccuPay.Web.Account;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccessTokenDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _accountService.Login(dto.Email, dto.Password);

                return new AccessTokenDto() { Token = token };
            }
            catch (LoginException ex)
            {
                return BadRequest(new { ErrorType = ex.Message });
            }
        }
    }
}
