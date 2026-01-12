using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<UserRolesDto> GiveModeratorRole(Guid userId);
        Task<List<string>> GetUserRoles(Guid userId);
        Task<RefreshToken?> GetRefreshToken(Guid userId, string token);
        Task ReplaceRefreshTokenAsync(Guid userId, string token, DateTime expiresAt);
        Task<bool> RemoveRefreshToken(Guid userId, string token);
    }
}
