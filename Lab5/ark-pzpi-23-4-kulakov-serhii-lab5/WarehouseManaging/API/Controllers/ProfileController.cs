using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("get-my-user-profile")]
        [Authorize]
        public async Task<ActionResult<ProfileDto>> GetUserProfileAsync()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userProfile = await _profileService.GetUserByIdAsync(userId);
            if (userProfile == null)
            {
                return BadRequest(new { message = "User is not found in database" });
            }
            return Ok(userProfile);
        }
        [HttpPatch("update-profile")]
        [Authorize]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UpdateProfileDto updto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userProfile = await _profileService.GetUserByIdAsync(userId);
            if (userProfile == null)
                return BadRequest(new { message = "User is not found in database" });
            await _profileService.UpdateUserProfileAsync(userId, updto);
            return Ok(updto);
        }
    }
}
