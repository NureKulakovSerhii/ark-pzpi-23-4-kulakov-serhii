using Data.DateTrensferObjects;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Models;

using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;


namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginUserDto request)
        {
            var userExists = await _userRepository.GetUserByEmailAsync(request.UserEmail);
            if (userExists == null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword
                (userExists, userExists.Password, request.UserPassword) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(userExists);
        }

        public async Task<User?> RegisterUserAsync(RegisterUserDto request)
        {
            var userExists = await _userRepository.GetUserByEmailAsync(request.UserEmail);
            if (userExists != null)
            {
                return null;
            }
            var user = new User();
            var hashPassword = new PasswordHasher<User>()
                .HashPassword(user, request.UserPassword);

            user.Name = request.UserName;
            user.Surname = request.UserLastName;
            user.Email = request.UserEmail;
            user.Password = hashPassword;

            await _userRepository.CreateUserAsync(user);
            return user;
        }
        
        public async Task<UserRolesDto> PromoteToModerator(Guid userId)
        {
            var userExists = await _userRepository.GiveModeratorRole(userId);
            return userExists;
        }
        
        public async Task<TokenResponseDto?> RefreshTokens(RefreshTokenRequestDto requsest)
        {
            var user = await ValidateRefreshToken(requsest.UserId, requsest.RefreshToken);
            if (user is null)
                return null;
            return await CreateTokenResponse(user);
        }

        private async Task<string> CreateJwtToken(User user)
        {
            var userRoles = await _userRepository.GetUserRoles(user.Id);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),

            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration.GetSection("Appsettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetSection("Appsettings:Issuer").Value,
                audience: _configuration.GetSection("Appsettings:Audience").Value,
                signingCredentials: creds,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60)
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var token = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddDays(3);
            await _userRepository.ReplaceRefreshTokenAsync(user.Id, token, expiresAt);
            return token;
        }

        private async Task<User> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null) { return null; }
            var rt = await _userRepository.GetRefreshToken(userId, refreshToken);
            if (rt == null || rt.Token != refreshToken ||
                rt.ExpiresAt < DateTime.UtcNow) { return null; }
            return user;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? userExists)
        {
            return new TokenResponseDto
            {
                JwtToken = await CreateJwtToken(userExists),
                RefreshToken = await GenerateAndSaveRefreshToken(userExists)
            };
        }

        public async Task<bool> LogoutAsync(Guid userId, string token)
        {
            return await _userRepository.RemoveRefreshToken(userId, token);
        }
    }
}
