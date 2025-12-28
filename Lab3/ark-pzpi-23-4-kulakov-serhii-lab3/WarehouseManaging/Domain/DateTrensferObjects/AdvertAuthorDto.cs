using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class AdvertAuthorDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
