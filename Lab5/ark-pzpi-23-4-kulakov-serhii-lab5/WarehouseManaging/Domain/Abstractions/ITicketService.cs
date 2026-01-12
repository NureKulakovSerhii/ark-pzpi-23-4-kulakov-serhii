using Domain.DateTrensferObjects;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ITicketService
    {
        Task<Guid> CreateTicketAsync(CreateTicketDto createTicketDto, Guid userId);
        Task DeleteTicketAsync(Guid ticketId);
        Task AssignTicketAsync(Guid ticketId, Guid assigntdToId);
        Task<TicketDetailsDto> GetTicketDetailsAsync(Guid ticketId);
        Task CloseTicketAsync(Guid ticketId, Guid userId);
        Task<List<TicketDto>> GetOpenTicketsAsync();
        Task<List<Guid>> GetTicketsForDeletionAsync();
    }
}
