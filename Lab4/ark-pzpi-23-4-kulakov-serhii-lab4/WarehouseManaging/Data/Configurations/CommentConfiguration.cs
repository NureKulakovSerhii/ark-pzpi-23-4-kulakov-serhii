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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(comment => comment.Id);
            builder.Property(comment => comment.Text).IsRequired();
            builder.Property(comment => comment.CreatedAt).IsRequired();
            builder.HasMany(comment => comment.Attachments).WithOne(attach => attach.Comment)
                .HasForeignKey(attach => attach.CommentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
