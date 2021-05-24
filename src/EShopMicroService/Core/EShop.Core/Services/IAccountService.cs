using System;
using EShop.Core.Helpers;
using EShop.Core.Model.ResponseModels;

namespace EShop.Core.Services
{
    public interface IAccountService
    {
        JsonWebToken CrateToken(Guid customerId);
        CommandResult RefreshToken(Guid customerId, string token);
        CommandResult LoginWithEmail(string email, string password);
     
    }
}
