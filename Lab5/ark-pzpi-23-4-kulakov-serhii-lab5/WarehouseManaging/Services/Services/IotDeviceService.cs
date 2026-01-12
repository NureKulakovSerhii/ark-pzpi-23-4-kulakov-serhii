using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Models;

namespace Services.Services
{
    public class IotDeviceService(IIotDeviceRepository repository) : IIotDeviceService
    {
        public async Task<WarehouseDevice?> AuthenticateDeviceAsync(string deviceId, string secretKey)
        {
            var device = await repository.GetDeviceByCredentialsAsync(deviceId, secretKey);
            if (device != null)
            {
                await repository.UpdateDeviceStatusAsync(device.Id, DateTime.UtcNow, true);
                return device; 
            }
            else
            {
                return null;
            }
        }

        public async Task<List<DeviceTelemetry>> GetDeviceTelemetryAsync(Guid userId, Guid deviceId, TelemetryFilterDto filter)
        {
            var hasAccess = await repository.UserHasAccessToDeviceAsync(userId, deviceId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("User has not access to this device");
            return await repository.GetTelemetryByDeviceAsync
            (
                deviceId,
                filter.From,
                filter.To,
                filter.EventType,
                filter.Limit
            );
        }

        public async Task<List<WarehouseDevice>> GetUserDevicesAsync(Guid userId)
        {
            return await repository.GetUserDevicesAsync(userId);
        }

        public async Task<DeviceTelemetry> ProcessTelemetryAsync(string deviceId, string secretKey, TelemeteryDataDto dto)
        {
            var device = await AuthenticateDeviceAsync(deviceId, secretKey);
            if (device == null)
                Console.WriteLine("Invalid device credentials");
            ValidateTelemetryData(dto);
            var telemetry = new DeviceTelemetry
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Timestamp = DateTime.UtcNow,
                IsDoorOpen = dto.IsDoorOpen ?? false,
                IsPowerOn = dto.IsPowerOn ?? true,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                EventType = dto.EventType,
            };
            await repository.AddTelemetryAsync(telemetry);
            return telemetry;
        }

        public Task<DeviceTelemetry> SaveTelemetryAsync(WarehouseDevice device, TelemeteryDataDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDeviceStatusAsync(Guid deviceId, bool isOnline)
        {
            throw new NotImplementedException();
        }
        private void ValidateTelemetryData(TelemeteryDataDto dto)
        {
            if (dto.EventType == "temperature_humidity")
            {
                if (dto.Temperature == null || dto.Humidity == null)
                    throw new ArgumentException("Temperature and humidity are required for this event type");

                if (dto.Temperature < -50 || dto.Temperature > 100)
                    throw new ArgumentException("Temperature value is out of valid range");

                if (dto.Humidity < 0 || dto.Humidity > 100)
                    throw new ArgumentException("Humidity value is out of valid range");
            }
        }
    }

}

