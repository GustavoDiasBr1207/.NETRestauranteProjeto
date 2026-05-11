using RestaurantOrders.Application;
using RestaurantOrders.Infrastructure;
using RestaurantOrders.API.Extensions;
using RestaurantOrders.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration); // ← passa IConfiguration completo

builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddAuth();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>(); 
app.UseAuthentication();                         
app.UseAuthorization();

app.MapControllers();

app.Run();