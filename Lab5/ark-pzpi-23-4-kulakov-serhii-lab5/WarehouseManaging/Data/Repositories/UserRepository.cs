using Data.DB;
using Domain.Abstractions;
using Domain.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DateTrensferObjects;

namespace Data.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User> CreateUserAsync(User user)
        {
            var userRoleEntity = await appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == "User");

            if(userRoleEntity == null)
            {
                throw new Exception("Не знайдена роль User");
            }

            if (!user.UserRoles.Any())
            {
                user.UserRoles.Add(new UserRole
                {
                    RoleId = userRoleEntity.Id,
                    AssignedAt = DateTime.UtcNow,
                    UserId = user.Id
                });
            }
            appDbContext.Users.Add(user);
            await appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var userExists = await appDbContext.Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
            return userExists;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var userExists = await appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            return userExists;
        }

        public async Task<UserRolesDto> GiveModeratorRole(Guid userId)
        {
            var userExists = await appDbContext.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userExists == null) { return null; }

            var userRoleEntity = await appDbContext.Roles
               .FirstOrDefaultAsync(r => r.Name == "Moderator");
            if (userRoleEntity == null){
                throw new Exception("Не знайдена роль Moderator");
            }

            var userIsAlreadyModerator = userExists.UserRoles.Any(ur => ur.RoleId == userRoleEntity.Id);
            if (!userIsAlreadyModerator)
            {
                userExists.UserRoles.Add(new UserRole
                {
                    RoleId = userRoleEntity.Id,
                    AssignedAt = DateTime.UtcNow,
                    UserId = userExists.Id
                });
                await appDbContext.SaveChangesAsync();
            }
            return new UserRolesDto { UserId = userId, RoleNames = userExists.UserRoles.Select(ur => ur.Role.Name).ToList() };
        }
        public async Task<List<string>> GetUserRoles(Guid userId)
        {
            var user = await appDbContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == userId);
            if(user != null)
            {
                var userRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
                return userRoles;
            }
            return new();
        }
        public async Task<RefreshToken?> GetRefreshToken(Guid userId, string token)
        {
            var tokenDb = await appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == token);
            return tokenDb;
        }

        public async Task ReplaceRefreshTokenAsync(Guid userId, string token, DateTime expiresAt)
        {
            var tokenExists = await appDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if(tokenExists != null)
            {
                tokenExists.Token = token;
                tokenExists.ExpiresAt = expiresAt;
                appDbContext.RefreshTokens.Update(tokenExists);
            }
            else
            {
                await appDbContext.RefreshTokens.AddAsync(new RefreshToken
                {
                    UserId = userId,
                    Token = token,
                    ExpiresAt = expiresAt
                });
            }
            await appDbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveRefreshToken(Guid userId, string token)
        {
            var refreshtoken = await appDbContext.RefreshTokens.FirstOrDefaultAsync(
                rt => rt.UserId == userId && rt.Token == token);
            if (refreshtoken == null)
                return false;
            appDbContext.RefreshTokens.Remove(refreshtoken);
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
