using Microsoft.PWABuilder.Oculus.Models;
using Microsoft.PWABuilder.Oculus.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TempDirectory>();
builder.Services.AddTransient<ProcessRunner>();
builder.Services.AddTransient<ZombieProcessKiller>();
builder.Services.AddTransient<OculusCliWrapper>();
builder.Services.AddTransient<OculusPackageCreator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
