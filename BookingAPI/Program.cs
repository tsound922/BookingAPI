using BookingAPI.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var AllowSpecificOrigins = "BookingAPICORSRules";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sg =>
{
    sg.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking API", Version = "v1" });
});


//CORS policy setting is here
//If you want to integrate with Frontend App, set whitelist and/or rules in this code block
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7042", "http://localhost:8080")
                                            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();

                      });
});


builder.Services.AddTransient<IBookingRepository, BookingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(sui => sui.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingAPI V1"));
}


app.UseCors(AllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
