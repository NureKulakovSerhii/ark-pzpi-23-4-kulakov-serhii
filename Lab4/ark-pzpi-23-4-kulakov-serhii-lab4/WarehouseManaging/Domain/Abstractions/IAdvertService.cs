using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IAdvertService
    {
        Task<AdvertDto> CreateAdvertAsync(CreateAdvertDto advertDto, Guid userId);
        Task DeleteAdvertAsync(Guid advertId, Guid userId, bool isModerator);
        Task HideAdvertAsync(Guid advertId, Guid userId, bool isModerator);
        Task ActivateAdvertAsync(Guid advertId, Guid userId, bool isModerator);
        Task<AdvertDto> UpdateAdvertAsync(Guid userId, Guid advertId, UpdateAdvertDto updateAdvertDto);
        Task<List<AdvertDto>>? SearchWarehouses(SearchWarehouseDto searchWarehouseDto, AdvertSortBy sortBy);
        Task<List<AdvertDto>> GetAllAdvertsAsync(AdvertSortBy sortBy);
        Task<List<AdvertDto>> GetAllInactiveAdverts();
        Task<AdvertDto> GetAdvertByIdAsync(Guid advertId);
    }
}
