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
    [Route("api/support")]
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
    }
}