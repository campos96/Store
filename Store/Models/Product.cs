using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Brand { get; set; }

        public string? Sku { get; set; }
        
        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }

        public string? Model { get; set; }

        public byte[]? FrontPhoto { get; set; }
        
        public byte[]? RearPhoto { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public ProductCategory? Category { get; set; }

    }
}
