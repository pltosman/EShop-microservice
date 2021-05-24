using System;
using EShop.Core.Domain.Entities.Base;

namespace EShop.Core.Domain.Entities
{
    public class CustomerDetail : EntityBase
    {
        public Guid CustomerId { get; set; }
        public DateTime RegistrationOn { get; set; }
        public string PasswordReminderToken { get; set; }
        public DateTime? PasswordReminderExpire { get; set; }
        public string EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationExpire { get; set; }
        public bool EmailConfirmed { get; set; }
       
    }
}
