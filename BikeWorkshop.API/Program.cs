using BikeWorkshop.API.SwaggerDoc;
using BikeWorkshop.Application;
using BikeWorkshop.Infrastructure;
using BikeWorkshop.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocSettings();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddShared();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
