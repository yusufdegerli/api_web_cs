using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_for_sambapos.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; } = string.Empty;

        [Column("GroupCode")]
        public string? GroupCode { get; set; } = string.Empty;

        public List<MenuItemPortions>? Portions { get; set; } = new List<MenuItemPortions>();
        public List<MenuItemProperties>? Properties { get; set; } = new List<MenuItemProperties>(); // Yeni eklenen özellik

        [NotMapped]
        public decimal Price { get; set; }

        [NotMapped]
        public string Category { get; set; } = "Main";
    }
}