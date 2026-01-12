using Data.DB;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class IotDeviceRepository(AppDbContext appDbContext) : IIotDeviceRepository
    {
        public async Task AddTelemetryAsync(DeviceTelemetry telemetry)
        {
            appDbContext.DeviceTelemetries.Add(telemetry);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<WarehouseDevice?> GetDeviceByCredentialsAsync(string deviceId, string secretKey)
        {
            return await appDbContext.WarehouseDevices.FirstOrDefaultAsync
                (d => d.DeviceId == deviceId && d.SecretKey == secretKey);
        }

        public async Task<WarehouseDevice?> GetDeviceByIdAsync(Guid deviceId)
        {
            return await appDbContext.WarehouseDevices.FindAsync(deviceId);
        }

        public async Task<DeviceTelemetry?> GetLatestTelemetryAsync(Guid deviceId, string? eventType = null)
        {
            var query = appDbContext.DeviceTelemetries.Where(t => t.DeviceId == deviceId);
            if (!string.IsNullOrEmpty(eventType))
                query = query.Where(t => t.EventType == eventType);
            return await query.OrderByDescending(t => t.Timestamp).FirstOrDefaultAsync();
        }

        public async Task<List<DeviceTelemetry>> GetTelemetryByDeviceAsync(Guid deviceId, DateTime? from, DateTime? to, string? eventType = null, int limit = 100)
        {
            var query = appDbContext.DeviceTelemetries.Where(t => t.DeviceId == deviceId);
            if (from.HasValue)
                query = query.Where(t => t.Timestamp >= from.Value);
            if (to.HasValue)
                query = query.Where(t => t.Timestamp <= to.Value);
            if (!string.IsNullOrWhiteSpace(eventType))
                query = query.Where(t => t.EventType == eventType);
            return await query.Take(limit).ToListAsync();
        }

        public async Task<List<WarehouseDevice>> GetUserDevicesAsync(Guid userId)
        {
            return await appDbContext.WarehouseDevices
                .Include(d => d.Warehouse)
                .ThenInclude(w => w.Adverts)
                .Where(d => d.Warehouse.Adverts.Any(a => a.UserId == userId)).ToListAsync();
        }

        public async Task UpdateDeviceStatusAsync(Guid deviceId, DateTime lastSeen, bool isOnline)
        {
            var device = await appDbContext.WarehouseDevices.FindAsync(deviceId);
            if(device != null)
            {
                device.LastSeen = lastSeen;
                device.IsOnline = isOnline;
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UserHasAccessToDeviceAsync(Guid userId, Guid deviceId)
        {
            return await appDbContext.WarehouseDevices
               .Include(d => d.Warehouse)
               .ThenInclude(w => w.Adverts)
               .AnyAsync(d => d.Id == deviceId && d.Warehouse.Adverts.Any(a => a.UserId == userId));
        }
    }
}
