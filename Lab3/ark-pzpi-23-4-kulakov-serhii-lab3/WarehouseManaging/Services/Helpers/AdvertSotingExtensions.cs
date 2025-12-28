using Domain.DateTrensferObjects;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class AdvertSotingExtensions
    {
        public static IEnumerable<AdvertDto> ApplySorting(this IEnumerable<AdvertDto> adverts, AdvertSortBy sortBy)
        {
            return sortBy switch
            {
                AdvertSortBy.PriceAsc => adverts.OrderBy(a => a.Warehouse.PricePerMonth),
                AdvertSortBy.PriceDesc => adverts.OrderByDescending(a => a.Warehouse.PricePerMonth),
                AdvertSortBy.ScaleAsc => adverts.OrderBy(a => a.Warehouse.Scale),
                AdvertSortBy.ScaleDesc => adverts.OrderByDescending(a => a.Warehouse.Scale),
                AdvertSortBy.FloorAsc => adverts.OrderBy(a => a.Warehouse.Floor),
                AdvertSortBy.FloorDesc => adverts.OrderByDescending(a => a.Warehouse.Floor),
                _ => adverts
            };
        }
    }
}
