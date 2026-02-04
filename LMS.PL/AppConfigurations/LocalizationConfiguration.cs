using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace LMS.PL.AppConfigurations
{
    public static class LocalizationConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "");

            const string defaultCulture = "en";

            var supportedCultures = new[]
            {
              new CultureInfo(defaultCulture),
              new CultureInfo("ar")
            };

            services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                {
                    QueryStringKey = "lang"
                });
            });
        }
    }
}
