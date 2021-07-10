using Microsoft.AspNetCore.Mvc;
using PC.Webshop.DAL;
using System.Collections.Generic;
using System.Linq;

namespace PC.Webshop.Web.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : Controller
    {
        WebshopDbContext dbContext;

        public ProductApiController(WebshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = dbContext.Products
                .Select(product => new ProductDTO()
                {
                    ID = product.ID,
                    Name = product.Name,
                    Description = product.Description,
                    Brand = product.Brand,
                    Price = product.Price,
                    SerialNumber = product.SerialNumber,
                    Category = new CategoryDTO()
                    {
                        ID = dbContext.Categories.Where(c => c.Name.ToLower() == product.Category.Name.ToLower()).First().ID,
                        Name = product.Category.Name
                    },
                    Img = product.Img
                })
                .ToList();

            return Ok(products);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            ProductDTO product = dbContext.Products
                .Where(c => c.ID == id)
               .Select(product => new ProductDTO()
               {
                   ID = product.ID,
                   Name = product.Name,
                   Description = product.Description,
                   Brand = product.Brand,
                   Price = product.Price,
                   SerialNumber = product.SerialNumber,
                   Category = new CategoryDTO()
                   {
                       ID = dbContext.Categories.Where(c => c.Name.ToLower() == product.Category.Name.ToLower()).First().ID,
                       Name = product.Category.Name
                   },
                   Img = product.Img
               })
               .FirstOrDefault();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Route("search/{query}")]
        [HttpGet]
        public IActionResult Get(string query)
        {
            List<ProductDTO> products = null;

            if (!string.IsNullOrWhiteSpace(query))
                products = dbContext.Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                .Select(product => new ProductDTO()
                {
                    ID = product.ID,
                    Name = product.Name,
                    Description = product.Description,
                    Brand = product.Brand,
                    Price = product.Price,
                    SerialNumber = product.SerialNumber,
                    Category = new CategoryDTO()
                    {
                        ID = dbContext.Categories.Where(c => c.Name.ToLower() == product.Category.Name.ToLower()).First().ID,
                        Name = product.Category.Name
                    },
                    Img = product.Img
                })
                .ToList();

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductDTO product)
        {
            var newProduct = new Model.Product
            {
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                SerialNumber = product.SerialNumber,
                CategoryId = product.Category.ID,
                Img = product.Img
            };

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();

            return Get(newProduct.ID);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var product = dbContext.Products.FirstOrDefault(c => c.ID == id);
            if (product != null)
                dbContext.Products.Remove(product);

            dbContext.SaveChanges();
            return Ok();
        }
    }

    public class ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public float Price { get; set; }
        public string SerialNumber { get; set; }
        public CategoryDTO Category { get; set; }
        public string Img { get; set; }
    }

    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
