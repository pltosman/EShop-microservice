using System;
using EShop.EventBus.Events;

namespace EShop.Core.IntegrationEvents
{
    public class EmailConfirmationEvent : IntegrationEvent
    {
        public string To { get; set; }  
        public string OtpCode { get; set; }

        public EmailConfirmationEvent(string to,  string otpCode)
        {
            To = to;
            OtpCode = otpCode;
        }
    }
}
