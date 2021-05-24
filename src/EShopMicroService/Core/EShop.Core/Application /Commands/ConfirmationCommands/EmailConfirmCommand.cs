using System;
using System.Runtime.Serialization;
using EShop.Core.Model.ResponseModels;
using MediatR;

namespace EShop.Core.Application.Commands.ConfirmationCommands
{
    public class EmailConfirmCommand : IRequest<CommandResult>
    {
        [DataMember]
        public Guid CustomerId { get; set; }

        [DataMember]
        public string OtpCode { get; set; }

        public EmailConfirmCommand(Guid customerId, string otpCode)
        {
            CustomerId = customerId;
            OtpCode = otpCode;
        }
    }
}
