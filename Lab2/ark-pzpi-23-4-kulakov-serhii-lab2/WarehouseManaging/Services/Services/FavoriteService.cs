using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FavoriteService : IFavoriteService
    {
        private const int limit = 9;
        private readonly IAdvertRepository _advertRepository;
        private readonly IUserRepository _userRepository;
        public FavoriteService(IAdvertRepository advertRepository, IUserRepository userRepository)
        {
            _advertRepository = advertRepository;
            _userRepository = userRepository;
        }

        public async Task AddAdvertToFavoritesAsync(Guid userId, Guid advertId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User is not found");

            var advert = await _advertRepository.GetAdvertById(advertId);
            if (advert == null)
                throw new Exception("Advert is not found");

            var advertInFavorites = await _advertRepository.IsAdvertInFavorites(userId, advertId);
            if (advertInFavorites)
                throw new Exception("Advert is already added to favorites");

            var currentCount = await _advertRepository.GetUserFavoritesCount(userId);
            if (currentCount > limit)
                throw new Exception("You can save only 9 adverts in your favorite list");

            var favorite = new UserFavoriteAdvert
            {
                UserId = userId,
                AdvertId = advertId,
                AddedAt = DateTime.UtcNow,
                Order = currentCount + 1
            };
            await _advertRepository.AddAdvertToFavorites(favorite);
        }

        public async Task<List<AdvertDto>> GetUserFavoritesListAsync(Guid userId, AdvertSortBy sortBy)
        {
            var favorites = await _advertRepository.GiveUserFavorites(userId);
            return favorites.Where(f => f.Advert.IsActive)
                .Select(f => MapToAdvertDto(f.Advert, f.Advert.Warehouse, f.Advert.User))
                .ApplySorting(sortBy)
                .ToList(); 
        }

        public async Task RemoveAdvertFromFavoritesAsync(Guid userId, Guid advertId)
        {
            var advertExists = await _advertRepository.IsAdvertInFavorites(userId, advertId);
            if (!advertExists)
                throw new Exception("Advert is not in favorite list");
            await _advertRepository.RemoveAdvertFromFavorites(userId, advertId);
        }

        private AdvertDto MapToAdvertDto(Advert advert, Warehouse warehouse, User user)
        {
            var advertDto = new AdvertDto
            {
                Id = advert.Id,
                Title = advert.Title,
                Description = advert.Description,
                IsActive = advert.IsActive,
                CreatedAt = advert.CreatedAt,
                Warehouse = new WarehouseDto
                {
                    PricePerMonth = warehouse.PricePerMonth,
                    Scale = warehouse.Scale,
                    Address = warehouse.Address,
                    Floor = warehouse.Floor,
                    BuildingType = warehouse.BuildingType,
                    HouseholdAppliances = warehouse.HouseholdAppliances,
                    Infrastructures = warehouse.Infrastructures,
                    ImageUrl = warehouse.ImageUrl,
                    City = warehouse.City
                },
                Author = new AdvertAuthorDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    CreatedAt = user.CreatedAt,
                }
            };
            return advertDto;
        }
    }

}
