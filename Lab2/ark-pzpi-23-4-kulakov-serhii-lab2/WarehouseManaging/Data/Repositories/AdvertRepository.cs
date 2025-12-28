using Data.DB;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AdvertRepository(AppDbContext appDbContext): IAdvertRepository
    {
        public async Task<int> CountAdvertsWithWarehouseId(Guid warehouseId)
        {
            return await appDbContext.Adverts.CountAsync(a => a.WarehouseId == warehouseId);
        }

        public async Task<Advert> CreateAdvert(Advert advert)
        {
            appDbContext.Adverts.Add(advert);
            await appDbContext.SaveChangesAsync();
            return advert;
        }

        public async Task DeleteAdvert(Guid advertId)
        {
            var advert = await appDbContext.Adverts.FindAsync(advertId);
            if (advert != null)
            {
                appDbContext.Adverts.Remove(advert);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Advert?> GetAdvertById(Guid advertId)
        {
            var advert = await appDbContext.Adverts.Include(a => a.Warehouse)
                .Include(a =>  a.User).AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == advertId);
            return advert;
        }

        public async Task<List<Advert?>> GetAdvertByUserId(Guid userId)
        {
            var adverts = await appDbContext.Adverts
                .Include(a => a.Warehouse).Include(a => a.User)
                .Where(a => a.UserId == userId).ToListAsync();
            return adverts;
        }

        public async Task<List<Advert?>> GetAllActiveAdverts()
        {
            var adverts = await appDbContext.Adverts
                .Include(a => a.Warehouse)
                .Include(a => a.User)
                .Where(a => a.IsActive == true).AsNoTracking()
                .ToListAsync();
            return adverts;
        }
        public async Task<List<Advert?>> GetAllInactiveAdverts()
        {
            var adverts = await appDbContext.Adverts
                .Include(a => a.Warehouse)
                .Include(a => a.User)
                .Where(a => a.IsActive == false)
                .AsNoTracking()
                .ToListAsync();
            return adverts;
        }

        public async Task<List<Advert>> GetAllAdverts()
        {
            var adverts = await appDbContext.Adverts.Include(a => a.Warehouse).Include(a => a.User).ToListAsync();
            return adverts;
        }
        
        public async Task<List<UserFavoriteAdvert>> GiveUserFavorites(Guid userId)
        {
            var userFavoriteAdverts = await appDbContext.UserFavoriteAdverts
                .Include(uf => uf.Advert).ThenInclude(a => a.User)
                .Include(uf => uf.Advert).ThenInclude(a => a.Warehouse)
                .Where(uf => uf.UserId == userId).OrderByDescending(a => a.AddedAt).ToListAsync();
            return userFavoriteAdverts;
        }

        public async Task AddAdvertToFavorites(UserFavoriteAdvert userFavoriteAdvert)
        {
            await appDbContext.UserFavoriteAdverts.AddAsync(userFavoriteAdvert);
            await appDbContext.SaveChangesAsync();
        }

        public async Task RemoveAdvertFromFavorites(Guid userId, Guid advertId)
        {
            var advert = await appDbContext.UserFavoriteAdverts
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.AdvertId == advertId);
            if(advert != null) 
            {
                appDbContext.Remove(advert);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAdvert(Advert advert)
        {
            appDbContext.Adverts.Update(advert);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsAdvertInFavorites(Guid userId, Guid advertId)
        {
            return await appDbContext.UserFavoriteAdverts
                .AnyAsync(uf => uf.UserId == userId && uf.AdvertId == advertId);
        }

        public async Task<int> GetUserFavoritesCount(Guid userId)
        {
            return await appDbContext.UserFavoriteAdverts.CountAsync(uf => uf.UserId == userId);
        }
    }
}
