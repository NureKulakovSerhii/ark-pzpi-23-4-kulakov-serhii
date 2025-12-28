using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using User = Domain.Models.User;

namespace Services.Services
{
    public class AdvertService(IAdvertRepository advertRepository, IUserRepository userRepository,
        IWarehouseRepository warehouseRepository, IConfiguration configuration) : IAdvertService
    {
        private readonly Cloudinary _cloudinary = new Cloudinary(new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        ));
        public async Task<AdvertDto> CreateAdvertAsync(CreateAdvertDto advertDto, Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            AdvertValidation(advertDto);
            WarehouseValidation(advertDto.WarehouseDto);
            var warehouseExist = await warehouseRepository.GetWarehouseByAddress(advertDto.WarehouseDto.Address);

            Warehouse warehouse;
            if (warehouseExist != null)
            {
                throw new Exception("Warehouse with this address already exists!");
            }
            else
            {
                warehouse = new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Address = advertDto.WarehouseDto.Address,
                    PricePerMonth = advertDto.WarehouseDto.PricePerMonth,
                    Scale = advertDto.WarehouseDto.Scale,
                    Floor = advertDto.WarehouseDto.Floor,
                    BuildingType = advertDto.WarehouseDto.BuildingType,
                    City = advertDto.WarehouseDto.City,
                    Communications = advertDto.WarehouseDto.Communications,
                    HouseholdAppliances = advertDto.WarehouseDto.HouseholdAppliances,
                    Infrastructures = advertDto.WarehouseDto.Infrastructures,
                };
                if (advertDto.WarehouseDto.ImageFile != null && advertDto.WarehouseDto.ImageFile.Length > 0)
                {
                    warehouse.ImageUrl = await UploadToCloudinaryAsync(advertDto.WarehouseDto.ImageFile, warehouse.Id);
                }
                await warehouseRepository.CreateWarehouse(warehouse);
            }

            var advert = new Advert
            {
                Id = Guid.NewGuid(),
                Title = advertDto.Title,
                Description = advertDto.Description,
                CreatedAt = DateTime.UtcNow,
                IsActive = false,
                WarehouseId = warehouse.Id,
                UserId = userId,
            };
            await advertRepository.CreateAdvert(advert);
            return MapToAdvertDto(advert, warehouse, user);
        }
        public async Task DeleteAdvertAsync(Guid advertId, Guid userId, bool isModerator)
        {
            var advertExists = await advertRepository.GetAdvertById(advertId);
            if (advertExists == null)
                throw new Exception("Advert is not found");
            if (userId != advertExists.UserId && !isModerator)
                throw new Exception("You can`t delete this advert");

            var warehouseId = advertExists.WarehouseId;
            await advertRepository.DeleteAdvert(advertId);

            var advertsCounter = await advertRepository.CountAdvertsWithWarehouseId(warehouseId);
            if (advertsCounter == 0)
            {
                var warehouse = await warehouseRepository.GetWarehouseByIdAsync(warehouseId);
                if (warehouse != null && !string.IsNullOrEmpty(warehouse.ImageUrl))
                {
                    var publicId = ExtractPublicIdFromUrl(warehouse.ImageUrl);
                    if (publicId != null)
                        await DeleteFromCloudinaryAsync(publicId);
                }
                await warehouseRepository.DeleteWarehouse(warehouseId);
            }
        }
        public async Task HideAdvertAsync(Guid advertId, Guid userId, bool isModerator)
        {
            var advert = await advertRepository.GetAdvertById(advertId);
            if (advert == null)
                throw new Exception("Advert not found");
            if (userId != advert.UserId && !isModerator)
                throw new Exception("User can hide his only adverts");
            if (advert.IsActive == false)
                throw new Exception("Advert is alrady hide");
            advert.IsActive = false;
            await advertRepository.UpdateAdvert(advert);
        }
        public async Task ActivateAdvertAsync(Guid advertId, Guid userId, bool isModerator)
        {
            var advert = await advertRepository.GetAdvertById(advertId);
            if (advert == null)
                throw new Exception("Advert not found");
            if (!isModerator)
                throw new Exception("Only moderators can activate adverts");
            if (advert.IsActive == true)
                throw new Exception("Advert is already active");
            advert.IsActive = true;
            await advertRepository.UpdateAdvert(advert);
        }
        public async Task<AdvertDto> UpdateAdvertAsync(Guid userId, Guid advertId, UpdateAdvertDto updateAdvertDto)
        {
            var advert = await advertRepository.GetAdvertById(advertId);
            if (advert == null)
                throw new Exception("Advert not found");

            if (advert.UserId != userId)
                throw new Exception("User can update his own adverts");

            if (string.IsNullOrEmpty(updateAdvertDto.Title))
                throw new Exception("Title field must be filled!");
            advert.Title = updateAdvertDto.Title;
            if (string.IsNullOrEmpty(updateAdvertDto.Description))
                throw new Exception("Description field must be filled!");
            advert.Description = updateAdvertDto.Description;

            if (updateAdvertDto.updateWarehouseDto != null)
            {
                if (updateAdvertDto.updateWarehouseDto.PricePerMonth <= 0)
                {
                    throw new Exception("Price field must be greater than 0!");
                }
                advert.Warehouse.PricePerMonth = updateAdvertDto.updateWarehouseDto.PricePerMonth;
            }
            if (updateAdvertDto.updateWarehouseDto.Infrastructures.Count > 0)
            {
                advert.Warehouse.Infrastructures = updateAdvertDto.updateWarehouseDto.Infrastructures;
            }
            if (updateAdvertDto.updateWarehouseDto.HouseholdAppliances.Count > 0)
            {
                advert.Warehouse.HouseholdAppliances = updateAdvertDto.updateWarehouseDto.HouseholdAppliances;
            }
            if (updateAdvertDto.updateWarehouseDto.Communications.Count > 0)
            {
                advert.Warehouse.Communications = updateAdvertDto.updateWarehouseDto.Communications;
            }
            if (updateAdvertDto.updateWarehouseDto.ImageFile != null && updateAdvertDto.updateWarehouseDto.ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(advert.Warehouse.ImageUrl))
                {
                    var oldPublicId = ExtractPublicIdFromUrl(advert.Warehouse.ImageUrl);
                    if (oldPublicId != null)
                        await DeleteFromCloudinaryAsync(oldPublicId);
                }
                advert.Warehouse.ImageUrl = await UploadToCloudinaryAsync(updateAdvertDto.updateWarehouseDto.ImageFile, advert.Warehouse.Id);
            }
            await advertRepository.UpdateAdvert(advert);
            var updatedAdvert = await advertRepository.GetAdvertById(advertId);
            return MapToAdvertDto(updatedAdvert!, updatedAdvert!.Warehouse, updatedAdvert.User);
        }

        public async Task<List<AdvertDto>>? SearchWarehouses(SearchWarehouseDto searchWarehouseDto, AdvertSortBy sortBy)
        {
            var warehouses = await warehouseRepository.SearchWarehouses(searchWarehouseDto);
            if (!warehouses.Any())
            {
                return new List<AdvertDto>();
            }
            var adverts = warehouses.SelectMany(w => w.Adverts).Select(advert => MapToAdvertDto(advert, advert.Warehouse, advert.User))
                .ApplySorting(sortBy)
                .ToList();
            return adverts;
        }
        public async Task<List<AdvertDto>> GetAllAdvertsAsync(AdvertSortBy sortBy)
        {
            var advertsDB = await advertRepository.GetAllAdverts();
            if (!advertsDB.Any())
            {
                return new List<AdvertDto>();
            }
            var adverts = advertsDB.Select(advert => MapToAdvertDto(advert, advert.Warehouse, advert.User))
                .ApplySorting(sortBy)
                .ToList();
            return adverts;
        }
        public async Task<List<AdvertDto>> GetAllInactiveAdverts()
        {
            var advertsDB = await advertRepository.GetAllInactiveAdverts();
            if (!advertsDB.Any())
            {
                return new List<AdvertDto>();
            }
            var adverts = advertsDB.Select(advert => MapToAdvertDto(advert, advert.Warehouse, advert.User)).ToList();
            return adverts;
        }
        public async Task<AdvertDto> GetAdvertByIdAsync(Guid advertId)
        {
            var advert = await advertRepository.GetAdvertById(advertId);
            if (advert == null)
            {
                throw new Exception("Advert is not fount in database");
            }
            return MapToAdvertDto(advert!, advert!.Warehouse, advert!.User);
        }
        private void AdvertValidation(CreateAdvertDto cadto)
        {
            if (string.IsNullOrEmpty(cadto.Title) || cadto.Title.Length < 3)
                throw new Exception("Advert's title field must be filled!");
            if (string.IsNullOrEmpty(cadto.Description) || cadto.Description.Length < 15)
                throw new Exception("Advert's Descriprion field must be filled!");
        }
        private void WarehouseValidation(CreateWarehouseDto cwdto)
        {
            if (string.IsNullOrEmpty(cwdto.Address))
                throw new Exception("Adress field must be filled!");
            if (cwdto.PricePerMonth <= 0)
                throw new Exception("Price field must be filled!");
            if (cwdto.Scale <= 0)
                throw new Exception("Scale field must be filled!");
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
                    Communications = warehouse.Communications,
                    HouseholdAppliances = warehouse.HouseholdAppliances,
                    Infrastructures = warehouse.Infrastructures,
                    City = warehouse.City,
                    ImageUrl = warehouse.ImageUrl,
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
        private async Task<string> UploadToCloudinaryAsync(IFormFile file, Guid warehouseId)
        {
            // Валідація файлу
            if (file.Length == 0 || file.Length > 10 * 1024 * 1024)
                throw new Exception("Розмір фото має бути до 10 МБ");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
                throw new Exception("Тільки JPG, PNG, WebP");

            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = $"warehouses/{warehouseId}", // Організовуємо в папку
                    Overwrite = true,
                    Transformation = new Transformation()
                        .Width(1200).Height(800).Crop("limit") // Обмежуємо розмір
                        .Quality("auto") // Автоматична оптимізація
                        .FetchFormat("auto") // Автоматичний формат (WebP для браузерів що підтримують)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Помилка завантаження: {uploadResult.Error?.Message}");
                }

                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Не вдалося завантажити фото: {ex.Message}");
            }
        }
        private string? ExtractPublicIdFromUrl(string url)
        {
            try
            {
                // URL формату: https://res.cloudinary.com/{cloud_name}/image/upload/v{version}/{public_id}.{format}
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/');

                // Знаходимо індекс "upload"
                var uploadIndex = Array.IndexOf(segments, "upload");
                if (uploadIndex == -1 || uploadIndex + 2 >= segments.Length)
                    return null;

                // public_id починається після версії (v1234567890)
                var publicIdParts = segments.Skip(uploadIndex + 2).ToArray();
                var publicIdWithExt = string.Join("/", publicIdParts);

                // Видаляємо розширення
                var lastDotIndex = publicIdWithExt.LastIndexOf('.');
                return lastDotIndex > 0 ? publicIdWithExt.Substring(0, lastDotIndex) : publicIdWithExt;
            }
            catch
            {
                return null;
            }
        }
        private async Task DeleteFromCloudinaryAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId);
                await _cloudinary.DestroyAsync(deletionParams);
            }
            catch
            {
                // Ігноруємо помилки видалення
            }
        }
    }
}