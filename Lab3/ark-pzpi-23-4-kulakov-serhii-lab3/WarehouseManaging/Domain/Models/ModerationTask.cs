using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ModerationTask
    {
        public Guid Id { get; set; }
        public string TargetEntity { get; set; }
        = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public ModerationStatus ModerationStatus { get; set; }
        public Guid AdministratorId { get; set; }
        public Guid AdvertId { get; set; }
        public User Administrator { get; set; }
        public Advert Advert { get; set; }
    }
}
