using AspNetCoreRateLimit;
using Mpmt.Services.Extensions;
using Mpmt.PublicApi.Extensions;
using Mpmt.PublicApi.Middleware;
using Mpmt.PublicApi.Features.RateLimiting;
using Mpmt.PublicApi;
using Mpmt.PublicApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ApiMappingProfiles).Assembly);
builder.Services.AddCommonPackageLibraries();

builder.Services.AddApplicationServices();
builder.Services.AddHostedServices();
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.AddApplicationCorsPolicies(builder.Configuration);

// Add rate limiting services
//builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
//builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services.AddSingleton<IIpPolicyStore, LazyCacheCachePolicyStore>(_ => new LazyCacheCachePolicyStore());
builder.Services.AddSingleton<IRateLimitCounterStore, LazyCacheRateLimitCounterStore>(_ => new LazyCacheRateLimitCounterStore());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("errors/{0}");

app.UseMiddleware<CustomRateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseCors(MpmtPublicApiDefaults.DefaultCorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();