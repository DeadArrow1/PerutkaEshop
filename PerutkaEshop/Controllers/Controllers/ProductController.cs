using Microsoft.AspNetCore.Mvc;
using Perutka.Eshop.Web.Models.database;
using Perutka.Eshop.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perutka.Eshop.Web.Controllers
{
    public class ProductController : Controller
    {
        readonly EshopDbContext eshopDbContext;

        public ProductController(EshopDbContext eshopDbContext)
        {
            this.eshopDbContext = eshopDbContext;
        }
        public IActionResult Detail(int id)
        {
            Product product = eshopDbContext.Products.FirstOrDefault(ei => ei.ID == id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }
    }
}
