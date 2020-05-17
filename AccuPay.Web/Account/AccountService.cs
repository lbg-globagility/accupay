using AccuPay.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AccuPay.Web.Account
{
    public class AccountService
    {
        private readonly UserManager<AspNetUser> _users;
        private readonly SignInManager<AspNetUser> _signIn;
        private readonly AccountTokenService _tokens;

        public AccountService(UserManager<AspNetUser> users,
                              SignInManager<AspNetUser> signIn,
                              AccountTokenService tokens)
        {
            _users = users;
            _signIn = signIn;
            _tokens = tokens;
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _users.FindByNameAsync(username);
            var result = await _signIn.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                throw LoginException.CredentialsMismatch();
            }

            var token = _tokens.CreateAccessToken(user);

            return token;
        }
    }
}
