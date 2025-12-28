using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SupportTicket
    {
        public Guid Id { get; set; }
        public Priority Priority { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? AnsweredAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public Guid? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
