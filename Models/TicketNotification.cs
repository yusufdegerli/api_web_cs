public class TicketNotification
{
    public int TicketId { get; set; }
    public string Action { get; set; } = string.Empty; // "created", "updated", "deleted"
    public DateTime Timestamp { get; set; }
}

public class TableNotification
{
    public int TableId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
