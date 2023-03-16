using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.PWABuilder.Oculus.Models;
using Microsoft.PWABuilder.Oculus.Services;

var builder = WebApplication.CreateBuilder(args);
var allowedOriginsPolicyName = "allowedOrigins";
var allowedOrigins = new[]
{
    "https://www.pwabuilder.com",
    "https://pwabuilder.com",
    "https://preview.pwabuilder.com",
    "https://localhost:3333",
    "https://localhost:3000",
    "http://localhost:3333",
    "http://localhost:3000",
    "https://localhost:8000",
    "http://localhost:8000",
    "https://nice-field-047c1420f.azurestaticapps.net"
};
var appSettings = builder.Configuration.GetSection("AppSettings");
var aiOptions = setUpAppInsights(appSettings);

builder.Services.Configure<AppSettings>(appSettings);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOriginsPolicyName, builder => builder
        .SetIsOriginAllowed(o => allowedOrigins.Any(o => o.Contains(o, StringComparison.OrdinalIgnoreCase)))
        .AllowAnyHeader()
        .AllowAnyMethod());
});
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddTransient<TempDirectory>();
builder.Services.AddTransient<Analytics>();
builder.Services.AddTransient<ProcessRunner>();
builder.Services.AddTransient<ZombieProcessKiller>();
builder.Services.AddTransient<OculusCliWrapper>();
builder.Services.AddTransient<KeyToolWrapper>();
builder.Services.AddTransient<OculusPackageCreator>();
builder.Services.AddApplicationInsightsTelemetry(aiOptions);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(allowedOriginsPolicyName);
app.UseAuthorization();
app.MapControllers();

app.Run();

static ApplicationInsightsServiceOptions setUpAppInsights(IConfigurationSection appSettings)
{
    var connectionString = appSettings["ApplicationInsightsConnectionString"];
    var aiOptions = new ApplicationInsightsServiceOptions();
    aiOptions.EnableRequestTrackingTelemetryModule = false;
    aiOptions.EnableDependencyTrackingTelemetryModule = true;
    aiOptions.EnableHeartbeat = false;
    aiOptions.EnableAzureInstanceMetadataTelemetryModule = false;
    aiOptions.EnableActiveTelemetryConfigurationSetup = false;
    aiOptions.EnableAdaptiveSampling = false;
    aiOptions.EnableAppServicesHeartbeatTelemetryModule = false;
    aiOptions.EnableAuthenticationTrackingJavaScript = false;
    aiOptions.ConnectionString = connectionString;
    return aiOptions;
}