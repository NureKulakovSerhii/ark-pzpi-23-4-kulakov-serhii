using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ITicketRepository
    {
        Task<Guid> CreateTicket(SupportTicket supportTicket);
        Task UpdateTicket(Guid ticketId);
        Task DeleteTicket(Guid ticketId);
        Task<SupportTicket> GetTicket(Guid ticketId);
        Task<(int total, List<SupportTicket>)> GetTicketsWithCount(TicketsQueryFiltersDto filters);
        Task<SupportTicket> GetTicketById(Guid ticketId);
        Task<List<SupportTicket>> GetOpenTickets();
        Task<List<Guid>> GetClosedTicketsOlderThan(DateTime date);
    }
}
