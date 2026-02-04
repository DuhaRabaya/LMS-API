using LMS.DAL.Models;
using LMS.PL.Data;
using Microsoft.AspNetCore.Identity;

namespace LMS.PL.AppConfigurations
{
    public static class AppConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            //exception handling
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            //identity configuration
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); 
       

        }
    }
}
