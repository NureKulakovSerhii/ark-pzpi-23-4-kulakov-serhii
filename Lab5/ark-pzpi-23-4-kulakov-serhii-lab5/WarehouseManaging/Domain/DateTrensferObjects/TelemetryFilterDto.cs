using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class TelemetryFilterDto
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? EventType { get; set; }
        public int Limit { get; set; } = 100;
    }
}
