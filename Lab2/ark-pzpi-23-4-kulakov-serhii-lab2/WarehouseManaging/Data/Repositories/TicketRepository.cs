using Data.DB;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketRepository(AppDbContext appDbContext) : ITicketRepository
    {
        public async Task<Guid> CreateTicket(SupportTicket supportTicket)
        {
            appDbContext.SupportTickets.Add(supportTicket);
            await appDbContext.SaveChangesAsync();
            return supportTicket.Id;
        }

        public async Task DeleteTicket(Guid ticketId)
        {
            var ticket = await appDbContext.SupportTickets.FindAsync(ticketId);
            appDbContext.Remove(ticket);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<SupportTicket>> GetOpenTickets()
        {
            var tickets = await appDbContext.SupportTickets
                .Include(t => t.User)
                .Include(t => t.Comments)
                .Include(t => t.AssignedTo)
                .Where(t => t.TicketStatus == TicketStatus.Open)
                .ToListAsync();
            return tickets;
        }

        public async Task<SupportTicket> GetTicket(Guid ticketId)
        {
            var ticket = await appDbContext.SupportTickets.Include(t => t.User)
                .Include(t => t.Comments).Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == ticketId);
            return ticket;
        }

        public async Task<SupportTicket> GetTicketById(Guid ticketId)
        {
            return await appDbContext.SupportTickets.FindAsync(ticketId);
        }

        public async Task<(int total, List<SupportTicket>)> GetTicketsWithCount(TicketsQueryFiltersDto filters)
        {
            var query = appDbContext.SupportTickets.Include(st => st.User)
                .Include(st => st.AssignedTo)
                .Include(st => st.Comments)
                .AsQueryable();
            if (filters.Status != null)
            {
                query = query.Where(t => t.TicketStatus == filters.Status);
            }
            if(filters.Priority != null)
            {
                query = query.Where(t => t.Priority == filters.Priority);
            }
            if(filters.UserId != null)
            {
                query = query.Where(t => t.UserId == filters.UserId);
            }
            if(filters.AsignedToId != null)
            {
                query = query.Where(t => t.AssignedToId == filters.AsignedToId);
            }
            var ticketsCount = await query.CountAsync();
            if(!string.IsNullOrEmpty(filters.SortColumn) && !string.IsNullOrEmpty(filters.Order))
            {
                if(filters.Order.ToLower() == "asc")
                {
                    query = query.OrderBy(t => EF.Property<object>(t, filters.SortColumn));
                }
                else if(filters.Order.ToLower() == "desc")
                {
                    query = query.OrderByDescending(t => EF.Property<object>(t, filters.SortColumn));
                }
            }
            var tickets = await query.Skip(filters.Skip).Take(filters.Take).ToListAsync();
            return (ticketsCount, tickets);
        }

        public async Task UpdateTicket(Guid ticketId)
        {
            var ticket = await appDbContext.SupportTickets.FindAsync(ticketId);
            appDbContext.Update(ticket);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<List<Guid>> GetClosedTicketsOlderThan(DateTime date)
        {
            return await appDbContext.SupportTickets
                .Where(t => t.TicketStatus == TicketStatus.Closed && t.ClosedAt < date)
                .Select(t => t.Id)
                .ToListAsync();
        }
    }
}
