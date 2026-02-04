using LMS.DAL.Models;
using LMS.DAL.Utils;
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

            //seed data
            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddScoped<ISeedData, UserSeedData>();

            



        }
    }
}
