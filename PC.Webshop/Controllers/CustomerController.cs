using Microsoft.AspNetCore.Mvc;
using PC.Webshop.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PC.Webshop.Web.Controllers
{
    public class CustomerController : Controller
    {
        private WebshopDbContext dbContext;

        public CustomerController(WebshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index(int? id = null)
        {
            id = 1;
            var customer = dbContext.Customers
                .Where(c => c.ID == id).FirstOrDefault();

            return View("Index", customer);
        }
    }
}
