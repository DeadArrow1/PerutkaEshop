using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Perutka.Eshop.Web.Models.Entity;
using Perutka.Eshop.Web.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Perutka.Eshop.Web.Models.Database.Configuration;

namespace Perutka.Eshop.Web.Models.database
{
    public class EshopDbContext : IdentityDbContext<User,Role,int>
    {
        public DbSet<CarouselItem> CarouselItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public EshopDbContext(DbContextOptions options) : base(options)
        { 
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Order>(new OrderConfiguration());

            var entityTypes = builder.Model.GetEntityTypes();
            foreach (var entity in entityTypes)
            {
                entity.SetTableName(entity.GetTableName().Replace("AspNet", ""));
            }
        }
    }
}
