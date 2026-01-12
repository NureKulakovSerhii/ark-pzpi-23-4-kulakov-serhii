using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class DeviceAuthRequest
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;
        [Required]
        public string SecretKey { get; set; } = string.Empty;
    }
}
