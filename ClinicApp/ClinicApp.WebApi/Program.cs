using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

[assembly:ApiController]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.RegisterDatabase();
builder.RegisterServices();
builder.ConfigureAuthorization();

var app = builder.Build();

app.MigrateDatabase();
await app.AddDefaultAdmin();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }