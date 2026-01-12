using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IIotDeviceService
    {
        Task<WarehouseDevice?> AuthenticateDeviceAsync(string deviceId, string secretKey);
        Task<DeviceTelemetry> SaveTelemetryAsync(WarehouseDevice device, TelemeteryDataDto dto);
        Task UpdateDeviceStatusAsync(Guid deviceId, bool isOnline);
        Task<List<WarehouseDevice>> GetUserDevicesAsync(Guid userId);
        Task<List<DeviceTelemetry>> GetDeviceTelemetryAsync(Guid userId, Guid deviceId, TelemetryFilterDto filter);
        Task<DeviceTelemetry> ProcessTelemetryAsync(string deviceId, string secretKey, TelemeteryDataDto dto);
    }
}
