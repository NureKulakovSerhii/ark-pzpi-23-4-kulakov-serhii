using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class UpdateWarehouseDto
    {
        public int PricePerMonth { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<Communications> Communications { get; set; } = new();

        public List<HouseholdAppliances> HouseholdAppliances { get; set; } = new();

        public List<Infrastructure> Infrastructures { get; set; } = new();
    }
}
