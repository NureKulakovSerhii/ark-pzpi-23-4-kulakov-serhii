using Data.DB;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class WarehouseRepository(AppDbContext appDbContext) : IWarehouseRepository
    {
        public async Task<Warehouse> CreateWarehouse(Warehouse warehouse)
        {
            appDbContext.Warehouses.Add(warehouse);
            await appDbContext.SaveChangesAsync();
            return warehouse;
        }

        public async Task DeleteWarehouse(Guid warehouseId)
        {
            var warehouse = await appDbContext.Warehouses.FindAsync(warehouseId);
            if(warehouse != null)
            {
                appDbContext.Warehouses.Remove(warehouse);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Warehouse?> GetWarehouseByAddress(string address)
        {
            var warehouse = await appDbContext.Warehouses
                .FirstOrDefaultAsync(w => w.Address.Trim().ToLower().Equals(address.Trim().ToLower()));
            if (warehouse != null)
                return warehouse;
            return null;
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(Guid warehouseId)
        {
            var warehouse = await appDbContext.Warehouses.FindAsync(warehouseId);
            return warehouse;
        }

        public async Task<List<Warehouse>>? SearchWarehouses(SearchWarehouseDto searchWarehouseDto)
        {
            var query = appDbContext.Warehouses.Include(w => w.Adverts).ThenInclude(a => a.User).AsQueryable();
            if (searchWarehouseDto.pricePerMonthMin.HasValue)
            {
                query = query.Where(w => w.PricePerMonth > searchWarehouseDto.pricePerMonthMin);
            }
            if (searchWarehouseDto.pricePerMonthMax.HasValue)
            {
                query = query.Where(w => w.PricePerMonth < searchWarehouseDto.pricePerMonthMax);
            }
            if (searchWarehouseDto.minScale.HasValue)
            {
                query = query.Where(w => w.Scale > searchWarehouseDto.minScale);
            }
            if (searchWarehouseDto.maxScale.HasValue)
            {
                query = query.Where(w => w.Scale < searchWarehouseDto.maxScale);
            }
            if (searchWarehouseDto.minFloor.HasValue)
            {
                query = query.Where(w => w.Floor > searchWarehouseDto.minFloor);
            }
            if (searchWarehouseDto.maxFloor.HasValue)
            {
                query = query.Where(w => w.Floor < searchWarehouseDto.maxFloor);
            }
            if (searchWarehouseDto.BuildingType.HasValue)
            {
                query = query.Where(w => w.BuildingType == searchWarehouseDto.BuildingType);
            }
            if (searchWarehouseDto.City.HasValue)
            {
                query = query.Where(w => w.City == searchWarehouseDto.City);
            }
            if(searchWarehouseDto.Communications != null && searchWarehouseDto.Communications.Any())
            {
                foreach(var communication in searchWarehouseDto.Communications)
                {
                    query = query.Where(w => w.Communications.Contains(communication));
                }
            }
            if(searchWarehouseDto.HouseholdAppliances != null && searchWarehouseDto.HouseholdAppliances.Any())
            {
                foreach(var householdAppliance in searchWarehouseDto.HouseholdAppliances)
                {
                    query = query.Where(w => w.HouseholdAppliances.Contains(householdAppliance));
                }
            }
            if(searchWarehouseDto.Infrastructures != null && searchWarehouseDto.Infrastructures.Any())
            {
                foreach(var infrastructure in searchWarehouseDto.Infrastructures)
                {
                    query = query.Where(w => w.Infrastructures.Contains(infrastructure));
                }
            }
            var warehouses = await query.ToListAsync();
            return warehouses;
        }

        public async Task UpdateWarehouse(Warehouse warehouse)
        {
            appDbContext.Warehouses.Update(warehouse);
            await appDbContext.SaveChangesAsync();
        }
    }
}
