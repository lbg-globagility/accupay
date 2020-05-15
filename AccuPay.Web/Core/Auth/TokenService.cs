using Accupay.Web.Core.Configurations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccuPay.Web.Core.Auth
{
    public class TokenService
    {
        private readonly JwtConfiguration _configuration;

        public TokenService(JwtConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ClaimsPrincipal Decode(string token)
        {
            var key = GetKey();

            var validationParameters = new TokenValidationParameters
            {
                // Clock skew compensates for server time drift.
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(5),
                // Specify the key used to sign the token:
                IssuerSigningKey = key,
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
                // Ensure the token audience matches our audience value (default true):
                ValidateAudience = true,
                ValidAudience = _configuration.Audience,
                // Ensure the token was issued by a trusted authorization server (default true):
                ValidateIssuer = true,
                ValidIssuer = _configuration.Issuer
            };

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validationParameters, out var rawValidatedToken);

                return claimsPrincipal;
            }
            catch (SecurityTokenValidationException ex)
            {
                // The token failed validation!
                // TODO: Log it or display an error.
                //throw new Exception($"Token failed validation: {stvex.Messagek}");
                return null;
            }
            catch (ArgumentException)
            {
                // The token was not well-formed or was invalid for some other reason.
                // TODO: Log it or display an error.
                //throw new Exception($"Token was invalid: {argex.Message}");
                return null;
            }
        }

        public string Encode(TimeSpan timeToExpire, IEnumerable<Claim> customClaims)
        {
            var token = Create(timeToExpire, customClaims);
            var serialized = Serialize(token);

            return serialized;
        }

        private JwtSecurityToken Create(TimeSpan timeToExpire, IEnumerable<Claim> claims)
        {
            var credentials = GetSigningCredentials();
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.Add(timeToExpire);

            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                signingCredentials: credentials,
                notBefore: notBefore,
                expires: expires,
                claims: claims);

            return token;
        }

        /// <summary>
        /// Serializes a JwtSecurityToken object into an encoded jwt token string.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private string Serialize(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = GetKey();
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private SecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        }
    }
}
