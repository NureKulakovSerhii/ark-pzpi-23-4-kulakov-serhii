using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Warehouse).WithMany(w => w.Adverts)
                .HasForeignKey(a => a.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.User).WithMany(l => l.UserAdverts);
            builder.HasMany(a => a.ModerationTasks).WithOne(m => m.Advert);
        }
    }
}
