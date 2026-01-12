using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class UpdateAdvertDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UpdateWarehouseDto? updateWarehouseDto { get; set; }
    }
}
