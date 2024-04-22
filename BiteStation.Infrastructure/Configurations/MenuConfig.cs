using BiteStation.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Infrastructure.Configurations;
internal class MenuConfig : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Restaurant)
            .WithMany(x => x.Menus)
            .HasForeignKey(x => x.RestaurantId)
            .IsRequired();
    }
}
