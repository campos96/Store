using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public enum Uom { Unit = 1001, Box = 1002, Bag = 1003, Can = 1004, Bottle = 1005 }

    public class Inventory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Uom Uom { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
