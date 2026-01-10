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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasMany(u => u.UserRoles).WithOne(ur => ur.User);
            builder.HasMany(u => u.UserAdverts).WithOne(ua => ua.User);
            builder.HasMany(u => u.ModerationTasks).WithOne(mt => mt.Administrator);
            builder.HasMany(u => u.AssignedTickets).WithOne(at => at.AssignedTo)
                .HasForeignKey(at => at.AssignedToId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.CreateTickets).WithOne(ct => ct.User)
                .HasForeignKey(ct => ct.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
