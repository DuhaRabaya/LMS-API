using LMS.BLL.Services.AuthenticationServices;
using LMS.BLL.Services.CheckoutServices;
using LMS.BLL.Services.CourseServices;
using LMS.BLL.Services.EmailServices;
using LMS.BLL.Services.EnrollmentsServices;
using LMS.BLL.Services.FileServices;
using LMS.BLL.Services.ManagerServices;
using LMS.BLL.Services.RefundServices;
using LMS.BLL.Services.TokenService;
using LMS.DAL.Models;
using LMS.DAL.Repository;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.SignIn.RequireConfirmedEmail = true;
            }
            )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
       
            //seed data
            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddScoped<ISeedData, UserSeedData>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IManageUserService, ManageUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IStripeRefundService, StripeRefundService>();

        }
    }
}
