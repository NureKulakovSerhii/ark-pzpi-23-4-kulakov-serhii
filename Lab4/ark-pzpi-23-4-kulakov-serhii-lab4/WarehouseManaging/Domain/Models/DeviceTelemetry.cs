using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DeviceTelemetry
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsDoorOpen { get; set; }
        public bool IsPowerOn { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public string EventType { get; set; } = "telemetry";
        public Guid DeviceId { get; set; }
        public WarehouseDevice Device {get;set;}
    }
}
