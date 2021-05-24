using System;
using EShop.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Core.Domain.EntityConfigurations
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customers");

            builder.HasKey(x => x.CustomerId);

            builder.HasIndex(x => x.CustomerId)
                .IsUnique();

            builder.Property(x => x.CustomerId)
                   .HasColumnName("customer_id")
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsRequired();

            builder.Property(x => x.PasswordHash)
                   .HasColumnName("password_hash");

            builder.Property(x => x.PasswordSalt)
                   .HasColumnName("password_salt");

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name");

            builder.Property(x => x.LastName)
                .HasColumnName("last_name");

            builder.Property(x => x.FullName)
                .HasColumnName("full_name");

            builder.Property(x => x.Email)
                .HasColumnName("email");  

            builder.Property(x => x.DateOfBirth)
                .HasColumnName("date_of_birth");  

            builder.Property(x => x.Status)
                .HasColumnName("status");
            
        }
    }
}
