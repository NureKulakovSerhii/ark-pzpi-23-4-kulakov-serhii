using Data.DB;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository(AppDbContext appDbContext) : ICommentRepository
    {
        public async Task<Guid> CreateComment(Comment comment)
        {
            appDbContext.Comments.Add(comment);
            await appDbContext.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<Comment?> GetCommentById(Guid commentId)
        {
            var comment = await appDbContext.Comments
                .Include(c => c.User)
                .Include(c => c.Ticket)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == commentId);
            return comment;
        }

        public async Task<List<Comment>> GetTicketComments(Guid ticketId)
        {
            var comments = await appDbContext.Comments
                .Include(c => c.User)
                .Include(c => c.Attachments)
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return comments;
        }
    }
}
