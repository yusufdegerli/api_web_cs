using api_for_sambapos.Models;
using api_for_sambapos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_for_sambapos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<TicketItem>> PostTicketItem([FromBody] TicketItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticketItem = new TicketItem
            {
                TicketId = dto.TicketId,
                MenuItemId = dto.MenuItemId,
                MenuItemName = dto.MenuItemName,
                Price = dto.Price,
                Quantity = dto.Quantity,
                PortionName = dto.PortionName,
                CreatedDateTime = dto.CreatedDateTime ?? DateTime.UtcNow,
                CreatingUserId = dto.CreatingUserId,
                DepartmentId = 1,
                PortionCount = 1,
                ReasonId = 0,
                Gifted = false,
                Locked = true,
                ModifiedUserId = 0,
                ModifiedDateTime = DateTime.UtcNow,
                VatTemplateId = 0,
            };

            _context.TicketItems.Add(ticketItem);
            await _context.SaveChangesAsync();

            // WebSocket yayını
            var payload = new
            {
                type = "ticketitem_created",
                data = new
                {
                    ticketItem.Id,
                    ticketItem.TicketId,
                    ticketItem.MenuItemId,
                    ticketItem.MenuItemName,
                    ticketItem.Quantity,
                    ticketItem.Price,
                    ticketItem.PortionName,
                    ticketItem.CreatedDateTime,
                    ticketItem.Locked,
                    ticketItem.Voided,
                    ticketItem.OrderNumber,
                    ticketItem.VatRate,
                    ticketItem.VatAmount,
                    ticketItem.VatIncluded,
                    ticketItem.CreatingUserId,
                    ticketItem.DepartmentId,
                    ticketItem.PortionCount,
                    ticketItem.ReasonId,
                    ticketItem.Gifted,
                    ticketItem.ModifiedUserId,
                    ticketItem.ModifiedDateTime,
                    ticketItem.VatTemplateId,
                }
            };

            var message = JsonSerializer.Serialize(payload);
            await WebSocketHandler.SendMessageToAllAsync(message);

            return CreatedAtAction(nameof(GetTicketItem), new { id = ticketItem.Id }, ticketItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketItem>> GetTicketItem(int id)
        {
            var ticketItem = await _context.TicketItems.FindAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            return ticketItem;
        }
        [HttpGet("byTicketId/{ticketId}")]
        public async Task<ActionResult<List<TicketItem>>> GetTicketItemsByTicketId(int ticketId)
        {
            var ticketItems = await _context.TicketItems
                .Where(ti => ti.TicketId == ticketId)
                .ToListAsync();

            if (ticketItems == null || ticketItems.Count == 0)
            {
                return NotFound();
            }

            return ticketItems;
        }
    }

}
