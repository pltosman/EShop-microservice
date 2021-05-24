using System;
using EShop.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Core.Domain.EntityConfigurations
{
    public class CustomerTokenEntityTypeConfiguration : IEntityTypeConfiguration<CustomerToken>
    {
        public void Configure(EntityTypeBuilder<CustomerToken> builder)
        {
            builder.ToTable("customer_tokens");

            builder.HasKey(x => x.CustomerTokenId);

            builder.HasIndex(x => x.CustomerTokenId)
                .IsUnique();

            builder.Property(x => x.CustomerTokenId)
                   .HasColumnName("id")
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsRequired();

            builder.Property(x => x.CustomerId)
                   .HasColumnName("customer_id");

            builder.Property(x => x.Token)
                   .HasColumnName("token");

            builder.Property(x => x.RefreshToken)
                   .HasColumnName("refresh_token");

            builder.Property(x => x.TokenExpire)
                   .HasColumnName("token_expire");
        }
    }
}
