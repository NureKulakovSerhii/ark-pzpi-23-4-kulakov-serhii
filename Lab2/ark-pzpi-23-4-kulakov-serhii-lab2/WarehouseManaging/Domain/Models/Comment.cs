using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
        public User User { get; set; } = null!;
        public SupportTicket Ticket { get; set; } = null!;
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
