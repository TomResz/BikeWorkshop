using BikeWorkshop.API.Extensions;
using BikeWorkshop.API.SwaggerDoc;
using BikeWorkshop.Application;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure;
using BikeWorkshop.Shared;
using Serilog;
using Spectre.Console;

var builder = WebApplication.CreateBuilder(args);
var connectionString = ConnectionStringExtension.GetConnectionString(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocSettings();

builder.Services
	.AddInfrastructure(builder.Configuration,connectionString)
	.AddApplication()
	.AddShared();

builder.Host.UseSerilog((context, configuration) =>
	configuration.WriteTo.Console()
	.MinimumLevel.Information());

AnsiConsole.Write(new FigletText("BikeWorkshop API")
	.LeftJustified()
	.Color(Color.Green));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.ApplyMigration();
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseShared();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


public partial class Program { }