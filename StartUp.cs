using ASPLiteBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASPLiteBlog
{
    public class StartUp
    {
        public static async Task RUN(IApplicationBuilder applicationBuilder)
        {
            //create service getter
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //get db service
                var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var _userStore = serviceScope.ServiceProvider.GetService<IUserStore<ApplicationUser>>();
                var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var _emailStore = serviceScope.ServiceProvider.GetService<IUserEmailStore<ApplicationUser>>();
                

                //make sure the db is created
                //context.Database.EnsureCreated();
                //this isn't needed?

                //migrate with all the changes
                _context.Database.Migrate();

                //check if there are any users in DB
                if (!_context.Users.Any())
                {
                    //create admin
                    var user = Activator.CreateInstance<ApplicationUser>();
                    await _userStore.SetUserNameAsync(user, "admin", CancellationToken.None);
                    //_emailStore.SetEmailAsync(user, "admin@admin.com", CancellationToken.None);
                    user.Email = "admin@admin.com";
                    user.NormalizedEmail = "admin@admin.com";

                    user.EmailConfirmed = true;//short circuiting a bit here

                    user.firstName = "admin";
                    user.lastName = "admin";

                    await _userManager.CreateAsync(user, "Admin123!");
                }
            }
        }
    }
}
