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
    public class CarouselController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public CarouselController(EshopDbContext eshopDB, IWebHostEnvironment env)
        {
            this.env = env;
            eshopDbContext = eshopDB;
        }

        public IActionResult Select()
        {
            IList<CarouselItem> carouselItems = eshopDbContext.CarouselItems.ToList();
            return View(carouselItems);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarouselItem carouselItem)
        {
            if (carouselItem != null && carouselItem.Image != null)
            {

                FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/CarouselItems", "image");
                carouselItem.ImageSource = await fileUpload.FileUploadAsync(carouselItem.Image);

                ModelState.Clear();
                TryValidateModel(carouselItem);
                if (ModelState.IsValid)
                {
                    eshopDbContext.CarouselItems.Add(carouselItem);

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(CarouselController.Select));


                }
            }
            return View(carouselItem);


        }
        public IActionResult Edit(int ID)
        {
            CarouselItem ciFromDatabase = eshopDbContext.CarouselItems.FirstOrDefault(ci => ci.ID == ID);

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
        public async Task<IActionResult> Edit(CarouselItem carouselItem)
        {
            CarouselItem ciFromDatabase = eshopDbContext.CarouselItems.FirstOrDefault(ci => ci.ID == carouselItem.ID);

            if (ciFromDatabase != null)
            {

                if (carouselItem != null && carouselItem.Image != null)
                {

                    FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/CarouselItems", "image");
                    carouselItem.ImageSource = await fileUpload.FileUploadAsync(carouselItem.Image);

                    if (String.IsNullOrWhiteSpace(carouselItem.ImageSource) == false)
                    {
                        ciFromDatabase.ImageSource = carouselItem.ImageSource;
                    }
                }
                else
                {
                    carouselItem.ImageSource = ":-]";
                }

                ModelState.Clear();
                TryValidateModel(carouselItem);
                if (ModelState.IsValid)
                {
                    ciFromDatabase.ImageAlt = carouselItem.ImageAlt;

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(CarouselController.Select));
                }
                return View(carouselItem);
            }
            else
            {
                return NotFound();
            }


        }

        public async Task<IActionResult> Delete(int ID)
        {
            DbSet<CarouselItem> carouselItems = eshopDbContext.CarouselItems;
            CarouselItem carouselItem = carouselItems.Where(carouselItem => carouselItem.ID == ID).FirstOrDefault();

            if (carouselItem != null)
            {
                carouselItems.Remove(carouselItem);

                await eshopDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CarouselController.Select));
        }
    }
}
