using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupportController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ICommentService _commentService;

        public SupportController(ITicketService ticketService, ICommentService commentService)
        {
            _ticketService = ticketService;
            _commentService = commentService;
        }

        [HttpPost("create-ticket")]
        public async Task<ActionResult<Guid>> CreateTicket([FromBody] CreateTicketDto createTicketDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ticketId = await _ticketService.CreateTicketAsync(createTicketDto, userId);

            return Ok(new
            {
                id = ticketId,
                message = "Тікет успішно створено"
            });
        }

        [HttpGet("get-opened-tickets")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<List<TicketDto>>> GetOpenTickets()
        {
            var tickets = await _ticketService.GetOpenTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("get-ticket-by-id/{ticketId}")]
        public async Task<ActionResult<TicketDetailsDto>> GetTicket([FromRoute] Guid ticketId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isModerator = User.IsInRole("Moderator");

            var ticket = await _ticketService.GetTicketDetailsAsync(ticketId);
            if (!isModerator && ticket.UserId != userId)
            {
                return Forbid();
            }
            else if (isModerator && ticket.AssignedToId != null && ticket.AssignedToId != userId)
            {
                return Forbid();
            }

            return Ok(ticket);
        }
        [HttpPost("{ticketId}/assign")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> AssignToMe([FromRoute] Guid ticketId)
        {
            var moderatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _ticketService.AssignTicketAsync(ticketId, moderatorId);

            return Ok(new { message = "Тікет призначено вам" });
        }
        [HttpPatch("{ticketId}/close")]
        public async Task<ActionResult> CloseTicket([FromRoute] Guid ticketId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _ticketService.CloseTicketAsync(ticketId, userId);

            return Ok(new { message = "Тікет закрито" });
        }
        [HttpDelete("delete-ticket/{ticketId}")]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult> DeleteTicket([FromRoute] Guid ticketId)
        {
            await _ticketService.DeleteTicketAsync(ticketId);
            return Ok(new { message = "Тікет видалено" });
        }
        [HttpPost("create-comment")]
        public async Task<ActionResult> CreateComment(
           [FromRoute] Guid ticketId,
           [FromBody] CreateCommentDto createCommentDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isModerator = User.IsInRole("Moderator");

            var ticket = await _ticketService.GetTicketDetailsAsync(ticketId);
            if (ticket == null)
                return NotFound("Тікет не знайдено");

            // Перевіряємо доступ
            bool hasAccess = false;
            if (!isModerator)
            {
                hasAccess = (ticket.UserId == userId);
            }
            else if (isModerator)
            {
                hasAccess = (ticket.AssignedToId == userId || ticket.AssignedToId == null);
            }

            if (!hasAccess)
            {
                return Forbid();
            }

            try
            {
                var commentId = await _commentService.CreateCommentAsync(
                    ticketId,
                    createCommentDto,
                    userId);

                return Ok(new
                {
                    id = commentId,
                    message = "Коментар додано"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{commentId}/get-comment-by-id")]
        public async Task<ActionResult<CommentDto>> GetCommentById(
            [FromRoute] Guid ticketId,
            [FromRoute] Guid commentId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isModerator = User.IsInRole("Moderator");

            var ticket = await _ticketService.GetTicketDetailsAsync(ticketId);
            if (ticket == null)
                return NotFound("Тікет не знайдено");
            bool hasAccess = false;
            if (!isModerator)
            {
                hasAccess = (ticket.UserId == userId);
            }
            else if (isModerator)
            {
                hasAccess = (ticket.AssignedToId == userId);
            }

            if (!hasAccess)
            {
                return Forbid();
            }
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(commentId);

                if (comment.TicketId != ticketId)
                {
                    return BadRequest("Коментар не належить цьому тікету");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("get-comments")]
        public async Task<ActionResult<List<CommentDto>>> GetComments([FromRoute] Guid ticketId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isModerator = User.IsInRole("Moderator");

            var ticket = await _ticketService.GetTicketDetailsAsync(ticketId);
            if (ticket == null)
                return NotFound("Тікет не знайдено");
            bool hasAccess = false;
            if (!isModerator)
            {
                hasAccess = (ticket.UserId == userId);
            }
            else if (isModerator)
            {
                hasAccess = (ticket.AssignedToId == userId);
            }

            if (!hasAccess)
            {
                return Forbid();
            }

            try
            {
                var comments = await _commentService.GetTicketCommentsAsync(ticketId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}