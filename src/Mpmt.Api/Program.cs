using Microsoft.AspNetCore.Authorization;
using Mpmt.Api;
using Mpmt.Api.Extensions;
using Mpmt.Api.Features.AuthenticationSchemes.AgentApi;
using Mpmt.Api.Features.AuthenticationSchemes.PartnerApi;
using Mpmt.Api.Middleware;
using Mpmt.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Partner api authentication scheme
builder.Services.AddAuthentication()
    .AddPartnerApiAuthentication(PartnerApiAuthenticationOptions.DefaultScheme, opts => { })
    .AddAgentApiAuthentication(AgentApiAuthenticationOptions.DefaultScheme, opts => { });

// Partner api authentication scheme inclusion in authorization policy
builder.Services.AddAuthorization(opts =>
{
    opts.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(PartnerApiAuthenticationOptions.DefaultScheme, AgentApiAuthenticationOptions.DefaultScheme)
        .Build();
});

builder.Services.AddCommonPackageLibraries();
builder.Services.AddCommonApplicationServices();
builder.Services.ConfigureCommonApplicationServices(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddHostedServices();
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.AddApplicationCorsPolicies(builder.Configuration);
builder.Services.AddAppStaticContentDirectory();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("errors/{0}");

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(MpmtApiDefaults.DefaultCorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAppStaticContentDirectory();

app.MapControllers();

await app.RunAsync();
