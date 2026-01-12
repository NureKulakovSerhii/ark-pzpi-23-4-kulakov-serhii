using AutoMapper;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper): ITicketService
    {
        public async Task<Guid> CreateTicketAsync(CreateTicketDto createTicketDto, Guid userId)
        {
            var ticket = new SupportTicket
            {
                Title = createTicketDto.Title,
                Description = createTicketDto.Description,
                Priority = createTicketDto.Priority,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                TicketStatus = TicketStatus.Open,
                AssignedToId = null
            };

            return await ticketRepository.CreateTicket(ticket);
        }
        public async Task DeleteTicketAsync(Guid ticketId)
        {
            var ticket = await ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
                throw new Exception("Ticket is not found");
            await ticketRepository.DeleteTicket(ticket.Id);
        }
        public async Task<TicketDetailsDto> GetTicketDetailsAsync(Guid ticketId)
        {
            var ticket = await ticketRepository.GetTicket(ticketId);
            if (ticket == null)
                throw new Exception("Тікет не знайдено");

            return mapper.Map<SupportTicket, TicketDetailsDto>(ticket);
        }
        public async Task AssignTicketAsync(Guid ticketId, Guid assigntdToId) 
        {
            var ticket = await ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
                throw new Exception("Тікет не знайдено");

            if (ticket.TicketStatus != TicketStatus.Open)
                throw new Exception("Тільки відкриті тікети можна призначити");

            var roles = await userRepository.GetUserRoles(assigntdToId);
            if (!roles.Contains("Moderator"))
                throw new Exception("Користувач не є модератором");

            ticket.AssignedToId = assigntdToId;
            ticket.TicketStatus = TicketStatus.InProgress;
            ticket.AnsweredAt = DateTime.UtcNow;

            await ticketRepository.UpdateTicket(ticket.Id);
        }
        public async Task CloseTicketAsync(Guid ticketId, Guid userId)
        {
            var ticket = await ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
                throw new Exception("Тікет не знайдено");

            if (ticket.UserId != userId)
                throw new Exception("Ви не можете закрити чужий тікет");

            if (ticket.TicketStatus == TicketStatus.Closed)
                throw new Exception("Тікет вже закритий");

            ticket.TicketStatus = TicketStatus.Closed;
            ticket.ClosedAt = DateTime.UtcNow;

            await ticketRepository.UpdateTicket(ticket.Id);
        }
        public async Task<List<TicketDto>> GetOpenTicketsAsync()
        {
            var tickets = await ticketRepository.GetOpenTickets();
            return mapper.Map<List<SupportTicket>, List<TicketDto>>(tickets);
        }
        public async Task<List<Guid>> GetTicketsForDeletionAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddHours(-24);
            return await ticketRepository.GetClosedTicketsOlderThan(cutoffDate);
        }
    }
}
