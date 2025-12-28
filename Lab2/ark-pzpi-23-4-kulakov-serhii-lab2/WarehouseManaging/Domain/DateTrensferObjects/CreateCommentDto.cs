using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public record CreateCommentDto
    {
        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string Text { get; init; } = string.Empty;
    }
}
