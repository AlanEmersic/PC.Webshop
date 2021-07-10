using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.DAL;
using PC.Webshop.Model;
using System.Linq;
using System.Threading.Tasks;

namespace PC.Webshop.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private WebshopDbContext dbContext;
        UserManager<Customer> userManager;

        public CartController(WebshopDbContext dbContext, UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
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

            ViewData["sum"] = cart.CartItems.Sum(p => p.Product.Price * p.Amount);

            return View("Index", cart);
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var cartItem = dbContext.CartItems
                .Include(p => p.Product)
                .FirstOrDefault(p => p.ID == id);

            return View(cartItem);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var cartItem = dbContext.CartItems
                            .Include(p => p.Product)
                            .FirstOrDefault(p => p.ID == id);
            var ok = await TryUpdateModelAsync(cartItem);

            if (ok && ModelState.IsValid)
            {
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var cartItem = dbContext.CartItems.FirstOrDefault(p => p.ID == id);
            if (cartItem != null)
                dbContext.CartItems.Remove(cartItem);

            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
