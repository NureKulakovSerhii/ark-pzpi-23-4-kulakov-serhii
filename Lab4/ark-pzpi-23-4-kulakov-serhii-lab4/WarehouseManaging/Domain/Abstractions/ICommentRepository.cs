using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ICommentRepository
    {
        Task<Guid> CreateComment(Comment comment);
        Task<Comment?> GetCommentById(Guid commentId);
        Task<List<Comment>> GetTicketComments(Guid ticketId);
    }
}
