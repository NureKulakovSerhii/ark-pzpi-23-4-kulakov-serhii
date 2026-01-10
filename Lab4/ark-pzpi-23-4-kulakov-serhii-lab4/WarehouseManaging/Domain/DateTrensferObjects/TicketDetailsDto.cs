using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class TicketDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Priority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? ClosedAt { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public Guid? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public string? AssignedToEmail { get; set; }
        public List<CommentDto> Comments { get; set; } = null!;
    }
}
