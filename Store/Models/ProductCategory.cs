using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

    }
}
