using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_for_sambapos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.Json;
using api_for_sambapos.Services;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<TicketsController> _logger;

    public TicketsController(AppDbContext context, ILogger<TicketsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
        try
        {
            return await _context.Tickets
                .Include(t => t.Tables)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Name = t.Name != null ? t.Name : "No Name",
                    TicketNumber = t.TicketNumber != null ? t.TicketNumber : "No Number",
                    CustomerName = t.CustomerName != null ? t.CustomerName : "No Customer",
                    RemainingAmount = t.RemainingAmount,
                    TotalAmount = t.TotalAmount,
                    Note = t.Note != null ? t.Note : "No Notes",
                    Tag = t.Tag != null ? t.Tag : "No Tags"
                })
                .ToListAsync();
        }
        catch (SqlException ex)
        {
            return StatusCode(500, $"Veritabanı hatası: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        try
        {
            var ticket = await _context.Tickets
                .Include(t => t.Tables)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Name = t.Name ?? "No Name",
                    TicketNumber = t.TicketNumber ?? "No Number",
                    CustomerName = t.CustomerName ?? "No Customer",
                    RemainingAmount = t.RemainingAmount,
                    TotalAmount = t.TotalAmount,
                    Note = t.Note ?? "No Notes",
                    Tag = t.Tag ?? "No Tags",
                    DepartmentId = t.DepartmentId,
                    LastUpdateTime = t.LastUpdateTime,
                    Date = t.Date,
                    LastOrderDate = t.LastOrderDate,
                    LastPaymentDate = t.LastPaymentDate ?? default,
                    LocationName = t.LocationName ?? "No Location",
                    CustomerId = t.CustomerId,
                    CustomerGroupCode = t.CustomerGroupCode,
                    IsPaid = t.IsPaid,
                    Locked = t.Locked,
                    TableId = t.Tables.FirstOrDefault() != null ? t.Tables.FirstOrDefault()!.Id : null,
                    Tables = t.Tables.Select(table => new TableDto
                    {
                        Id = table.Id,
                        Name = table.Name ?? "No Name",
                        Order = table.Order,
                        Category = table.Category ?? "No Category",
                        TicketId = table.TicketId
                    }).ToList()
                })
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            return ticket;
        }
        catch (SqlException ex)
        {
            return StatusCode(500, $"Veritabanı hatası: {ex.Message}");
        }
    }

    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<Ticket>> PostTicket([FromBody] TicketDto ticketDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var minDate = new DateTime(1753, 1, 1);
            var maxDate = new DateTime(9999, 12, 31);

            if (ticketDto.Date == default)
                ticketDto.Date = DateTime.UtcNow;

            if (!ticketDto.LastUpdateTime.HasValue || ticketDto.LastUpdateTime == default)
                ticketDto.LastUpdateTime = DateTime.UtcNow;

            if (ticketDto.LastOrderDate == default)
                ticketDto.LastOrderDate = DateTime.UtcNow;

            if (ticketDto.LastPaymentDate == default)
                ticketDto.LastPaymentDate = DateTime.UtcNow;

            if (ticketDto.Date < minDate || ticketDto.Date > maxDate)
                return BadRequest($"Date {minDate} ve {maxDate} arasında olmalıdır. Gönderilen: {ticketDto.Date}");

            if (ticketDto.LastUpdateTime.HasValue &&
                (ticketDto.LastUpdateTime.Value < minDate || ticketDto.LastUpdateTime.Value > maxDate))
                return BadRequest($"LastUpdateTime {minDate} ve {maxDate} arasında olmalıdır. Gönderilen: {ticketDto.LastUpdateTime}");

            if (ticketDto.LastOrderDate < minDate || ticketDto.LastOrderDate > maxDate)
                return BadRequest($"LastOrderDate {minDate} ve {maxDate} arasında olmalıdır. Gönderilen: {ticketDto.LastOrderDate}");

            if (ticketDto.LastPaymentDate < minDate || ticketDto.LastPaymentDate > maxDate)
                return BadRequest($"LastPaymentDate {minDate} ve {maxDate} arasında olmalıdır. Gönderilen: {ticketDto.LastPaymentDate}");

            Console.WriteLine($"▶️ POST içindeki TicketNumber (orijinal): '{ticketDto.TicketNumber}'");

            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var maxTicketNumber = await _context.Tickets
                        .OrderByDescending(t => t.TicketNumber)
                        .Select(t => t.TicketNumber)
                        .FirstOrDefaultAsync();

                    int newTicketNumber = 1;
                    if (int.TryParse(maxTicketNumber, out int lastNumber))
                        newTicketNumber = lastNumber + 1;

                    ticketDto.TicketNumber = newTicketNumber.ToString();

                    var ticket = new Ticket
                    {
                        Date = ticketDto.Date < minDate || ticketDto.Date > maxDate ? DateTime.UtcNow : ticketDto.Date,
                        LastUpdateTime = ticketDto.LastUpdateTime.HasValue && (ticketDto.LastUpdateTime.Value < minDate || ticketDto.LastUpdateTime.Value > maxDate)
                            ? DateTime.UtcNow
                            : ticketDto.LastUpdateTime ?? DateTime.UtcNow,
                        LastOrderDate = ticketDto.LastOrderDate < minDate || ticketDto.LastOrderDate > maxDate ? DateTime.UtcNow : ticketDto.LastOrderDate,
                        LastPaymentDate = ticketDto.LastPaymentDate < minDate || ticketDto.LastPaymentDate > maxDate
                            ? DateTime.UtcNow
                            : ticketDto.LastPaymentDate,
                        TicketNumber = ticketDto.TicketNumber,
                        DepartmentId = ticketDto.DepartmentId,
                        LocationName = ticketDto.LocationName ?? "No Location",
                        CustomerId = ticketDto.CustomerId,
                        CustomerName = ticketDto.CustomerName ?? "No Customer",
                        Note = ticketDto.Note ?? "No Notes",
                        IsPaid = ticketDto.IsPaid,
                        TotalAmount = ticketDto.TotalAmount,
                        RemainingAmount = ticketDto.RemainingAmount,
                        Name = ticketDto.Name ?? "No Name",
                        Locked = ticketDto.Locked,
                        IsClosed = false,
                        TableId = ticketDto.TableId
                    };

                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();

                    if (ticketDto.TableId.HasValue)
                    {
                        var tableEntity = await _context.Tables.FindAsync(ticketDto.TableId.Value);
                        if (tableEntity != null)
                        {
                            tableEntity.TicketId = ticket.Id;
                            await _context.SaveChangesAsync();

                            // WebSocket bildirimi
                            var updateMessage = JsonSerializer.Serialize(new
                            {
                                type = "ticket_updated",
                                data = new { tableId = tableEntity.Id, ticketId = ticket.Id, isPaid = ticket.IsPaid, isClosed = ticket.IsClosed }
                            });
                            await WebSocketHandler.SendMessageToAllAsync(updateMessage);
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            return NotFound($"Table with ID {ticketDto.TableId.Value} not found.");
                        }
                    }

                    var payload = new
                    {
                        type = "ticket_created",
                        data = new
                        {
                            ticket.Id,
                            ticket.TicketNumber,
                            ticket.LocationName,
                            ticket.TotalAmount,
                            ticket.IsPaid,
                            ticket.IsClosed,
                            ticket.LastUpdateTime
                        }
                    };
                    var message = JsonSerializer.Serialize(payload);
                    await WebSocketHandler.SendMessageToAllAsync(message);

                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
        catch (SqlException ex)
        {
            return StatusCode(500, $"Veritabanı hatası: {ex.Message}");
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<Ticket>> CreateTicket([FromBody] TicketDto ticketDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var minDate = new DateTime(1753, 1, 1);

            if (ticketDto.Date < minDate) ticketDto.Date = DateTime.UtcNow;
            if (!ticketDto.LastUpdateTime.HasValue || ticketDto.LastUpdateTime < minDate)
                ticketDto.LastUpdateTime = DateTime.UtcNow;
            if (ticketDto.LastOrderDate < minDate) ticketDto.LastOrderDate = DateTime.UtcNow;
            if (ticketDto.LastPaymentDate < minDate) ticketDto.LastPaymentDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(ticketDto.TicketNumber) || ticketDto.TicketNumber == "0")
            {
                var allTicketNumbers = await _context.Tickets
                    .Select(t => t.TicketNumber)
                    .ToListAsync();

                int maxNumber = 0;
                foreach (var number in allTicketNumbers)
                {
                    if (int.TryParse(number, out int currentNumber) && currentNumber > maxNumber)
                        maxNumber = currentNumber;
                }

                ticketDto.TicketNumber = (maxNumber + 1).ToString();
            }

            var ticket = new Ticket
            {
                Date = ticketDto.Date,
                LastUpdateTime = ticketDto.LastUpdateTime ?? DateTime.UtcNow,
                LastOrderDate = ticketDto.LastOrderDate,
                LastPaymentDate = ticketDto.LastPaymentDate,
                TicketNumber = ticketDto.TicketNumber,
                DepartmentId = ticketDto.DepartmentId,
                LocationName = ticketDto.LocationName ?? "No Location",
                CustomerId = ticketDto.CustomerId,
                CustomerName = ticketDto.CustomerName ?? "No Customer",
                Note = ticketDto.Note ?? "No Notes",
                IsPaid = ticketDto.IsPaid,
                TotalAmount = ticketDto.TotalAmount,
                RemainingAmount = ticketDto.RemainingAmount,
                Name = ticketDto.Name ?? "No Name",
                Locked = ticketDto.Locked,
                IsClosed = false
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Internal server error",
                details = ex.Message,
                innerException = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("open")]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetOpenTickets()
    {
        try
        {
            var openTickets = await _context.Tickets
                .Where(t => !t.IsClosed)
                .Select(t => new Ticket
                {
                    Id = t.Id,
                    Name = t.Name ?? "No Name",
                    DepartmentId = t.DepartmentId,
                    LastUpdateTime = t.LastUpdateTime,
                    TicketNumber = t.TicketNumber ?? "No Number",
                    PrintJobData = t.PrintJobData,
                    Date = t.Date,
                    LastOrderDate = t.LastOrderDate,
                    LastPaymentDate = t.LastPaymentDate,
                    LocationName = t.LocationName ?? "No Location",
                    CustomerId = t.CustomerId,
                    CustomerName = t.CustomerName ?? "No Customer",
                    CustomerGroupCode = t.CustomerGroupCode,
                    IsPaid = t.IsPaid,
                    RemainingAmount = t.RemainingAmount,
                    TotalAmount = t.TotalAmount,
                    Note = t.Note ?? "No Notes",
                    Locked = t.Locked,
                    Tag = t.Tag ?? "No Tags",
                    IsClosed = t.IsClosed
                })
                .ToListAsync();

            return Ok(openTickets);
        }
        catch (SqlNullValueException ex)
        {
            _logger.LogError(ex, "Tickets tablosundaki null değerle karşılaşıldı");
            return StatusCode(500, "Sunucu iç hatası");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTicket(int id, [FromBody] TicketDto ticketDto)
    {
        if (id <= 0 || ticketDto == null)
        {
            return BadRequest("Geçersiz istek.");
        }

        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound($"ID {id} olan ticket bulunamadı.");
        }

        try
        {
            // Güncellenebilir alanları buraya ekle
            ticket.TicketNumber = ticketDto.TicketNumber ?? ticket.TicketNumber;
            ticket.LastUpdateTime = DateTime.UtcNow;
            ticket.LastOrderDate = ticketDto.LastOrderDate;
            ticket.LastPaymentDate = ticketDto.LastPaymentDate;
            ticket.IsPaid = ticketDto.IsPaid;
            ticket.TotalAmount = ticketDto.TotalAmount;
            ticket.RemainingAmount = ticketDto.RemainingAmount;
            ticket.Note = ticketDto.Note ?? ticket.Note;
            ticket.CustomerName = ticketDto.CustomerName ?? ticket.CustomerName;
            ticket.Locked = ticketDto.Locked;

            // Eğer IsPaid true ise, IsClosed true yapılır ve ilgili masaların TicketId sıfırlanır
            if (ticketDto.IsPaid)
            {
                ticket.IsClosed = true;
                var relatedTables = await _context.Tables.Where(t => t.TicketId == ticket.Id).ToListAsync();
                foreach (var relatedTable in relatedTables)
                {
                    relatedTable.TicketId = 0; // Masanın TicketId'sini sıfırla (Problem çıkartabilir 0 olması)
                }
            }

            await _context.SaveChangesAsync();

            // WebSocket üzerinden mesaj gönder
            var payload = new
            {
                type = "ticket_updated",
                data = new
                {
                    ticket.Id,
                    ticket.TicketNumber,
                    ticket.LocationName,
                    ticket.TotalAmount,
                    ticket.IsPaid,
                    ticket.IsClosed,
                    ticket.LastUpdateTime,
                    tableId = (await _context.Tables.FirstOrDefaultAsync(t => t.TicketId == ticket.Id))?.Id
                }
            };

            var message = JsonSerializer.Serialize(payload);
            await WebSocketHandler.SendMessageToAllAsync(message);

            return Ok(ticket);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}