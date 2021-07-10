using Microsoft.AspNetCore.Identity;

namespace PC.Webshop.Model
{
    public class Customer : IdentityUser
    {        
        public virtual Cart Cart { get; set; }
    }
}
