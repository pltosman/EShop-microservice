using System;
namespace EShop.Core.Model.RequestModels
{
    public class LoginWithEmailRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
