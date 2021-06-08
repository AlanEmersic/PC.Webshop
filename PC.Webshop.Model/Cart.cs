﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC.Webshop.Model
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }

        //[Required]
        //public AppUser User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }        
    }
}
