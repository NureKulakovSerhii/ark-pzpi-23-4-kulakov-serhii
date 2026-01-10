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
    public class DeviceTelemetryConfiguration: IEntityTypeConfiguration<DeviceTelemetry>
    {
        public void Configure(EntityTypeBuilder<DeviceTelemetry> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => new { t.DeviceId, t.Timestamp });
            builder.HasOne(t => t.Device)
                .WithMany()
                .HasForeignKey(t => t.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
