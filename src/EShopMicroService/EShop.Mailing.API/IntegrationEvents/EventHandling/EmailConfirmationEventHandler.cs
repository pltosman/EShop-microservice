using System.Threading.Tasks;
using EShop.EventBus.Abstractions;
using EShop.Mailing.API.Helpers;
using EShop.Mailing.API.IntegrationEvents.Events;

namespace EShop.Mailing.API.IntegrationEvents.EventHandling
{
    public class EmailConfirmationEventHandler : IIntegrationEventHandler<EmailConfirmationEvent>
    {
        private readonly string[] subject =
        {
            "EShop Mail adres doğrulama",
            "EShop Mail address verification",
            "EShop Überprüfung der E-Mail-Adresse"
        };

        private readonly string[] body =
        {
            "Mail adres doğrulama kodunuz {0}.",
            "Your mail address verification code {0}.",
            "Bestätigungscode für Ihre E-Mail-Adresse {0}."
        };

        public Task Handle(EmailConfirmationEvent @event)
        {
            string bodyContent = body[1].Replace("{0}", @event.OtpCode);

            MailSender.SendGmail(@event.To, subject[1], bodyContent);

            return Task.CompletedTask;
        }
    }
}