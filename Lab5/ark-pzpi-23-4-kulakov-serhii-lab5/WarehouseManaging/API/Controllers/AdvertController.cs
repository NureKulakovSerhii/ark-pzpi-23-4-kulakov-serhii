using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly IFavoriteService _favoriteSevice;
        public AdvertController(IAdvertService advertService, IFavoriteService favoriteService)
        {
            _advertService = advertService;
            _favoriteSevice = favoriteService;
        }
        [HttpPost("create-advert")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<ActionResult<AdvertDto>> CreateAdvertAsync([FromForm]CreateAdvertDto dto)
        {
            var user = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var advert = await _advertService.CreateAdvertAsync(dto, user);
            if(advert == null)
            {
                return BadRequest("Не вдалося створити оголошення");
            }
            return Ok(advert);
        }

        [HttpDelete("{advertId}/delete-advert")]
        [Authorize]
        public async Task<ActionResult> DeleteAdvertAsync([FromRoute]Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.IsInRole("Moderator");
            await _advertService.DeleteAdvertAsync(advertId, userId, role);
            return Ok();
        }

        [HttpPut("{advertId}/update-advert")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<ActionResult> UpdateAdvertAsync([FromForm] UpdateAdvertDto updateAdvertDto, [FromRoute] Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _advertService.UpdateAdvertAsync(userId, advertId, updateAdvertDto);
            return Ok(updateAdvertDto);
        }

        [HttpPut("{advertId}/hide-advert")]
        [Authorize]
        public async Task<ActionResult> HideAdvertAsync([FromRoute] Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.IsInRole("Moderator");
            await _advertService.HideAdvertAsync(advertId, userId, role);
            return Ok("Advert was hide successfully");
        }

        [HttpPut("{advertId}/activate-advert")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> ActivateAdvertAsync([FromRoute] Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.IsInRole("Moderator");
            await _advertService.ActivateAdvertAsync(advertId, userId, role);
            return Ok("Advert was activate successfully");
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AdvertDto>>> SearchAdvertsAsync([FromQuery] SearchWarehouseDto searchWarehouseDto, 
            [FromQuery] AdvertSortBy sortBy = AdvertSortBy.None)
        {
            var adverts = await _advertService.SearchWarehouses(searchWarehouseDto, sortBy);
            return Ok(adverts);
        }
        [HttpPost("{advertId}/add-to-favorites")]
        [Authorize]
        public async Task<ActionResult> AddToFavoritesAsync([FromRoute] Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _favoriteSevice.AddAdvertToFavoritesAsync(userId, advertId);
            return Ok(new { message = "Advert was added to favorites successfully"} );
        }
        [HttpDelete("{advertId}/delete-from-favorites")]
        [Authorize]
        public async Task<ActionResult> RemoveFromFavoritesAsync([FromRoute] Guid advertId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _favoriteSevice.RemoveAdvertFromFavoritesAsync(userId, advertId);
            return Ok(new { message = "Advert was removed from favorites successfully" } );
        }
        [HttpGet("get-favorites")]
        [Authorize]
        public async Task<ActionResult<List<UserFavoriteAdvert>>> GetUserFavorites(
            [FromQuery] AdvertSortBy sortBy = AdvertSortBy.None)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var favorites = await _favoriteSevice.GetUserFavoritesListAsync(userId, sortBy);
            return Ok(favorites);
        }
        [HttpGet("all-adverts")]
        public async Task<ActionResult> GetAllAdverts([FromQuery] AdvertSortBy sortBy)
        {
            var adverts = await _advertService.GetAllAdvertsAsync(sortBy);
            if(adverts == null)
            {
                return BadRequest(new { message = "No adverts has been recieved " });
            }
            return Ok(adverts);
        }
        [HttpGet("inactive-adverts")]
        public async Task<ActionResult> GetAllInactiveAdverts()
        {
            var adverts = await _advertService.GetAllInactiveAdverts();
            if(adverts == null)
            {
                return BadRequest(new { message = "No adverts has been received" });
            }
            return Ok(adverts);
        }
        [HttpGet("get-advert")]
        public async Task<ActionResult<AdvertDto>> GetAdvertById(Guid advertId)
        {
            var advert = await _advertService.GetAdvertByIdAsync(advertId);
            if(advert == null)
            {
                return BadRequest(new { message = "Advert is not found" });
            }
            return Ok(advert);
        }
    }
}
