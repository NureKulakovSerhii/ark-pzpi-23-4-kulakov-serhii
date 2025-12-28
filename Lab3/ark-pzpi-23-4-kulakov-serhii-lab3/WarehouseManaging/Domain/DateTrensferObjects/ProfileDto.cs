using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set;  } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? SecondPhoneNumber { get; set; } = string.Empty;
        public List<AdvertDto> UserAdverts { get; set; } = new();
    }
}
