using AccuPay.Web.Account;
using AccuPay.Web.Organizations;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserTokenService _userTokenService;
        private readonly OrganizationService _organizationService;
        private readonly RoleService _roleService;

        public AccountController(AccountService accountService,
                                 UserTokenService userTokenService,
                                 OrganizationService organizationService,
                                 RoleService roleService)
        {
            _accountService = accountService;
            _userTokenService = userTokenService;
            _organizationService = organizationService;
            _roleService = roleService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
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

        [HttpPost("change-organization")]
        public async Task<ActionResult<AccessTokenDto>> ChangeOrganization([FromBody] ChangeOrganizationDto dto)
        {
            try
            {
                var token = await _accountService.ChangeOrganization(dto.OrganizationId);

                return new AccessTokenDto() { Token = token };
            }
            catch (LoginException ex)
            {
                return BadRequest(new { ErrorType = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetInformation()
        {
            var dto = await _accountService.GetInformation();

            return dto;
        }

        [HttpGet("current-role")]
        public async Task<ActionResult<RoleDto>> GetCurrentRole()
        {
            var dto = await _roleService.GetCurrentRole();

            return dto;
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

        [HttpGet("organization")]
        public async Task<ActionResult<OrganizationDto>> GetCurrentOrganization()
        {
            return await _organizationService.GetCurrentOrganization();
        }
    }
}
