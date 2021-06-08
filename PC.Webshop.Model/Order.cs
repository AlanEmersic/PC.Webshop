using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.Webshop.Model
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
