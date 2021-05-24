using EShop.Core.Helpers;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Model.Enums;
using EShop.Core.Model.ResponseModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Z.EntityFramework.Plus;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EShop.Core.Domain.Entities;

namespace EShop.Core.Services
{

    public class AccountService : IAccountService
    {
        private readonly IdentityContext _context;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _cache;

        public AccountService(IdentityContext context,
            IOptions<AppSettings> options,
            IDistributedCache cache)
        {
            _context = context;
            _appSettings = options.Value;
            _cache = cache;
        }

        public JsonWebToken CrateToken(Guid customerId)
        {
            JsonWebToken token = Create(customerId);

            _context.CustomerTokens
                .Where(x => x.CustomerId == customerId)
                .Delete();

            _context.CustomerTokens.Add(new CustomerToken
            {
                CustomerId = customerId,
                RefreshToken = token.RefreshToken,
                Token = token.Token,
                TokenExpire = token.TokenExpire
            });

            _context.SaveChanges();

            _cache.Set(customerId.ToString(), token);

            return token;
        }

        public CommandResult RefreshToken(Guid customerId, string token)
        {
            var jsonWebToken = _cache.GetT<JsonWebToken>(customerId.ToString()).Result;

            if (jsonWebToken == null)
            {
                var customerToken = _context.CustomerTokens.FirstOrDefault(x => x.RefreshToken == token && x.CustomerId == customerId);
                if (customerToken != null)
                {
                    jsonWebToken = new();
                }
            }

            if (jsonWebToken != null)
            {
                jsonWebToken = CrateToken(customerId);

                return CommandResult.GetSuccess(jsonWebToken, ResponseStatus.Success);
            }

            return new CommandResult
            {
                Status = ResponseStatus.Unauthorized
            };
        }

        public CommandResult LoginWithEmail(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_EMAIL_NOT_EMPTY);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_PASSWORD_NOT_EMPTY);
            }

            var customer = _context.Customers.FirstOrDefault(x => x.Email == email);
            if (customer == null)
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_NOTFOUND);
            }

            if (EncryptionHelper.VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
            {
                var token = CrateToken(customer.CustomerId);

                return CommandResult.GetSuccess(token, ResponseStatus.Success);
            }
            else
            {
                return CommandResult.GetError(ResponseStatus.Error, StringResources.COMMAND_CUSTOMER_INVALIDCREDENTIALS);
            }
        }


        #region Methods
        private JsonWebToken Create(Guid customerId)
        {
            DateTime tokenExpire = DateTime.Now.AddMinutes(_appSettings.TokenSettings.JwtTokenLifeTime);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _appSettings.TokenSettings.JwtTokenAudience,
                Issuer = _appSettings.TokenSettings.JwtTokenIssuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("CustomerId", customerId.ToString())
                }),
                Expires = tokenExpire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(jwtTokenHandler);
            string refreshToken = GenerateRefreshToken();

            JsonWebToken model = new()
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpire = tokenExpire,
                CustomerId = customerId
            };

            return model;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }
}
