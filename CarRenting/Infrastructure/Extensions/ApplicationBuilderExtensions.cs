using CarRenting.Data;
using CarRenting.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static CarRenting.WebConstants;

namespace CarRenting.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase( this IApplicationBuilder app)
        {

            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            //TestSeedUser(services);

            SeedCategories(services);

            SeedAdministartor(services);

            return app;
        }

        private static void TestSeedUser(IServiceProvider services)
        {
            Task.Run(async () =>
            {
                var newUser = new User
                {
                    Email = "Antark@abv.bg",
                    UserName = "Antark@abv.bg",
                    FullName = "Antark"
                };

                await services.GetRequiredService<UserManager<User>>().CreateAsync(newUser, "antark");
            })
                            .GetAwaiter()
                            .GetResult();
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<CarRentingDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<CarRentingDbContext>();

            if (data.Categories.Any()) return;

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Mini"},
                new Category { Name = "Economy"},
                new Category { Name = "Midsize"},
                new Category { Name = "Large"},
                new Category { Name = "SUV"},
                new Category { Name = "Vans"},
                new Category { Name = "Luxury"},
            });

            data.SaveChanges();
        }

        private static void SeedAdministartor(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                 .Run(async () =>
                 {
                     if (await roleManager.RoleExistsAsync(AdministartorRoleName))
                     {
                         return;
                     }

                     var role = new IdentityRole { Name = AdministartorRoleName };

                     await roleManager.CreateAsync(role);

                     const string adminEmail = "admin@crs.com";
                     const string adminPassword = "admin1234";

                     var user = new User
                     {
                         Email = adminEmail,
                         UserName = adminEmail,
                         FullName = "Admin"
                     };

                     await userManager.CreateAsync(user, adminPassword);

                     await userManager.AddToRoleAsync(user, role.Name);
                 })
                 .GetAwaiter()
                 .GetResult();
        }
    }
}
