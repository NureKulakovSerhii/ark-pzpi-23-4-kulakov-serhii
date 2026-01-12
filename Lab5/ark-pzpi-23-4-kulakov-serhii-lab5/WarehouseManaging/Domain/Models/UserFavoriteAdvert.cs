using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserFavoriteAdvert
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Advert Advert { get; set; }
        public Guid AdvertId { get; set; }
        public DateTime AddedAt { get; set; }
        public int Order { get; set; }
    }
}
