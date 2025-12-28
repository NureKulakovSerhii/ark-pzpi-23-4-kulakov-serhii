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
