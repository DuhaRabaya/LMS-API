using LMS.PL.Data;

namespace LMS.PL
{
    public static class AppConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddControllers();
            services.AddOpenApi();
        }
    }
}
