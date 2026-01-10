using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? SecondPhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<UserRole> UserRoles { get; set; } = new();
        public List<Advert> UserAdverts { get; set; } = new();
        public List<ModerationTask> ModerationTasks { get; set; } = new();
        public List<SupportTicket> CreateTickets { get; set; } = new();
        public List<SupportTicket> AssignedTickets { get; set; } = new();
        public List<Comment> Comments { get; set; } = new ();
        public List<UserFavoriteAdvert> FavoriteAdverts { get; set; } = new();
    }
}

