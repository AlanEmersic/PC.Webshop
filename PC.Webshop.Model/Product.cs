using System.Collections.Generic;
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

        [Required]
        public string SerialNumber { get; set; }

        public string Img { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
