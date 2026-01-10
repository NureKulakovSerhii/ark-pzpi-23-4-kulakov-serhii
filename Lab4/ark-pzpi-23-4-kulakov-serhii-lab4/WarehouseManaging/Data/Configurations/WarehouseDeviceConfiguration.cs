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
    public class WarehouseDeviceConfiguration: IEntityTypeConfiguration<WarehouseDevice>
    {
        public void Configure(EntityTypeBuilder<WarehouseDevice> builder)
        {
            builder.HasKey(d => d.Id);
            builder.HasIndex(d => d.DeviceId).IsUnique();
            builder.HasOne(d => d.Warehouse)
                .WithOne(w => w.Device)
                .HasForeignKey<WarehouseDevice>(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
