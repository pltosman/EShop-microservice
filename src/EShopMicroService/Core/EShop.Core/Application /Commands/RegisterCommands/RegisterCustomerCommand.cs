using System;
using System.Runtime.Serialization;
using EShop.Core.Model.ResponseModels;
using MediatR;

namespace EShop.Core.Application.Commands.RegisterCommands
{
    public class RegisterCustomerCommand : IRequest<CommandResult>
    {
        [DataMember]
        public Guid CustomerId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Token { get; set; }

        public RegisterCustomerCommand(Guid customerId,
            string password,
            string firstName,
            string lastName,
            string email,
            string token)
        {
            CustomerId = customerId;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;  
            Token = token;
        }
    }
}
