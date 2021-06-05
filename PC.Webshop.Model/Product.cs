using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC.Webshop.Model
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public float Price { get; set; }

        public string SerialNumber { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(Cart))]
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
