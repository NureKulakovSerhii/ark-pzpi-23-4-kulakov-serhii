using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class TokenResponseDto
    {
        public required string JwtToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
