namespace api_for_sambapos.Models
{
    public class TableDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Category { get; set; } = string.Empty;
        public int TicketId { get; set; }
    }
}
