using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace WebApplication_dog.Models.Migrations
{
    public class Configuration : DbMigrationsConfiguration<WebApplication_dog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(WebApplication_dog.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new IdentityRole("User"));
            }

            if (!context.Users.Any(u => u.Email == "admin@example.com"))
            {
                var admin = new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true };
                userManager.Create(admin, "Admin@123");
                userManager.AddToRole(admin.Id, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "user@example.com"))
            {
                var user = new ApplicationUser { UserName = "user@example.com", Email = "user@example.com", EmailConfirmed = true };
                userManager.Create(user, "User@123");
                userManager.AddToRole(user.Id, "User");
            }
        }
    }
}