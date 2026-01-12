using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        public Guid TicketId { get; set; }
        public string TicketTitle { get; set; } = null!;
        public List<AttachmentDto> Attachments { get; set; } = null!;
    }
}
