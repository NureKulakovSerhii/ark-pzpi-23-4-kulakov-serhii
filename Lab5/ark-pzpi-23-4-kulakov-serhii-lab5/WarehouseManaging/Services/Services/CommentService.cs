using AutoMapper;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateCommentAsync(
            Guid ticketId,
            CreateCommentDto createCommentDto,
            Guid userId)
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
                throw new Exception("Тікет не знайдено");

            if (ticket.TicketStatus == TicketStatus.Closed)
                throw new Exception("Тікет закритий. Ви не можете залишити повідомлення");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Text = createCommentDto.Text,
                CreatedAt = DateTimeOffset.UtcNow,
                UserId = userId,
                TicketId = ticketId,
            };

            var commentId = await _commentRepository.CreateComment(comment);
            var userRoles = await _userRepository.GetUserRoles(userId);
            if (ticket.AnsweredAt == null && userRoles.Contains("Moderator"))
            {
                ticket.AnsweredAt = DateTime.UtcNow;
                await _ticketRepository.UpdateTicket(ticket.Id);
            }

            return commentId;
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            if (comment == null)
                throw new Exception("Коментар не знайдено");

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetTicketCommentsAsync(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
                throw new Exception("Тікет не знайдено");

            var comments = await _commentRepository.GetTicketComments(ticketId);
            return _mapper.Map<List<CommentDto>>(comments);
        }
    }
}