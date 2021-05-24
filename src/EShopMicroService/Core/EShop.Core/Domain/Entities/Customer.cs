using System;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Domain.Entities.Base;
using EShop.Core.Model.Enums;

namespace EShop.Core.Domain.Entities
{
    public class Customer : IEntityBase
    {
        [Key]
        public Guid CustomerId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CustomerStatus Status { get; set; }
        
    }
}
