using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PC.Webshop.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using PC.Webshop.Model;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.Web.Models;

namespace PC.Webshop.Web.Controllers
{
    public class ProductController : Controller
    {
        private WebshopDbContext dbContext;

        public ProductController(WebshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index(ProductFilterModel filter)
        {
            //var query = dbContext.Products.Include(c => c.Category).AsQueryable();
            var query = dbContext.Products.Include(c => c.Category).Where(c => c.ID > 0).AsQueryable();

            filter ??= new ProductFilterModel();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

            var model = query.ToList();            
            return View("Index", model);
        }

        public IActionResult Details(int? id = null)
        {
            var product = dbContext.Products.Include(c => c.Category)
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return View(product);
        }

        //[Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            FillDropdownValues();
            return View();
        }

        //[Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult Create(Product model)
        {
            //var userId = userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                dbContext.Products.Add(model);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                FillDropdownValues();
                return View();
            }
        }

        //[Authorize(Roles = "Admin,Manager")]
        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            //var userId = userManager.GetUserId(User);

            var model = dbContext.Products.FirstOrDefault(p => p.ID == id);
            this.FillDropdownValues();
            return View(model);
        }

        //[Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            //var userId = userManager.GetUserId(User);

            var product = dbContext.Products.FirstOrDefault(p => p.ID == id);
            var ok = await this.TryUpdateModelAsync(product);

            if (ok && this.ModelState.IsValid)
            {
                //product.UpdatedById = userId;
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            FillDropdownValues();
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ID == id);
            if (product != null)
                dbContext.Products.Remove(product);

            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();

            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in dbContext.Categories)
            {
                listItem = new SelectListItem(category.Name, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleCategories = selectItems;
        }
    }
}
