using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.DAL;
using System.Linq;
using System.Threading.Tasks;

namespace PC.Webshop.Web.Controllers
{
    public class CartController : Controller
    {
        private WebshopDbContext dbContext;

        public CartController(WebshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index(int? id = null)
        {
            id = 1;
            var cart = dbContext.Carts.Include(c => c.Customer)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.ID == id).FirstOrDefault();

            return View("Index", cart);
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            //var userId = userManager.GetUserId(User);

            var cartItem = dbContext.CartItems
                .Include(p => p.Product)
                .FirstOrDefault(p => p.ID == id);
            //FillDropdownValues();
            return View(cartItem);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            //var userId = userManager.GetUserId(User);

            var cartItem = dbContext.CartItems
                .Include(p => p.Product)
                .FirstOrDefault(p => p.ID == id);
            var ok = await TryUpdateModelAsync(cartItem);

            if (ok && ModelState.IsValid)
            {
                //product.UpdatedById = userId;
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            //FillDropdownValues();
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
