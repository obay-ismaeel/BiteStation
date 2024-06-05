using BiteStation.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Infrastructure.Configurations;
internal class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Order)
            .WithOne(x => x.Cart)
            .HasForeignKey<Cart>(x => x.OrderId)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Cart)
            .HasForeignKey<Cart>(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId );
    }
}
