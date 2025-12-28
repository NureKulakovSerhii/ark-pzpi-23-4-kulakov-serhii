using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class UpdateTicketDto
    {
        public TicketStatus TicketStatus { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? AssignedToId { get; set; }
        public Priority Priority { get; set; }
    }
}
