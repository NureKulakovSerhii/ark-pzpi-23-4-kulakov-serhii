using Data.DateTrensferObjects;
using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUserService
    {
        Task<User?> RegisterUserAsync(RegisterUserDto request);
        Task<TokenResponseDto?> LoginAsync(LoginUserDto request);
        Task<UserRolesDto> PromoteToModerator(Guid userId); 
        Task<TokenResponseDto?> RefreshTokens(RefreshTokenRequestDto requsest);
        Task<bool> LogoutAsync(Guid userId, string token);
    }
}
