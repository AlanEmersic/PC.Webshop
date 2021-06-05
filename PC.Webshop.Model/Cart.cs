using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC.Webshop.Model
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }

        //[Required]
        //public AppUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
