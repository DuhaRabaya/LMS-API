using LMS.DAL.Utils;
using LMS.PL.AppConfigurations;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
namespace LMS.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            //seed data 
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();

                foreach (var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }

            app.Run();
        }
    }
}
