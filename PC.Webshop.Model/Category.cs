using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC.Webshop.Model
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
