using Microsoft.Owin;
using Owin;
using WebApplication_dog.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication_dog.Startup))]
namespace WebApplication_dog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            ConfigureAuth(app);
        }
    }
}