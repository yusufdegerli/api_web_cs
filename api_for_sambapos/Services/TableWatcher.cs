using api_for_sambapos.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace api_for_sambapos.Services
{
    public class TableWatcher : BackgroundService
    {
        private readonly IServiceProvider _services;
        private Dictionary<int, (int TicketId, bool? IsPaid, bool? IsClosed)> _lastTableStates = new();

        public TableWatcher(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Mevcut masa ve bilet durumlarını al
                    var currentTables = await db.Tables
                        .Select(t => new { t.Id, t.TicketId })
                        .ToListAsync(stoppingToken);

                    var currentTickets = await db.Tickets
                        .Select(t => new { t.Id, t.IsPaid, t.IsClosed })
                        .ToDictionaryAsync(t => t.Id, t => new { t.IsPaid, t.IsClosed }, stoppingToken);

                    // Değişiklikleri kontrol et
                    foreach (var table in currentTables)
                    {
                        // TicketId 0 ise masa boş, aksi takdirde dolu
                        var ticket = table.TicketId != 0 ? currentTickets.GetValueOrDefault(table.TicketId) : null;
                        var currentState = (TicketId: table.TicketId, IsPaid: ticket?.IsPaid, IsClosed: ticket?.IsClosed);

                        // Son bilinen durumu al veya varsayılan değerler ata
                        if (!_lastTableStates.TryGetValue(table.Id, out var lastState))
                        {
                            lastState = (TicketId: 0, IsPaid: null, IsClosed: null);
                        }

                        // TicketId, IsPaid veya IsClosed değiştiyse
                        if (lastState.TicketId != currentState.TicketId ||
                            lastState.IsPaid != currentState.IsPaid ||
                            lastState.IsClosed != currentState.IsClosed)
                        {
                            // Eğer IsPaid true ise, IsClosed true yap
                            if (ticket != null && ticket.IsPaid && !ticket.IsClosed && table.TicketId != 0)
                            {
                                var ticketEntity = await db.Tickets.FindAsync(table.TicketId);
                                if (ticketEntity != null)
                                {
                                    ticketEntity.IsClosed = true;
                                    await db.SaveChangesAsync();
                                }
                            }

                            // WebSocket bildirimi gönder
                            var message = JsonSerializer.Serialize(new
                            {
                                type = "ticket_updated",
                                data = new
                                {
                                    tableId = table.Id,
                                    ticketId = table.TicketId,
                                    isPaid = ticket?.IsPaid ?? false,
                                    isClosed = ticket?.IsClosed ?? false
                                }
                            });
                            await WebSocketHandler.SendMessageToAllAsync(message);
                        }
                    }

                    // Son durumu güncelle
                    _lastTableStates = currentTables.ToDictionary(
                        t => t.Id,
                        t => (t.TicketId, currentTickets.GetValueOrDefault(t.TicketId)?.IsPaid, currentTickets.GetValueOrDefault(t.TicketId)?.IsClosed)
                    );
                }
                catch (Exception ex)
                {
                    ILogger<TableWatcher> logger = _services.GetRequiredService<ILogger<TableWatcher>>();
                    logger.LogError(ex, "TableWatcher hatası: {Message}", ex.Message);
                    Console.WriteLine($"TableWatcher hatası: {ex.Message}");
                }

                // 1 saniye bekle
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}