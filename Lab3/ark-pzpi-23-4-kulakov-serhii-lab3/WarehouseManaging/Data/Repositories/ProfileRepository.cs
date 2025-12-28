using Data.DB;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProfileRepository(AppDbContext appDbContext) : IProfileRepository
    {
        public async Task<User> GetUserById(Guid userId)
        {
            var user = await appDbContext.Users
                .Include(u => u.UserAdverts)
                .ThenInclude(a => a.Warehouse)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task UpdateUserProfile(User user)
        {
            appDbContext.Users.Update(user);
            await appDbContext.SaveChangesAsync();
        }
    }
}
