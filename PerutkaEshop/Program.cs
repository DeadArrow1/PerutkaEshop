using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Perutka.Eshop.Web.Models.database;
using Perutka.Eshop.Web.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perutka.Eshop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                if (scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    var dbContext = scope.ServiceProvider.
                        GetRequiredService<EshopDbContext>();
                    DatabaseInit dbInit = new DatabaseInit();
                    dbInit.Initialize(dbContext);


                    var roleManager = scope.ServiceProvider.
                        GetRequiredService<RoleManager<Role>>();
                    Task task = dbInit.EnsureRoleCreated(roleManager);
                    task.Wait();
                    task.Dispose();

                    var userManager = scope.ServiceProvider.
                      GetRequiredService<UserManager<User>>();
                    task=dbInit.EnsureAdminCreated(userManager);
                    task.Wait();
                    task.Dispose();
                    task =dbInit.EnsureManagerCreated(userManager);
                    task.Wait();
                    task.Dispose();
                }
            }

                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging(logginBuilder=> {
                logginBuilder.ClearProviders();
                logginBuilder.AddConsole();
                logginBuilder.AddDebug();
                logginBuilder.AddFile("Logs/eshop-log-{Date}.txt");
            
            });
    }
}
