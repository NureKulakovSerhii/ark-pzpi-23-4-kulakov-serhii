using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class TicketsQueryFiltersDto
    {
        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
        public string SortColumn { get; set; } = "CreatedAt";
        public string Order { get; set; } = "desc";
        public Priority? Priority { get; set; }
        public TicketStatus? Status { get; set; }
        public Guid? AsignedToId { get; set; }
        public Guid? UserId { get; set; }
    }
}
