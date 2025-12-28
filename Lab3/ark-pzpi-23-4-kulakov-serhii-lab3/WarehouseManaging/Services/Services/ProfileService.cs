using AutoMapper;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProfileService(IProfileRepository profileRepository, IUserRepository userRepository, IMapper mapper)
        : IProfileService
    {
        public async Task<ProfileDto> GetUserByIdAsync(Guid userId)
        {
            var user = await profileRepository.GetUserById(userId);
            if(user == null)
            {
                throw new Exception("User is not found in database");
            }
            return mapper.Map<User,ProfileDto>(user);    
        }

        public async Task UpdateUserProfileAsync(Guid userId, UpdateProfileDto profileDto)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User is not found in database");
            if(profileDto.Name != null) user.Name = profileDto.Name!;
            if(profileDto.Surname != null) user.Surname = profileDto.Surname!;
            if(profileDto.PhoneNumber != null) user.PhoneNumber = profileDto.PhoneNumber!;
            if(profileDto.SecondNumber != null) user.SecondPhoneNumber = profileDto.SecondNumber!;
            await profileRepository.UpdateUserProfile(user);
        }
    }
}
