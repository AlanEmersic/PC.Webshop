using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PC.Webshop.DAL;
using PC.Webshop.Model;
using PC.Webshop.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PC.Webshop.Web.Controllers
{
    public class ProductController : Controller
    {
        private WebshopDbContext dbContext;

        public ProductController(WebshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index(string selected = null)
        {
            ViewData["selected"] = selected;          
            var query = dbContext.Products.Include(c => c.Category).Where(c => c.ID > 0).AsQueryable();
            var model = query.ToList();

            return View(nameof(Index), model);
        }

        [HttpPost]
        public IActionResult IndexAjax(ProductFilterModel filter)
        {
            string selected = (string)ViewData["selected"];            
            var query = dbContext.Products.Include(c => c.Category).Where(c => c.ID > 0).AsQueryable();
            filter ??= new ProductFilterModel();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

            if (selected != null)
                query = query.Where(p => p.Category.Name == selected);

            var model = query.ToList();

            return PartialView("_IndexTable", model);
        }

        public IActionResult Details(int? id = null)
        {
            var product = dbContext.Products.Include(c => c.Category)
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            FillDropdownValues();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product model)
        {            
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

        [Authorize(Roles = "Admin")]
        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = dbContext.Products.FirstOrDefault(p => p.ID == id);
            FillDropdownValues();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ID == id);
            var ok = await TryUpdateModelAsync(product);

            if (ok && ModelState.IsValid)
            {         
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            FillDropdownValues();
            return View();
        }

        [Authorize(Roles = "Admin")]
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
