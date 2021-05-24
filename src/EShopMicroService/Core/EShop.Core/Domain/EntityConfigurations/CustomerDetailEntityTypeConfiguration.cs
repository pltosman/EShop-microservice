using System;
using EShop.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Core.Domain.EntityConfigurations
{
    public class CustomerDetailEntityTypeConfiguration : IEntityTypeConfiguration<CustomerDetail>
    {
        public void Configure(EntityTypeBuilder<CustomerDetail> builder)
        {
            builder.ToTable("customer_details");

            builder.HasKey(x => x.CustomerId);

            builder.HasIndex(x => x.CustomerId)
                .IsUnique();

            builder.Property(x => x.CustomerId)
                   .HasColumnName("customer_id");

            builder.Property(x => x.RegistrationOn)
                   .HasColumnName("registration_on"); 

            builder.Property(x => x.PasswordReminderToken)
                   .HasColumnName("password_reminder_token");

            builder.Property(x => x.PasswordReminderExpire)
                   .HasColumnName("password_reminder_expire");

            builder.Property(x => x.EmailConfirmationToken)
                   .HasColumnName("email_confirmation_token");

            builder.Property(x => x.EmailConfirmationExpire)
                   .HasColumnName("email_confirmation_expire");

            builder.Property(x => x.EmailConfirmed)
                   .HasColumnName("email_confirmed");
 
        }
    }
}

