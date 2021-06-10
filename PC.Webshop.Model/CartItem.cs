using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC.Webshop.Model
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }        

        [Required]
        public int Amount { get; set; }

        [ForeignKey(nameof(Product))]
        public int? ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(Cart))]
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
