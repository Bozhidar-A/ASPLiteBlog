using ASPLiteBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace ASPLiteBlog
{
    public class StartUp
    {
        public static void RUN(IApplicationBuilder applicationBuilder)
        {
            //create service getter
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //get db service
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                //make sure the db is created
                context.Database.EnsureCreated();

                //migrate with all the changes
                context.Database.Migrate();
            }
        }
    }
}
