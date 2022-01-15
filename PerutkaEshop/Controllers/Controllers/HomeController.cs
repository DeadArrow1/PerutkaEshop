using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perutka.Eshop.Web.Models;
using Perutka.Eshop.Web.Models.database;
using Perutka.Eshop.Web.Models.Entity;
using Perutka.Eshop.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Perutka.Eshop.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,EshopDbContext eshopDB)
        {
            eshopDbContext = eshopDB;
            _logger = logger;

        }

       

        public IActionResult Index()
        {
            _logger.LogInformation("Byla zobrazena hlavni stranka");
            IndexViewModel indexVM = new IndexViewModel();
            indexVM.CarouselItems = eshopDbContext.CarouselItems.ToList();
            indexVM.Products = eshopDbContext.Products.ToList();


            return View(indexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
