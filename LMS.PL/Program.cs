
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppConfiguration.Config(builder.Services);
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

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
