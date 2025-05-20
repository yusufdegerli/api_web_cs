using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_for_sambapos.Models;
using api_for_sambapos.Services;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly AppDbContext _context;

    public TableController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tables>>> GetTables()
    {
        return await _context.Tables
            .Select(m => new Tables
            {
                Id = m.Id,
                Name = m.Name ?? string.Empty,
                Order = m.Order,
                Category = m.Category ?? string.Empty,
                TicketId = m.TicketId
            })
            .ToListAsync();
    }

    [HttpGet("{category}")]
    public async Task<ActionResult<IEnumerable<Tables>>> GetTablesByCategory(string category)
    {
        return await _context.Tables
            .Where(m => m.Category == category)
            .Select(m => new Tables
            {
                Id = m.Id,
                Name = m.Name ?? string.Empty,
                Order = m.Order,
                Category = m.Category ?? string.Empty,
                TicketId = m.TicketId
            })
            .ToListAsync();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTableTicketId(int id, [FromBody] TableUpdateDto updateDto)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table == null)
        {
            return NotFound($"Masa bulunamadı: ID = {id}");
        }

        table.TicketId = updateDto.TicketId;
        await _context.SaveChangesAsync();

        // WebSocket bildirimi
        var updateMessage = JsonSerializer.Serialize(new
        {
            type = "ticket_updated",
            data = new { tableId = table.Id, ticketId = table.TicketId }
        });
        await WebSocketHandler.SendMessageToAllAsync(updateMessage);

        return Ok(new { message = "Masa güncellendi", table });
    }


    // api/TableController.cs içinde
    [HttpGet("byName/{name}")]
    public async Task<ActionResult<int>> GetTableIdByName(string name)
    {
        var id = await _context.Tables
            .Where(t => t.Name == name)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

        if (id == 0)
            return NotFound($"Masa bulunamadı: {name}");

        return Ok(id);
    }

    // Bu metod [FromBody] attribute'ü eklenerek düzeltildi
    [HttpPut("moveTicket")]
    public async Task<IActionResult> MoveTicket([FromBody] MoveTicketRequest request)
    {
        var oldTable = await _context.Tables.FindAsync(request.OldTableId);
        if (oldTable == null)
        {
            return NotFound($"Eski masa bulunamadı: ID = {request.OldTableId}");
        }

        var newTable = await _context.Tables.FindAsync(request.NewTableId);
        if (newTable == null)
        {
            return NotFound($"Yeni masa bulunamadı: ID = {request.NewTableId}");
        }

        oldTable.TicketId = 0; // Eski masanın TicketId'sini null yap
        newTable.TicketId = request.TicketId; // Yeni masanın TicketId'sini güncelle
        await _context.SaveChangesAsync();

        var updateMessage = JsonSerializer.Serialize(new
        {
            type = "ticket_updated",
            data = new { tableId = newTable.Id, ticketId = newTable.TicketId }
        });
        await WebSocketHandler.SendMessageToAllAsync(updateMessage);

        return Ok("Fiş başarıyla taşındı");
    }
}

// İstek parametrelerini tanımlayan sınıf
public class MoveTicketRequest
{
    public int OldTableId { get; set; }
    public int NewTableId { get; set; }
    public int TicketId { get; set; }
}