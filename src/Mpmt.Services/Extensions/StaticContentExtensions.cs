using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Mpmt.Core.Configuration;

namespace Mpmt.Services.Extensions
{
    public static class StaticContentExtensions
    {
        public static IServiceCollection AddAppStaticContentDirectory(this IServiceCollection services)
        {
            services.AddOptions<StaticContentConfig>()
                .BindConfiguration("Static")
                .ValidateDataAnnotations();

            return services;
        }

        public static WebApplication UseAppStaticContentDirectory(this WebApplication app)
        {
            var staticContentConfig = app.Services.GetRequiredService<IOptions<StaticContentConfig>>().Value;
            if (!Directory.Exists(staticContentConfig.UserDataDirectory))
                Directory.CreateDirectory(staticContentConfig.UserDataDirectory);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.TrimEndingDirectorySeparator(staticContentConfig.UserDataDirectory)),
            });

            return app;
        }
    }
}
