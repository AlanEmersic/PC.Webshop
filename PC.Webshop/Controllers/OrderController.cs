using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.DAL;
using PC.Webshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PC.Webshop.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private WebshopDbContext dbContext;
        UserManager<Customer> userManager;

        public OrderController(WebshopDbContext dbContext, UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var query = dbContext.Orders.Include(c => c.Cart).ThenInclude(c => c.Customer).Where(c => c.ID > 0).AsQueryable();
            var model = query.ToList();

            return View(nameof(Index), model);
        }


        public IActionResult OrderProducts(int id)
        {
            string customerId = userManager.GetUserId(User);

            Cart cart = dbContext.Carts.Include(c => c.Customer)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Category)
            .Where(c => c.CustomerId == customerId).FirstOrDefault();

            Order order = new Order()
            {
                CartId = cart.ID,
                OrderDate = DateTime.Now
            };

            dbContext.Orders.Add(order);
            foreach (var item in cart.CartItems)
            {
                dbContext.CartItems.Remove(item);
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
    }
}
