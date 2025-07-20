namespace api_for_sambapos.Models
{
    public class TicketItemDto
    {
        public int TicketId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string PortionName { get; set; } = string.Empty;
        /**/
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;
        public int ModifiedUserId { get; set; } = 0;
        public int PortionCount { get; set; } = 1;
        public bool Locked { get; set; } = true;
        public bool Voided { get; set; } = false;
        public bool Gifted { get; set; } = false;
        public int OrderNumber { get; set; }
        public decimal VatRate { get; set; } = 0.00M;
        public decimal VatAmount { get; set; } = 0.00M;
        public bool VatIncluded { get; set; } = false;
        public int VatTemplateId { get; set; } = 0;
        public int ReasonId { get; set; } = 0;
        /**/
        public DateTime? CreatedDateTime { get; set; }
        public int CreatingUserId { get; set; }
        public int DepartmentId { get; set; } = 1;
    }

}
