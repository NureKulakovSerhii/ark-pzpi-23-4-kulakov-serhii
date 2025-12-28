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
    public class ModerationTaskConfiguration : IEntityTypeConfiguration<ModerationTask>
    {
        public void Configure(EntityTypeBuilder<ModerationTask> builder)
        {
            builder.HasKey(mt => mt.Id);
            builder.HasOne(mt => mt.Administrator).WithMany(a => a.ModerationTasks)
                .HasForeignKey(mt => mt.AdministratorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(mt => mt.Advert).WithMany(a => a.ModerationTasks)
                .HasForeignKey(mt => mt.AdvertId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
