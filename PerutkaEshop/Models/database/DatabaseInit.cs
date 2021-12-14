using Microsoft.AspNetCore.Identity;
using Perutka.Eshop.Web.Models.Entity;
using Perutka.Eshop.Web.Models.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Perutka.Eshop.Web.Models.database
{
    public class DatabaseInit
    {
        public async Task EnsureRoleCreated(RoleManager<Role> roleManager)
        {
            string[] roles = Enum.GetNames(typeof(Roles));

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new Role(role));
            }
        }

        public async Task EnsureAdminCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "admin",
                Email = "admin@admin.cz",
                EmailConfirmed = true,
                FirstName = "jmeno",
                LastName = "prijmeni"
            };
            string password = "abc";

            User adminInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (adminInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Admin: {error.Code}, {error.Description}");
                    }
                }
            }

        }

        public async Task EnsureManagerCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "manager",
                Email = "manager@manager.cz",
                EmailConfirmed = true,
                FirstName = "jmeno",
                LastName = "prijmeni"
            };
            string password = "abc";

            User managerInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (managerInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        if (role != Roles.Admin.ToString())
                            await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Manager: {error.Code}, {error.Description}");
                    }
                }
            }

        }


        public void Initialize(EshopDbContext eshopDbContext)
        {
            eshopDbContext.Database.EnsureCreated();

            if(eshopDbContext.CarouselItems.Count()==0)
            {
                IList<CarouselItem> carouselItems = GenerateCarouselItems();
                foreach (var ci in carouselItems)
                {
                    eshopDbContext.CarouselItems.Add(ci);
                }
                eshopDbContext.SaveChanges();
            }

            if (eshopDbContext.Products.Count() == 0)
            {
                IList<Product> products = GenerateProducts();
                foreach (var ci in products)
                {
                    eshopDbContext.Products.Add(ci);
                }
                eshopDbContext.SaveChanges();
            }
        }
        
        public List<CarouselItem> GenerateCarouselItems()
        {
            List<CarouselItem> carouselItems = new List<CarouselItem>();


            CarouselItem ci1 = new CarouselItem()
            {
                
                ImageSource = "/img/What_is_Information_Technology.jpg",
                ImageAlt = "First slide"
            };
            CarouselItem ci2 = new CarouselItem()
            {
                
                ImageSource = "/img/Information-Technology-thumbnail.jpg",
                ImageAlt = "Second slide"
            };
            CarouselItem ci3 = new CarouselItem()
            {
                
                ImageSource = "/img/how-to-become-an-information-technology-specialist160684886950141.jpg",
                ImageAlt = "Third slide"
            };
            carouselItems.Add(ci1);
            carouselItems.Add(ci2);
            carouselItems.Add(ci3);

            return carouselItems;
        }

        public List<Product> GenerateProducts()
        {
            List<Product> products = new List<Product>();


            Product ci1 = new Product()
            {
                ID = 10,
                Name = "Product1",
                Number = 5,
                Price = 25,
                Description = "",
                ImageSource = "~/img/vincent.gif",
                ImageAlt = "vincent"
            };
            Product ci2 = new Product()
            {
                ID = 11,
                Name = "Product2",
                Number = 5,
                Price = 25,
                Description = "",
            ImageSource = "~/img/vincent.gif",
            ImageAlt = "vincent"
        };

    Product ci3 = new Product()
    {
        ID = 12,
        Name = "Product3",
        Number = 4,
        Price = 25,
        Description = "",
        ImageSource = "~/img/vincent.gif",
        ImageAlt = "vincent"
    };

            products.Add(ci1);
            products.Add(ci2);
            products.Add(ci3);

            return products;
        }
    }
}
