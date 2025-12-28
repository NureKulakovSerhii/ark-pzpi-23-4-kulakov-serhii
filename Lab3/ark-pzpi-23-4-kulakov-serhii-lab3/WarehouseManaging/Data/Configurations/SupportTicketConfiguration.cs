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
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            builder.HasKey(st => st.Id);
            builder.Property(st => st.Title).HasMaxLength(50);
            builder.Property(st => st.Description).HasMaxLength(300);
            builder.HasMany(user => user.Comments).WithOne(comment => comment.Ticket)
                .HasForeignKey(comment => comment.TicketId)
                .IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
