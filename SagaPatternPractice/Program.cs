using Microsoft.EntityFrameworkCore;
using SagaPatternPractice.Data;
using SagaPatternPractice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<CargoService>();
builder.Services.AddScoped<OrderSagaService>(); 


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
