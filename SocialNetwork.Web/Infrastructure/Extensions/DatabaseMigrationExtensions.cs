using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DataModel;
using SocialNetwork.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Web.Infrastructure.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SocialNetworkDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                Task.Run(async () =>
                {
                    foreach (var role in GlobalConstants.UserRoles)
                    {
                        var roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }

                    var adminEmail = "admin@admin.com";
                    var adminUsername = "admin";
                    var adminExists = await userManager.FindByEmailAsync(adminEmail);

                    if (adminExists == null)
                    {
                        var admin = new User
                        {
                            Email = adminEmail,
                            UserName = adminUsername,
                            FirstName = "John",
                            LastName = "Doe"
                        };

                        await userManager.CreateAsync(admin, "pass123");
                        await userManager.AddToRoleAsync(admin, GlobalConstants.UserRole.Administrator);
                    }

                }).Wait();
            }


            return app;
        }
    }
}
