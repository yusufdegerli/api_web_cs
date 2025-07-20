using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_for_sambapos.Models
{
    public class MenuItemProperties
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int? MenuItemId { get; set; } // MenuItem ile ilişki
        public MenuItem? MenuItem { get; set; } // Navigation property

        [Column("MenuItemPropertyGroup_Id")]
        public int? MenuItemPropertyGroupId { get; set; } // MenuItemPropertyGroups ile ilişki
        public MenuItemPropertyGroups? MenuItemPropertyGroup { get; set; } // Navigation property
    }
}
/*
 * PAYMENST TABLOSU: SİPARİŞİ/ÖDEMESİ ALINAN SİPARİŞİ, FİYATIYLA USER IDSİYLE KAYDEDER
 */
