using Domain.DateTrensferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IProfileService
    {
        Task<ProfileDto> GetUserByIdAsync(Guid userId);
        Task UpdateUserProfileAsync(Guid userId, UpdateProfileDto profileDto);
    }
}
