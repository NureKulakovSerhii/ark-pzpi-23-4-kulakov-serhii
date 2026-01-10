using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public int PricePerMonth { get; set; }
        public int Scale { get; set; }
        public int Floor { get; set; }
        public BuildingType BuildingType { get; set; }
        public City City { get; set; }
        public string? ImageUrl { get; set; }

        public WarehouseDevice? Device { get; set; }
        public List<Communications> Communications { get; set; } = new();

        public List<HouseholdAppliances> HouseholdAppliances { get; set; } = new();

        public List<Infrastructure> Infrastructures { get; set; } = new();

        public List<Advert> Adverts { get; set; } = new();
    }
}
