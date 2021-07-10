using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PC.Webshop.DAL;
using PC.Webshop.Model;
using System.Linq;

namespace PC.Webshop.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private WebshopDbContext dbContext;
        UserManager<Customer> userManager;

        public CustomerController(WebshopDbContext dbContext, UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            Customer customer = dbContext.Customers
                .Where(c => c.Id == userManager.GetUserId(User)).FirstOrDefault();

            return View("Index", customer);
        }
    }
}
