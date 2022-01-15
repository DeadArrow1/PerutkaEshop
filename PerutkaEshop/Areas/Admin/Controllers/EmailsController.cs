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
    public class EmailsController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public EmailsController(EshopDbContext eshopDB, IWebHostEnvironment env)
        {
            this.env = env;
            eshopDbContext = eshopDB;
        }

        public IActionResult Select()
        {
            IList<Email> Emails = eshopDbContext.Emails.ToList();
            return View(Emails);

        }
       

       
        
        public IActionResult Edit(int ID)
        {
            Email ciFromDatabase = eshopDbContext.Emails.FirstOrDefault(ci => ci.ID == ID);

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
        public async Task<IActionResult> Edit(Email email)
        {
            Email ciFromDatabase = eshopDbContext.Emails.FirstOrDefault(ci => ci.ID == email.ID);

            if (ciFromDatabase != null)
            {  
                ModelState.Clear();
                TryValidateModel(email);
                if (ModelState.IsValid)
                {

                    ciFromDatabase.Body = email.Body;
                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(EmailsController.Select));
                }
                return View(email);
            }
            else
            {
                return NotFound();
            }


        }

        
    }
}
