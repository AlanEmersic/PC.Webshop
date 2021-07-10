using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC.Webshop.Model
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
