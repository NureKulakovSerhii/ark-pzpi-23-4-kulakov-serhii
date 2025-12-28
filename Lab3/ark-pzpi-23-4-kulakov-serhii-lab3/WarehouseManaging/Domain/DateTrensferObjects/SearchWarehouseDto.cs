using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public record SearchWarehouseDto(int? pricePerMonthMin, int? pricePerMonthMax,
        int? minScale, int? maxScale, int? minFloor, int? maxFloor,
        BuildingType? BuildingType, City? City,
        List<Communications>? Communications,
        List<HouseholdAppliances>? HouseholdAppliances,
        List<Infrastructure>? Infrastructures);
}
