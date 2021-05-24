using MediatR;
using System;
using System.Runtime.Serialization;
using EShop.Core.Model.ResponseModels;


namespace EShop.Core.Application.Commands.ConfirmationCommands
{
    public class EmailConfirmationCommand : IRequest<CommandResult>
    {
        [DataMember]
        public Guid CustomerId { get; set; }

        public EmailConfirmationCommand(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
