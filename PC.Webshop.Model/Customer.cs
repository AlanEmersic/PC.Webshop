using System.ComponentModel.DataAnnotations;

namespace PC.Webshop.Model
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Unesite barem 3 znaka")]
        public string Username { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Unesite barem 5 znaka")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
