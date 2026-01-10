using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class FilteredTicketsDto
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public long Total { get; set; }
        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }
}
