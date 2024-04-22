using BiteStation.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Infrastructure.Configurations;
internal class ItemConfig : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Menu)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.MenuId)
            .IsRequired();
        
        builder.Property(x => x.Price).HasPrecision(10,2);
    }
}
