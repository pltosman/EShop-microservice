using EShop.EventBus.Events;

namespace EShop.Mailing.API.IntegrationEvents.Events
{
      public class EmailConfirmationEvent : IntegrationEvent
    {
        public string To { get; set; }
        public int LanguageId { get; set; }
        public string OtpCode { get; set; }

        public EmailConfirmationEvent(string to, int languageId, string otpCode)
        {
            To = to;
            LanguageId = languageId;
            OtpCode = otpCode;
        }
    }
}