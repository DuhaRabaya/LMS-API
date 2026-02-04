using LMS.PL.AppConfigurations;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace LMS.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppConfiguration.Config(builder.Services); 
            LocalizationConfiguration.Config(builder.Services);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();
            //localization 
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
          
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
