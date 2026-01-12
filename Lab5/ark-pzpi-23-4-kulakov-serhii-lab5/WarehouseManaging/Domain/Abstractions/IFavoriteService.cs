using Domain.DateTrensferObjects;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IFavoriteService
    {
        Task AddAdvertToFavoritesAsync(Guid userId, Guid advertId);
        Task RemoveAdvertFromFavoritesAsync(Guid userId, Guid advertId);
        Task<List<AdvertDto>> GetUserFavoritesListAsync(Guid userId, AdvertSortBy sortBy);
    }
}
