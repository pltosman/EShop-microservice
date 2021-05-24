using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Core.Helpers
{

    public class TokenHelper
    {
        private readonly AppSettings _appSettings;

        public TokenHelper(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public AccessToken CreateToken(Guid customerId)
        {
            string token = GenerateJwtToken(customerId.ToString(), out DateTime tokenExpire);
            string refreshToken = GenerateRefreshToken();

            AccessToken model = new()
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpire = tokenExpire
            };

            return model;
        }

        private string GenerateJwtToken(string customerId, out DateTime tokenExpire)
        {
            tokenExpire = DateTime.Now.AddMinutes(_appSettings.TokenSettings.JwtTokenLifeTime);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _appSettings.TokenSettings.JwtTokenAudience,
                Issuer = _appSettings.TokenSettings.JwtTokenIssuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("CustomerId", customerId)
                }),
                Expires = tokenExpire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
