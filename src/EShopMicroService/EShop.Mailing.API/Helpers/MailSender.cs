using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace EShop.Mailing.API.Helpers
{
    public static class MailSender
    {
        public static void SendGmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("EMAIL_ADDRESS@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("EMAIL_ADDRESS@gmail.com", "EMAIL_PASSWORD");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}