using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user/telemetry")]
    public class UserTelemetryController(IIotDeviceService service) : ControllerBase
    {
        [HttpGet("devices")]
        public async Task<IActionResult> GetMyDevices()
        {
            var userId = GetCurrentUserId();
            var devices = await service.GetUserDevicesAsync(userId);
            var result = devices.Select(d => new
            {
                d.Id,
                d.DeviceId,
                d.IsOnline,
                d.LastSeen,
                Warehouse = new
                {
                    d.Warehouse.Id,
                    d.Warehouse.Address,
                    d.Warehouse.City,
                }
            });
            return Ok(result);
        }
        [HttpGet("device/{deviceId}/telemetry")]
        public async Task<IActionResult> GetDeviceTelemetry(Guid deviceId,
            [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null,
            [FromQuery] string? eventType = null, [FromQuery] int limit = 100)
        {
            try
            {
                var userId = GetCurrentUserId();
                var filter = new TelemetryFilterDto
                {
                    From = from,
                    To = to,
                    EventType = eventType,
                    Limit = limit,
                };
                var telemetry = await service.GetDeviceTelemetryAsync(userId, deviceId, filter);
                var result = telemetry.Select(t => new
                {
                    t.Timestamp,
                    t.EventType,
                    t.IsDoorOpen,
                    t.IsPowerOn,
                    t.Temperature,
                    t.Humidity,
                });
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}
