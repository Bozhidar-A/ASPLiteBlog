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
                var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();


                //make sure the db is created
                //context.Database.EnsureCreated();
                //this isn't needed?

                //migrate with all the changes
                _context.Database.Migrate();

                //creating roles
                var adminRole = new IdentityRole("admin");
                var writerRole = new IdentityRole("writer");

                if (!_context.Roles.Any())
                {
                    //added them to DB
                    await _roleManager.CreateAsync(adminRole);
                    await _roleManager.CreateAsync(writerRole);
                }

                //check if there are any users in DB
                if (!_context.Users.Any())
                {
                    //create admin
                    var adminUser = Activator.CreateInstance<ApplicationUser>();
                    await _userStore.SetUserNameAsync(adminUser, "admin", CancellationToken.None);

                    //short circuiting a bit here
                    //policy needs both a strong password AND confirmed email. Can change that, but I prefer to keep it on
                    adminUser.Email = "admin@admin.com";
                    adminUser.NormalizedEmail = "admin@admin.com";
                    adminUser.EmailConfirmed = true;

                    adminUser.firstName = "admin";
                    adminUser.lastName = "admin";

                    //create user
                    await _userManager.CreateAsync(adminUser, "Admin123!");

                    //add to role
                    await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }
        }
    }
}
