using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_for_sambapos.Models
{
    public class TicketItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; } = null!;

        [Required]
        public int MenuItemId { get; set; }

        public string MenuItemName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public string PortionName { get; set; } = string.Empty;
        /**/
        public int ModifiedUserId { get; set; } = 0;
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;
        public int PortionCount { get; set; } = 1;
        public bool Locked { get; set; } = true;
        public bool Voided { get; set; } = false;
        public bool Gifted { get; set; } = false;
        public int OrderNumber { get; set; }
        public decimal VatRate { get; set; } = 0.00M;
        public decimal VatAmount { get; set; } = 0.00M;
        public bool VatIncluded{ get; set; } = false;
        public int VatTemplateId { get; set; } = 0;

        public int ReasonId { get; set; } = 0;
        /**/

        public DateTime CreatedDateTime { get; set; }

        public int CreatingUserId { get; set; }

        public int DepartmentId { get; set; } = 1;
    }
}
