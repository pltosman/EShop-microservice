using System;
using System.Runtime.Serialization;
using EShop.Core.Model.ResponseModels;
using MediatR;

namespace EShop.Core.Application.Commands.ResetPasswordCommands
{
    public class ResetPasswordCommand : IRequest<CommandResult>
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token, string newPassword)
        {
            Token = token;
            NewPassword = newPassword;
        }
    }
}