using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.DAL;
using PC.Webshop.Model;
using System.Linq;

namespace PC.Webshop.Web.Controllers
{
    [Authorize]
    public class CartItemController : Controller
    {
        private WebshopDbContext dbContext;
        UserManager<Customer> userManager;

        public CartItemController(WebshopDbContext dbContext, UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public IActionResult Add(int id)
        {
            string customerId = userManager.GetUserId(User);

            Cart checkCart = dbContext.Carts.Include(c => c.Customer)
                .Where(c => c.CustomerId == customerId).FirstOrDefault();

            if (checkCart == null)
            {
                Cart newCart = new Cart() { CustomerId = customerId };
                dbContext.Carts.Add(newCart);
                dbContext.SaveChanges();
            }

            Cart cart = dbContext.Carts.Include(c => c.Customer)
              .Include(c => c.CartItems)
              .ThenInclude(ci => ci.Product)
              .ThenInclude(p => p.Category)
              .Where(c => c.CustomerId == customerId).FirstOrDefault();

            CartItem cartItem = new CartItem()
            {
                Amount = 1,
                CartId = cart.ID,
                ProductId = dbContext.Products.Include(c => c.Category).Where(p => p.ID == id).FirstOrDefault().ID
            };

            if (cart.CartItems.Any(ci => ci.ProductId == cartItem.ProductId))
            {                
                CartItem sameItem = cart.CartItems.Where(ci => ci.ProductId == cartItem.ProductId).FirstOrDefault();
                sameItem.Amount++;
                dbContext.SaveChanges();
            }
            else
            {
                dbContext.CartItems.Add(cartItem);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
