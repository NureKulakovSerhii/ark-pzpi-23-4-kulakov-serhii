using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public record UpdateProfileDto
        (string? Name, string? Surname, string? PhoneNumber, string SecondNumber);
}
