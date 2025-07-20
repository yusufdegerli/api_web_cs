using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api_for_sambapos.Models
{
    public class Tables
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public string? Category { get; set; } = string.Empty;
        //public int? _ticketId;
        public int TicketId { get; set; } // Nullable int for TicketId
        /*{
            get => _ticketId;
            set => _ticketId = (value == 0) ? null : value;
        }*/
        [JsonIgnore]
        public Ticket? Ticket { get; set; } // Navigation property
    }
}
