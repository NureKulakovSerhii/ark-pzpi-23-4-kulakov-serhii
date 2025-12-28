using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IProfileRepository
    {
        Task<User> GetUserById(Guid userId);
        Task UpdateUserProfile(User user);
    }
}
