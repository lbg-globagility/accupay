using AccuPay.Web.Account;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserService _userService;
        private readonly UserTokenService _userTokenService;

        public AccountController(AccountService accountService,
                                 UserService userService,
                                 UserTokenService userTokenService)
        {
            _accountService = accountService;
            _userService = userService;
            _userTokenService = userTokenService;
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

        [HttpGet("verify")]
        public async Task<ActionResult> Verify([FromQuery] string token)
        {
            var claims = _userTokenService.DecodeRegistrationToken(token);
            _ = await _accountService.Verify(claims.UserId);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] VerifyRegistrationDto dto)
        {
            var userDto = await _accountService.Register(dto);

            return userDto;
        }
    }
}
