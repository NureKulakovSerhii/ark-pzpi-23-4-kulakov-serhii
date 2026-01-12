using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Advert
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<ModerationTask> ModerationTasks { get; set; } = new();
        public List<UserFavoriteAdvert> FavoriteByUser { get; set; } = new();

    }
}
