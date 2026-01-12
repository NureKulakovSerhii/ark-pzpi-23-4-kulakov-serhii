using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class TelemeteryDataDto
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;
        [Required]
        public string SecretKey { get; set; } = string.Empty;
        public bool? IsDoorOpen { get; set; }
        public bool? IsPowerOn { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        [Required]
        public string EventType { get; set; } = "telemetry";
    }
}
