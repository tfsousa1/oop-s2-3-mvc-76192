using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Faculty", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(
                userManager,
                "admin@acmeglobal.ie",
                "AcmeVGC!26",
                "Admin");

            await CreateUserIfNotExistsAsync(
                userManager,
                "albus.dumbledore@acmeglobal.ie",
                "Dumbledore!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "severus.snape@acmeglobal.ie",
                "Snape!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "minerva.mcgonagall@acmeglobal.ie",
                "McGonagall!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "hermione.granger@acmeglobal.ie",
                "Hermione!26",
                "Student");

            await CreateUserIfNotExistsAsync(
                userManager,
                "harry.potter@acmeglobal.ie",
                "Harry!26",
                "Student");

            await CreateUserIfNotExistsAsync(
                userManager,
                "luna.lovegood@acmeglobal.ie",
                "Luna!26",
                "Student");
        }

        private static async Task CreateUserIfNotExistsAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }

                return;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}