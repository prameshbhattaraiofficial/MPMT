using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Mpmt.Services.Extensions;
using Mpmt.Services.Hubs;
using Mpmt.Web.Extensions;
using Mpmt.Web.Features.RateLimiting;
using Mpmt.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(opts => opts.Filters.Add(new AuthorizeFilter()));
builder.Services.AddAutoMapper(typeof(WebMappingProfiles).Assembly);
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight;
});

builder.Services.AddCommonPackageLibraries();
builder.Services.AddCommonApplicationServices();
builder.Services.ConfigureCommonApplicationServices(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddHostedServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);

builder.Services.AddAppStaticContentDirectory();

builder.Services.AddHttpClient();
builder.Services.AddSession();

builder.Services.AddSignalR();

// Add rate limiting services
builder.Services.AddAppRateLimiting(builder.Configuration);

var app = builder.Build();

// run tasks during application startup
await app.RunStartupTasksAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/InternalServerError");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<CustomRateLimitingMiddleware>();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";

    await next.Invoke();
});

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseCookiePolicy();
app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAppStaticContentDirectory();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
       name: "areas",
       pattern: "{area=partner}/{controller=Login}/{action=Index}/{id?}"
   );
   
   
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
     );
    endpoints.MapControllerRoute(

       name: "areas",
       pattern: "{area=admin}/{controller=Login}/{action=Index}/{id?}"
   );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});
app.MapHub<UserHub>("/hubs/usercount");

await app.RunAsync();
