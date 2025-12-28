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
    public class UserFavoriteAdvertConfiguration : IEntityTypeConfiguration<UserFavoriteAdvert>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteAdvert> builder)
        {
            builder.HasKey(uf => new { uf.UserId, uf.AdvertId });
            builder.HasOne(uf => uf.User).WithMany(u => u.FavoriteAdverts)
                .HasForeignKey(uf => uf.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(uf => uf.Advert).WithMany(a => a.FavoriteByUser)
                .HasForeignKey(uf => uf.AdvertId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
