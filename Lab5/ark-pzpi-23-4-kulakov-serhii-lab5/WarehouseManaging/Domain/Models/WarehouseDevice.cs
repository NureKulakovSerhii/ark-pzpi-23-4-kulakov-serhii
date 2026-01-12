using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class WarehouseDevice
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public DateTime LastSeen { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
