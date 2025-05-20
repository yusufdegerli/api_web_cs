using System.ComponentModel.DataAnnotations;
using api_for_sambapos.Models;

namespace api_for_sambapos.Models
{
    public class MenuItemPropertyGroups
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int? Order { get; set; }
        public List<MenuItemProperties>? Properties { get; set; } = new List<MenuItemProperties>();
       // public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public bool SingleSelection { get; set; }
        public bool MultipleSelection { get; set; }
    }
}
