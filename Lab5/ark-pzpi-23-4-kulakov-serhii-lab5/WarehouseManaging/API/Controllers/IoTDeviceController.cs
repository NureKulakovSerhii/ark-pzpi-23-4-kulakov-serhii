using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/iot")]
    [ApiController]
    public class IoTDeviceController(IIotDeviceService service) : ControllerBase
    {
        [HttpPost("telemetry")]
        public async Task<IActionResult> PostTelemetry([FromBody] TelemeteryDataDto dto)
        {
            try
            {
                var telemetry = await service.ProcessTelemetryAsync(dto.DeviceId, dto.SecretKey, dto);
                return Ok(new TelemetryResponseDto
                {
                    Success = true,
                    Message = $"Telemetry recieved: {dto.EventType}",
                    ServerTime = DateTime.UtcNow
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new TelemetryResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    ServerTime = DateTime.UtcNow,
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new TelemetryResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    ServerTime = DateTime.UtcNow,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new TelemetryResponseDto
                {
                    Success = false,
                    Message = "Internal server error!",
                    ServerTime = DateTime.UtcNow,
                });
            }
        }
        [HttpPost("heartbeat")]
        public async Task<IActionResult> HeartBeat([FromBody] DeviceAuthRequest request)
        {
            var device = await service.AuthenticateDeviceAsync(request.DeviceId, request.SecretKey);
            if (device == null)
                return Unauthorized();
            return Ok(new
            {
                status = "ok",
                timestamp = DateTime.UtcNow,
            });
        }
    }
}
