using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IIotDeviceRepository
    {
        Task<WarehouseDevice?> GetDeviceByCredentialsAsync(string deviceId, string secretKey);
        Task<WarehouseDevice?> GetDeviceByIdAsync(Guid deviceId);
        Task UpdateDeviceStatusAsync(Guid deviceId, DateTime lastSeen, bool isOnline);
        Task AddTelemetryAsync(DeviceTelemetry telemetry);
        Task<List<DeviceTelemetry>> GetTelemetryByDeviceAsync(Guid deviceId, DateTime? from, DateTime? to, string? eventType = null, int limit = 100);
        Task<DeviceTelemetry?> GetLatestTelemetryAsync(Guid deviceId, string? eventType = null);
        Task<bool> UserHasAccessToDeviceAsync(Guid userId, Guid deviceId);
        Task<List<WarehouseDevice>> GetUserDevicesAsync(Guid userId);
    }
}
