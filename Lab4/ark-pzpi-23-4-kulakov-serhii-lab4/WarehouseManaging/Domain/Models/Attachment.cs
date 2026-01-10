using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string Path { get; set; } = null!;
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; } = null!;
    }
}
