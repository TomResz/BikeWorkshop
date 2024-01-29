using BikeWorkshop.API.SwaggerDoc;
using BikeWorkshop.Application;
using BikeWorkshop.Infrastructure;
using BikeWorkshop.Shared;
using Serilog;
using Spectre.Console;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocSettings();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddShared();

builder.Host.UseSerilog((context, configuration) =>
	configuration.WriteTo.Console()
	.MinimumLevel.Information());
AnsiConsole.Write(new FigletText("BikeWorkshop API")
	.LeftJustified()
	.Color(Color.Green));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseShared();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
