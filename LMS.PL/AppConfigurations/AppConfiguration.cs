using LMS.PL.Data;

namespace LMS.PL.AppConfigurations
{
    public static class AppConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

       

        }
    }
}
