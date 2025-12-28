using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IAdvertRepository
    {
        Task<Advert> CreateAdvert(Advert advert);
        Task UpdateAdvert(Advert advert);
        Task DeleteAdvert(Guid advertId);
        Task<Advert?> GetAdvertById(Guid advertId);
        Task<List<Advert?>> GetAdvertByUserId(Guid userId);
        Task<List<Advert?>> GetAllActiveAdverts();
        Task<List<Advert?>> GetAllInactiveAdverts();
        Task<int> CountAdvertsWithWarehouseId(Guid warehouseId);
        Task<List<Advert>> GetAllAdverts();
        Task AddAdvertToFavorites(UserFavoriteAdvert userFavoriteAdvert);
        Task RemoveAdvertFromFavorites(Guid userId, Guid advertId);
        Task<List<UserFavoriteAdvert>> GiveUserFavorites(Guid userId);
        Task<bool> IsAdvertInFavorites(Guid userId, Guid advertId);
        Task<int> GetUserFavoritesCount(Guid userId);
    }
}
