using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perutka.Eshop.Web.Models.database;
using Perutka.Eshop.Web.Models.Entity;
using Perutka.Eshop.Web.Models.Identity;
using Perutka.Eshop.Web.Models.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perutka.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class ProductsController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public ProductsController(EshopDbContext eshopDB, IWebHostEnvironment env)
        {
            this.env = env;
            eshopDbContext = eshopDB;
        }

        public IActionResult Select()
        {
            IList<Product> Products = eshopDbContext.Products.ToList();
            return View(Products);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (product != null && product.Image != null)
            {

                FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Products", "image");
                product.ImageSource = await fileUpload.FileUploadAsync(product.Image);

                ModelState.Clear();
                TryValidateModel(product);
                if (ModelState.IsValid)
                {
                    eshopDbContext.Products.Add(product);

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(ProductsController.Select));


                }
            }
            return View(product);


        }
        public IActionResult Edit(int ID)
        {
            Product ciFromDatabase = eshopDbContext.Products.FirstOrDefault(ci => ci.ID == ID);

            if (ciFromDatabase != null)
            {
                return View(ciFromDatabase);

            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            Product ciFromDatabase = eshopDbContext.Products.FirstOrDefault(ci => ci.ID == product.ID);

            if (ciFromDatabase != null)
            {

                if (product != null && product.Image != null)
                {

                    FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Products", "image");
                    product.ImageSource = await fileUpload.FileUploadAsync(product.Image);

                    if (String.IsNullOrWhiteSpace(product.ImageSource) == false)
                    {
                        ciFromDatabase.ImageSource = product.ImageSource;
                    }
                }
                else
                {
                    product.ImageSource = ":-]";
                }

                ModelState.Clear();
                TryValidateModel(product);
                if (ModelState.IsValid)
                {
                    ciFromDatabase.ImageAlt = product.ImageAlt;

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(ProductsController.Select));
                }
                return View(product);
            }
            else
            {
                return NotFound();
            }


        }

        public async Task<IActionResult> Delete(int ID)
        {
            DbSet<Product> products = eshopDbContext.Products;
            Product product = products.Where(product => product.ID == ID).FirstOrDefault();

            if (product != null)
            {
                products.Remove(product);

                await eshopDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ProductsController.Select));
        }
    }
}



