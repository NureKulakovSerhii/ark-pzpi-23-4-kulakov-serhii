using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> CreateWarehouse(Warehouse warehouse);
        Task<Warehouse> GetWarehouseByIdAsync(Guid warehouseId);
        Task DeleteWarehouse(Guid warehouseId);
        Task<Warehouse?> GetWarehouseByAddress(string address);
        Task<List<Warehouse>>? SearchWarehouses(SearchWarehouseDto searchWarehouseDto);
        Task UpdateWarehouse(Warehouse warehouse);
    }
}
