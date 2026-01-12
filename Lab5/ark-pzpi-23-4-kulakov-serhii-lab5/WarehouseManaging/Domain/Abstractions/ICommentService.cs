using Domain.DateTrensferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ICommentService
    {
        Task<Guid> CreateCommentAsync(Guid ticketId, CreateCommentDto createCommentDto, Guid userId);
        Task<CommentDto> GetCommentByIdAsync(Guid commentId);
        Task<List<CommentDto>> GetTicketCommentsAsync(Guid ticketId);
    }
}