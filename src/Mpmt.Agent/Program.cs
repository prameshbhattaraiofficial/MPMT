using AspNetCoreHero.ToastNotification;
using Mpmt.Agent.Extensions;
using Mpmt.Agent.Infrastructure;
using Mpmt.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AgentMappingProfiles).Assembly);
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight;
});

builder.Services.AddCommonPackageLibraries();
builder.Services.AddAgentAuthenticationService(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAppStaticContentDirectory();
builder.Services.AddHttpClient();

var app = builder.Build();
await app.RunStartupTasksAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");
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
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}"
    );
});
   

app.Run();
