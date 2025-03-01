using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_using_crud_with_swagger.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ID otomatik artan bir değer olacak.
        public int Id { get; set; } // Eğer tabloda farklı bir ID varsa, burayı değiştir.

        public string Name { get; set; } = string.Empty;  // Varsayılan değer
        public string LocationName { get; set; } = string.Empty;  // Varsayılan değer
        public string CustomerName { get; set; } = string.Empty;  // Varsayılan değer
        public int DepartmentId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime LastUpdateTime { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        // Yapıcı metodu burada her bir özelliği set etmenize gerek yok çünkü varsayılan değerler zaten veriliyor
        // Yapıcıyı kaldırabilirsiniz veya sadece istenen özellikler için değer sağlayabilirsiniz
    }
}
